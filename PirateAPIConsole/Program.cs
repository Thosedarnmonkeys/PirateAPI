using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PirateAPI;
using PirateAPI.Logging;
using PirateAPI.WebClient;
using System.Reflection;

namespace PirateAPIConsole
{
  public class Program
  {
    #region private consts
    private const string iniFileName = "PirateAPIConsole.ini";
    #endregion

    #region Main
    static void Main(string[] args)
    {
      //PirateApiHost vals
      string webRoot = "";
      int port = 8080;
      List<string> locationPref = new List<string> { "uk" };
      List<string> blackList = new List<string>() { };
      TimeSpan proxyRefreshInterval = new TimeSpan(1, 0, 0);
      bool magnetSearchProxiesOnly = false;
      int apiLimit = 30;
      ILogger logger = new FileAndConsoleLogger(Environment.CurrentDirectory + Path.DirectorySeparatorChar + "PirateAPILog.txt");
      IWebClient webClient = new BasicWebClient(logger);


      //create waitHandle
      bool createdNew = false;
      EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, "PirateAPIConsoleWaitHandleEvent", out createdNew);
      if (!createdNew)
      {
        logger.LogError("waitHandle already exists, terminating existing process...");
        waitHandle.Set();
        waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, "PirateAPIConsoleWaitHandleEvent", out createdNew);
        if (!createdNew)
        {
          logger.LogError("Tried to create new waitHandle, but couldn't");
          return;
        }
      }

      //start PirateAPIHost
      PirateAPIHost host = new PirateAPIHost(webRoot, port, locationPref, blackList, proxyRefreshInterval, magnetSearchProxiesOnly, apiLimit, logger, webClient);
      host.StartServing();

      //wait untill signaled to stop
      bool signaledToStop;
      do
      {
        signaledToStop = waitHandle.WaitOne();
      } while (!signaledToStop);
    }
    #endregion

    #region private methods
    private Dictionary<string, string> ReadIniFileOptions()
    {
      string iniFilePath = Environment.CurrentDirectory + Path.DirectorySeparatorChar + iniFileName;

      if (!File.Exists(iniFilePath))
        CreateDefaultIniFile();

      return new Dictionary<string, string>();
    }

    private void CreateDefaultIniFile()
    {
      string iniFilePath = Environment.CurrentDirectory + Path.DirectorySeparatorChar + iniFileName;
      Assembly assembly = Assembly.GetExecutingAssembly();
      using (Stream stream = assembly.GetManifestResourceStream("PirateAPIConsole.PirateAPIConsole.ini"))
      {
        using (StreamReader reader = new StreamReader(stream))
        {
          string defaultIniFileContents = reader.ReadToEnd();
          File.WriteAllText(iniFilePath, defaultIniFileContents);
        }
      }
    }
    #endregion
  }
}
