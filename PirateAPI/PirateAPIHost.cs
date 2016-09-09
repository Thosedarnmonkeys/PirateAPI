using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PirateAPI.WebServer;
using PirateAPI.Logging;
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
    #endregion

    #region private fields
    private IWebServer webServer;
    private ILogger logger;
    #endregion

    #region constructor
    public PirateAPIHost(string webRoot, int port, List<string> proxyLocationPreferences, TimeSpan proxyResfreshInterval, ILogger logger, IWebClient webClient)
    {
      WebRoot = webRoot;
      Port = port;
      ProxyLocationPrefs = proxyLocationPreferences;
    }
    #endregion

    #region public methods
    public void StartServing()
    {
      
    }
    #endregion
  }
}
