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
    #region Main
    static void Main(string[] args)
    {
      ConsoleLogger logger = new ConsoleLogger();

      //create waitHandle
      bool createdNew;
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
      PirateAPIHost host = PirateAPIHostBuilder.Build();
      host.StartServing();

      //wait untill signaled to stop
      bool signaledToStop;
      do
      {
        signaledToStop = waitHandle.WaitOne();
      } while (!signaledToStop);
    }
    #endregion
  }
}
