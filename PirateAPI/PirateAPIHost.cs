using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Win32;
using PirateAPI.WebServer;
using PirateAPI.Logging;
using PirateAPI.Parsers.Torrents;
using PirateAPI.Parsers.Torznab;
using PirateAPI.Properties;
using PirateAPI.ProxyPicker;
using PirateAPI.ProxyProviders;
using PirateAPI.ProxyProviders.ThePirateBayProxyList;
using PirateAPI.RequestResolver;
using PirateAPI.ResponseBuilders.Caps;
using PirateAPI.ResponseBuilders.Torznab;
using PirateAPI.WebClient;
using PirateAPI.EventArgTypes;
using Timer = System.Timers.Timer;

namespace PirateAPI
{
  public class PirateAPIHost
  {
    //Broad workflow:

    //On StartServing() Proxypicker picks proxy to use, refreshes at specified interval or if there is error

    //BasicWebServer gets request
    //TorznabQueryParser parses Torznab string to PirateRequest obj
    //PirateRequestResolver takes PirateRequest, returns list of Torrent obj
    //TorznabResponseBuilder takes list of torrents, returns torznab string
    //BasicWebserver returns torznab string

    #region public events 
    public event EventHandler<ProxyUpdatedEventArgs> ProxyUpdated;
    #endregion

    #region public properties
    public string WebRoot { get; private set; }
    public int Port { get; private set; }
    public List<string> ProxyLocationPrefs { get; private set; }
    public TimeSpan ProxyRefreshInterval { get; private set; }
    public bool MagnetSearchProxiesOnly { get; set; }
    public Proxy BestProxy
    {
      get { return bestProxy; }
      private set
      {
        if (value == null)
          return;

        if (bestProxy == null)
        {
          bestProxy = value;
          ProxyUpdated?.Invoke(this, new ProxyUpdatedEventArgs() { Proxy = bestProxy });
        }

        if (!bestProxy.Equals(value))
        {
          bestProxy = value;
          ProxyUpdated?.Invoke(this, new ProxyUpdatedEventArgs() {Proxy = bestProxy});
        }
      }
    }

    public LoggingMode LoggingMode
    {
      get
      {
        if (logger == null)
          return LoggingMode.File;

        if (logger is FileAndConsoleLogger)
          return LoggingMode.FileAndConsoleWindow;

        if (logger is FileLogger)
          return LoggingMode.File;

        if (logger is ConsoleLogger)
          return LoggingMode.ConsoleWindow;
        
        return LoggingMode.File;
      }
    }
    #endregion

    #region private fields
    private IWebServer webServer;
    private ILogger logger;
    private IWebClient webClient;
    private PirateRequestResolveStrategy resolveStrategy;
    private PirateProxyPicker proxyPicker;
    private Proxy bestProxy;
    private List<Proxy> proxies;
    private Timer refreshProxiesTimer;
    private int apiLimit;
    private string emptyTorznabResponse = Resources.TorznabResponseTemplate.Replace("{0}", "");
    private int requestTimeOut;
    #endregion

    #region constructor
    public PirateAPIHost(string webRoot, int port, List<string> proxyLocationPreferences, List<string> blacklistedProxies, TimeSpan proxyResfreshInterval, bool magnetSearchProxiesOnly, int apiLimit, PirateRequestResolveStrategy resolveStrategy, int requestTimeOut, ILogger logger, IWebClient webClient)
    {
      WebRoot = webRoot;
      Port = port;
      ProxyLocationPrefs = proxyLocationPreferences;
      ProxyRefreshInterval = proxyResfreshInterval;
      MagnetSearchProxiesOnly = magnetSearchProxiesOnly;
      this.apiLimit = apiLimit;
      this.resolveStrategy = resolveStrategy;
      this.logger = logger;
      this.webClient = webClient;
      this.requestTimeOut = requestTimeOut;
      proxyPicker = new PirateProxyPicker(proxyLocationPreferences, blacklistedProxies, logger);
    }
    #endregion

    #region public methods
    public bool StartServing()
    {
      logger.LogMessage("PirateAPI is starting...");
      IProxyProvider proxyProvider = new ThePirateBayProxyListProvider(logger, webClient);
      proxies = MagnetSearchProxiesOnly ? proxyProvider.ListMagnetInSearchProxies() : proxyProvider.ListProxies();
      if (proxies == null)
      {
        logger.LogError("Failed to start serving: ThePirateBayProxyListProvider." + (MagnetSearchProxiesOnly ? "ListMagnetInSearchProxies" : "ListProxies") + " returned null");
        return false;
      }

      BestProxy = proxyPicker.BestProxy(proxies);

      refreshProxiesTimer = new Timer(ProxyRefreshInterval.TotalMilliseconds);
      refreshProxiesTimer.Elapsed += OnRefreshProxiesTimerInterval;
      refreshProxiesTimer.Start();

      webServer = new BasicWebServer(WebRoot, Port, ProcessRequest, logger);
      if (!webServer.StartServing())
      {
        logger.LogError("Failed to start serving: BasicWebServer.StartServing returned false");
        return false;
      }

      return true;
    }

    public bool StopServing()
    {
      refreshProxiesTimer.Stop();

      try
      {
        webServer.StopServing();
      }
      catch (Exception)
      {
        logger.LogError("WebServer did not shut down cleanly");
        return false;
      }

      return true;
    }

    public void RefreshProxies()
    {
      IProxyProvider proxyProvider = new ThePirateBayProxyListProvider(logger, webClient);
      proxies = proxyProvider.ListProxies();
      if (proxies == null)
      {
        logger.LogError("Failed to refresh proxies: ThePirateBayProxyListProvider.ListProxies returned null");
        return;
      }

      proxyPicker.ClearTempBlacklist();
      BestProxy = proxyPicker.BestProxy(proxies);
    }

    public void ReportSettings()
    {
      logger.LogMessage("Settings loaded");
      logger.LogMessage($"Web Root: {WebRoot}");
      logger.LogMessage($"Port: {Port}");
      logger.LogMessage($"Proxy Location Prefs: {string.Join(", ", ProxyLocationPrefs)}");
      logger.LogMessage($"Proxy Refresh Interval: {ProxyRefreshInterval.Days} Days, {ProxyRefreshInterval.Hours} Hours, {ProxyRefreshInterval.Minutes} Minutes, {ProxyRefreshInterval.Seconds} Seconds");
      logger.LogMessage($"Use Proxies With Magnets InSearch Only: {MagnetSearchProxiesOnly}");
      logger.LogMessage($"Max Search Results: {apiLimit}");
      logger.LogMessage($"Request Resolve Mode: {resolveStrategy}");
      logger.LogMessage($"Request Timeout in Millis: {webClient.TimeoutMillis}");
    }
    #endregion

    #region private methods

    private string ProcessRequest(string request)
    {
      if (string.IsNullOrWhiteSpace(request))
      {
        logger.LogError("PirateApiHost.ProcessRequest was passed a null or whitespace string for request");
        return null;
      }

      TorznabQueryParser torznabParser = new TorznabQueryParser(logger);
      if (!torznabParser.IsValidRequest(request))
      {
        logger.LogMessage($"Ignoring non Torznab request {request}");
        return null;
      }

      switch (torznabParser.DiscernQueryType(request))
      {
        case TorznabQueryType.Caps:
          return ProcessCapsRequest();

        case TorznabQueryType.TvSearch:
          return ProcessTvSearchRequest(request);

        default:
          return null;
      }
    }

    private string ProcessCapsRequest()
    {
      CapsResponseBuilder builder = new CapsResponseBuilder();
      return builder.BuildResponse(apiLimit);
    }

    private string ProcessTvSearchRequest(string request)
    {
      if (string.IsNullOrWhiteSpace(request))
      {
        logger.LogError("PirateApiHost.ProcessTvSearchRequest was passed a null or whitespace string for request");
        return null;
      }

      TorznabQueryParser torznabParser = new TorznabQueryParser(logger);
      if (!torznabParser.IsValidRequest(request))
      {
        logger.LogMessage($"Ignoring non Torznab request {request}");
        return null;
      }

      List<Torrent> torrents = new List<Torrent>();
      TimeSpan timeLimit = new TimeSpan(0, 0, requestTimeOut);
      Task timedTask = Task.Factory.StartNew(() => MakeTvRequests(torznabParser, request, torrents));
      timedTask.Wait(timeLimit);

      if (torrents.Count == 0)
      {
        return emptyTorznabResponse;
      }

      TorznabResponseBuilder responseBuilder = new TorznabResponseBuilder(logger);
      string response = responseBuilder.BuildResponse(torrents);
      if (response == null)
      {
        logger.LogError("TorznabResponseBuilder.BuildReponse returned null, returning empty torznab result");
        return emptyTorznabResponse;
      }

      return response;
    }

    private void MakeTvRequests(TorznabQueryParser torznabParser, string request, List<Torrent> torrents)
    {
      while (torrents.Count == 0)
      {
        //create request using the current best proxy
        PirateRequest pirateRequest = torznabParser.Parse(request, bestProxy.Domain);
        if (pirateRequest == null)
        {
          logger.LogError("TorznabQueryParser.Parse returned null, returning null string");
          return;
        }

        int remainingAttempts = 3;
        while (remainingAttempts > 0)
        {
          //try getting a response from proxy up to 3 times
          PirateRequestResolver requestResolver = new PirateRequestResolver(logger, webClient, apiLimit);
          try
          {
            requestResolver.Resolve(pirateRequest, resolveStrategy, torrents);
          }
          catch (Exception e)
          {
            logger.LogException(e, "PirateRequestResolver.Resolve threw exception");
            logger.LogError("PirateRequestResolver.Resolve threw error, returning empty torznab result");
            return;
          }

          if (torrents.Count != 0)
            break;

          remainingAttempts--;
        }

        //if null here then either proxy never responded or we have a malformed response/request which throws exception
        if (torrents.Count == 0)
        {
          //remove proxy from current pool and try again on next loop using next best proxy
          proxyPicker.TempBlacklistDomain(bestProxy.Domain);
          Proxy proxy = proxyPicker.BestProxy(proxies);
          if (proxy == null)
          {
            logger.LogError("PirateProxyPicker.BestProxy returned null, returning empty torznab result");
            return;
          }
          BestProxy = proxy;
        }
      }
    }

    private void OnRefreshProxiesTimerInterval(object sender, ElapsedEventArgs e)
    {
      RefreshProxies();
    }
    #endregion
  }
}
