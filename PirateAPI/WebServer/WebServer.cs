using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using PirateAPI.Logging;

namespace PirateAPI.WebServer
{
  public class WebServer
  {
    private HttpListener listener;
    private Func<string, string> responseFunc;
    private ILogger logger;

    public WebServer(string webroot, int port, Func<string, string> responseFunc, ILogger logger)
    {
      listener = new HttpListener();
      listener.Prefixes.Add("http://localhost:" + port + webroot + "/");
      this.responseFunc = responseFunc;
      this.logger = logger;
    }

    public bool StartServing()
    {
      try
      {
        listener.Start();
        return true;
      }
      catch (Exception e)
      {
        logger.LogException(e, "WebServer.StartServing threw exception");
        return false;
      }
    }

    public void StopServing()
    {
      try
      {
        listener.Stop();
        listener.Close();
      }
      catch (Exception e)
      {
        logger.LogException(e, "WebServer.StopServing threw exception");
        throw;
      }
    }
  }
}
