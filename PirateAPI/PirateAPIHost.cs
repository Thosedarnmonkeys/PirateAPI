using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Microsoft.Win32;
using PirateAPI.WebServer;
using PirateAPI.Logging;
using PirateAPI.Parsers.Torrents;
using PirateAPI.Parsers.Torznab;
using PirateAPI.ProxyPicker;
using PirateAPI.ProxyProviders;
using PirateAPI.ProxyProviders.ThePirateBayProxyList;
using PirateAPI.RequestResolver;
using PirateAPI.ResponseBuilders.Caps;
using PirateAPI.ResponseBuilders.Torznab;
using PirateAPI.WebClient;

namespace PirateAPI
{
  public class PirateAPIHost
  {
    //Broad workflow:

    //On StartServing() Proxypicker picks proxy to use, refreshes every hour or if there is error

    //WebServer gets request
    //TorznabQueryParser parses Torznab string to PirateRequest obj
    //PirateRequestResolver takes PirateRequest, returns list Torrent obj
    //TorznabResponseBuilder takes list of torrents, returns torznab string
    //Webserver returns torznab string

    #region public properties
    public string WebRoot { get; private set; }
    public int Port { get; private set; }
    public List<string> ProxyLocationPrefs { get; private set; }
    public TimeSpan ProxyRefreshInterval { get; private set; }
    public bool MagnetSearchProxiesOnly { get; set; }
    #endregion

    #region private fields
    private IWebServer webServer;
    private ILogger logger;
    private IWebClient webClient;
    private PirateProxyPicker proxyPicker;
    private Proxy bestProxy;
    private List<Proxy> proxies;
    private Timer refreshProxiesTimer;
    #endregion

    #region constructor
    public PirateAPIHost(string webRoot, int port, List<string> proxyLocationPreferences, List<string> blacklistedProxies, TimeSpan proxyResfreshInterval, bool magnetSearchProxiesOnly, ILogger logger, IWebClient webClient)
    {
      WebRoot = webRoot;
      Port = port;
      ProxyLocationPrefs = proxyLocationPreferences;
      ProxyRefreshInterval = proxyResfreshInterval;
      MagnetSearchProxiesOnly = magnetSearchProxiesOnly;
      this.logger = logger;
      this.webClient = webClient;
      proxyPicker = new PirateProxyPicker(proxyLocationPreferences, blacklistedProxies, logger);
    }
    #endregion

    #region public methods
    public bool StartServing()
    {
      IProxyProvider proxyProvider = new ThePirateBayProxyListProvider(logger, webClient);
      proxies = MagnetSearchProxiesOnly ? proxyProvider.ListMagnetInSearchProxies() : proxyProvider.ListProxies();
      if (proxies == null)
      {
        logger.LogError("Failed to start serving: ThePirateBayProxyListProvider." + (MagnetSearchProxiesOnly ? "ListMagnetInSearchProxies" : "ListProxies") + " returned null");
        return false;
      }

      bestProxy = proxyPicker.BestProxy(proxies);

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
      return builder.BuildResponse();
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

      List<Torrent> torrents = null;
      while (torrents == null)
      {
        //create request using the current best proxy
        PirateRequest pirateRequest = torznabParser.Parse(request, bestProxy.Domain);
        if (pirateRequest == null)
        {
          logger.LogError("TorznabQueryParser.Parse returned null, returning null string");
          return null;
        }

        int remainingAttempts = 3;
        while (remainingAttempts > 0)
        {
          //try getting a response from proxy up to 3 times
          PirateRequestResolver requestResolver = new PirateRequestResolver(logger, webClient);
          try
          {
            torrents = requestResolver.Resolve(pirateRequest);
          }
          catch (Exception e)
          {
            logger.LogException(e, "PirateRequestResolver.Resolve threw exception");
            logger.LogError("PirateRequestResolver.Resolve threw error, returning null string");
            return null;
          }

          if (torrents != null)
            break;

          remainingAttempts--;
        }

        //if null here then either proxy never responded or we have a malformed response/request which throws exception
        if (torrents == null)
        {
          //remove proxy from current pool and try again on next loop using next best proxy
          proxyPicker.TempBlacklistDomain(bestProxy.Domain);
          Proxy proxy = proxyPicker.BestProxy(proxies);
          if (proxy == null)
          {
            logger.LogError("PirateProxyPicker.BestProxy returned null, reutrning null string");
            return null;
          }
          bestProxy = proxy;
        }
      }

      TorznabResponseBuilder responseBuilder = new TorznabResponseBuilder(logger);
      string response = responseBuilder.BuildResponse(torrents);
      if (response == null)
      {
        logger.LogError("TorznabResponseBuilder.BuildReponse returned null, returning null string");
        return null;
      }

      return response;
    }

    private void OnRefreshProxiesTimerInterval(object sender, ElapsedEventArgs e)
    {
      RefreshProxies();
    }

    private void RefreshProxies()
    {
      IProxyProvider proxyProvider = new ThePirateBayProxyListProvider(logger, webClient);
      proxies = proxyProvider.ListProxies();
      if (proxies == null)
      {
        logger.LogError("Failed to refresh proxies: ThePirateBayProxyListProvider.ListProxies returned null");
        return;
      }

      proxyPicker.ClearTempBlacklist();
      bestProxy = proxyPicker.BestProxy(proxies);
    }
    #endregion
  }
}
