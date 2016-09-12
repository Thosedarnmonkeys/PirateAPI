using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirateAPI.Logging
{
  public class FileLogger : ILogger
  {
    private string logFilePath;
    private const string logMessageFormat = "{0}  |  {1}  |  {2}";
    private const string messageText = "Info ";
    private const string errorText = "Error";

    public FileLogger(string path)
    {
      if (string.IsNullOrWhiteSpace(path))
        throw new ArgumentException("Provided filepath was null or empty", nameof(path));

      if (path.Any(c => Path.GetInvalidPathChars().Contains(c)))
        throw new ArgumentException("Provided filepath contains invalid path characters");

      logFilePath = path;
    }

    public void LogMessage(string message)
    {
      if (string.IsNullOrWhiteSpace(message))
        LogError("FileLogger was asked to LogMessage but message was null or whitespace");

      CreateLogFileIfMissing();

      string formattedMessage = string.Format(logMessageFormat, DateTime.Now, messageText, message);
      using (StreamWriter writer = new StreamWriter(logFilePath, true))
      {
        writer.WriteLine(formattedMessage);
      }
    }

    public void LogError(string message)
    {
      if (string.IsNullOrWhiteSpace(message))
        LogError("FileLogger was asked to LogError but message was null or whitespace");

      CreateLogFileIfMissing();

      string formattedMessage = string.Format(logMessageFormat, DateTime.Now, errorText, message);
      using (StreamWriter writer = new StreamWriter(logFilePath, true))
      {
        writer.WriteLine(formattedMessage);
      }
    }

    public void LogException(Exception e, string message = null)
    {
      if (e == null && message == null)
        LogError("FileLogger was asked to LogException but exception and message were null");

      if (e == null && string.IsNullOrWhiteSpace(message))
        LogError("FileLogger was asked to LogException but exception was null and message was whitespace");

      CreateLogFileIfMissing();

      string formattedMessage = string.Format(logMessageFormat, DateTime.Now, errorText, (message ?? "") + (message == null || e == null ? "" : ": ") + e?.Message);
      using (StreamWriter writer = new StreamWriter(logFilePath, true))
      {
        writer.WriteLine(formattedMessage);
      }
    }

    private void CreateLogFileIfMissing()
    {
      if (!File.Exists(logFilePath))
        File.Create(logFilePath);
    }
  }
}
