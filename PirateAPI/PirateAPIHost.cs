using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PirateAPI.WebServer;
using PirateAPI.Logging;

namespace PirateAPI
{
  public class PirateAPIHost
  {
    //broad workflow:

    //Proxypicker picks proxy to use, refreshes every hour or if there is error

    //Webserver gets request
    //Parser parses Torznab to TPB web
    //RequestMaker makes actual request to proxy
    //second parser parses TPB web to Torznab
    //Webserver returns request

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
    public PirateAPIHost(string webRoot, int port, List<string> proxyLocationPreferences)
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
