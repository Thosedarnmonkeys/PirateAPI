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
using PirateAPI.ResponseBuilders;
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
    #endregion

    #region private fields
    private IWebServer webServer;
    private ILogger logger;
    private IWebClient webClient;
    private PirateProxyPicker proxyPicker;
    private Proxy bestProxy;
    private Timer refreshProxiesTimer;
    #endregion

    #region constructor
    public PirateAPIHost(string webRoot, int port, List<string> proxyLocationPreferences, List<string> blacklistedProxies, TimeSpan proxyResfreshInterval, ILogger logger, IWebClient webClient)
    {
      WebRoot = webRoot;
      Port = port;
      ProxyLocationPrefs = proxyLocationPreferences;
      ProxyRefreshInterval = proxyResfreshInterval;
      this.logger = logger;
      this.webClient = webClient;
      proxyPicker = new PirateProxyPicker(proxyLocationPreferences, logger);

      blacklistedProxies?.ForEach(p => proxyPicker.BlacklistDomain(p));
    }
    #endregion

    #region public methods
    public bool StartServing()
    {
      IProxyProvider proxyProvider = new ThePirateBayProxyListProvider(logger, webClient);
      List<Proxy> proxies = proxyProvider.ListProxies();
      if (proxies == null)
      {
        logger.LogError("Failed to start serving: ThePirateBayProxyListProvider.ListProxies returned null");
        return false;
      }

      bestProxy = proxyPicker.BestProxy(proxies);

      refreshProxiesTimer = new Timer(ProxyRefreshInterval.TotalMilliseconds);
      refreshProxiesTimer.Elapsed += OnRefreshProxiesTimerInterval;
      refreshProxiesTimer.Start();

      webServer  = new BasicWebServer(WebRoot, Port, ProcessRequest, logger);
      if (!webServer.StartServing())
      {
        logger.LogError("Failed to start serving: BasicWebServer.StartServing returned false");
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
      PirateRequest pirateRequest = torznabParser.Parse(request, bestProxy.Domain);
      if (pirateRequest == null)
      {
        logger.LogError("TorznabQueryParser.Parse returned null, returning null string");
        return null;
      }

      PirateRequestResolver requestResolver = new PirateRequestResolver(logger, webClient);
      List<Torrent> torrents = requestResolver.Resolve(pirateRequest);
      if (torrents == null)
      {
        logger.LogError("PirateRequestResolver.Resolve returned null, returning null string");
        return null;
      }

      TorznabResponseBuilder responseBuilder = new TorznabResponseBuilder(logger);
      return responseBuilder.BuildResponse(torrents);
    }

    private void OnRefreshProxiesTimerInterval(object sender, ElapsedEventArgs e)
    {
      RefreshProxies();
    }

    private void RefreshProxies()
    {
      IProxyProvider proxyProvider = new ThePirateBayProxyListProvider(logger, webClient);
      List<Proxy> proxies = proxyProvider.ListProxies();
      if (proxies == null)
      {
        logger.LogError("Failed to refresh proxies: ThePirateBayProxyListProvider.ListProxies returned null");
        return;
      }

      bestProxy = proxyPicker.BestProxy(proxies);
    }
    #endregion
  }
}
