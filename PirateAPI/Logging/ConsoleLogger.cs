using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirateAPI.Logging
{
  public class ConsoleLogger : AbstractLogger
  {
    #region public methods
    public override void LogError(string message)
    {
      if (string.IsNullOrWhiteSpace(message))
      {
        LogError("ConsoleLogger.LogError was passed a null or empty string for message");
        return;
      }

      string formattedMessage = FormatErrorMessage(DateTime.Now, message);
      Console.WriteLine(formattedMessage);
    }

    public override void LogException(Exception e, string message = null)
    {
      if (e == null && message == null)
      {
        LogError("ConsoleLogger was asked to LogException but exception and message were null");
        return;
      }

      if (e == null && string.IsNullOrWhiteSpace(message))
      {
        LogError("ConsoleLogger was asked to LogException but exception was null and message was whitespace");
        return;
      }

      string formattedMessage = FormatExceptionMessage(DateTime.Now, e, message);
      Console.WriteLine(formattedMessage);
    }

    public override void LogMessage(string message)
    {
      if (string.IsNullOrWhiteSpace(message))
      {
        LogError("ConsoleLogger was asked to LogMessage but message was null or whitespace");
        return;
      }

      string formattedMessage = FormatMessage(DateTime.Now, message);
      Console.WriteLine(formattedMessage);
    }
    #endregion
  }
}
