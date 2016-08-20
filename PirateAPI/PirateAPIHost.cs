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
    #region public properties
    public string WebRoot { get; private set; }
    public int Port { get; private set; }
    public List<string> ProxyLocationPrefs { get; private set; }
    #endregion

    #region private fields
    private BasicWebServer webServer;
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
