using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirateAPI.Logging
{
  public class FileAndConsoleLogger : FileLogger
  {
    #region private fields
    private ConsoleLogger consoleLogger = new ConsoleLogger();
    #endregion

    #region constructor
    public FileAndConsoleLogger(string path) : base(path) { }
    #endregion

    #region public overrides
    public override void LogMessage(string message)
    {
      base.LogMessage(message);
      consoleLogger.LogMessage(message);
    }

    public override void LogError(string message)
    {
      base.LogError(message);
      consoleLogger.LogError(message);
    }

    public override void LogException(Exception e, string message = null)
    {
      base.LogException(e, message);
      consoleLogger.LogException(e, message);
    }
    #endregion
  }
}
