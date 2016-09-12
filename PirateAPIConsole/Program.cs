using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PirateAPI;
using PirateAPI.Logging;
using PirateAPI.WebClient;

namespace PirateAPIConsole
{
  class Program
  {
    static void Main(string[] args)
    {
      string webRoot = "";
      int port = 8080;
      List<string> locationPref = new List<string> { "uk" };
      List<string> blackList = new List<string>() { };
      TimeSpan proxyRefreshInterval = new TimeSpan(1, 0, 0);
      ILogger logger = new FileLogger(Environment.CurrentDirectory + Path.DirectorySeparatorChar + "PirateAPILog.txt");
      IWebClient webClient = new BasicWebClient(logger);

      PirateAPIHost host = new PirateAPIHost(webRoot, port, locationPref, blackList, proxyRefreshInterval, logger, webClient);

      host.StartServing();

      while (true)
      {
        
      }
    }
  }
}
