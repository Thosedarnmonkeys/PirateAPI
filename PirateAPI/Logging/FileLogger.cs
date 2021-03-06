﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirateAPI.Logging
{
  public class FileLogger : AbstractLogger
  {
    #region private fields
    private string logFilePath;
    private object lockObj = new object();
    #endregion

    #region constructor
    public FileLogger(string path)
    {
      if (string.IsNullOrWhiteSpace(path))
        throw new ArgumentException("Provided logfile path was null or empty", nameof(path));

      if (path.Any(c => Path.GetInvalidPathChars().Contains(c)))
        throw new ArgumentException("Provided logfile path contains invalid path characters");

      logFilePath = path;
    }
    #endregion

    #region public overrides
    public override void LogMessage(string message)
    {
      if (string.IsNullOrWhiteSpace(message))
      {
        LogError("FileLogger was asked to LogMessage but message was null or whitespace");
        return;
      }

      CreateLogFileIfMissing();

      string formattedMessage = FormatMessage(DateTime.Now, message);

      lock (lockObj)
      {
        using (StreamWriter writer = new StreamWriter(logFilePath, true))
        {
          writer.WriteLine(formattedMessage);
        }
      }
    }

    public override void LogError(string message)
    {
      if (string.IsNullOrWhiteSpace(message))
      {
        LogError("FileLogger was asked to LogError but message was null or whitespace");
        return;
      }

      CreateLogFileIfMissing();

      string formattedMessage = FormatErrorMessage(DateTime.Now, message);

      lock (lockObj)
      {
        using (StreamWriter writer = new StreamWriter(logFilePath, true))
        {
          writer.WriteLine(formattedMessage);
        } 
      }
    }

    public override void LogException(Exception e, string message = null)
    {
      if (e == null && message == null)
      {
        LogError("FileLogger was asked to LogException but exception and message were null");
        return;
      }

      if (e == null && string.IsNullOrWhiteSpace(message))
      {
        LogError("FileLogger was asked to LogException but exception was null and message was whitespace");
        return;
      }

      CreateLogFileIfMissing();

      string formattedMessage = FormatExceptionMessage(DateTime.Now, e, message);

      lock (lockObj)
      {
        using (StreamWriter writer = new StreamWriter(logFilePath, true))
        {
          writer.WriteLine(formattedMessage);
        } 
      }
    }
    #endregion

    #region private methods
    private void CreateLogFileIfMissing()
    {
      string dirPath = Path.GetDirectoryName(logFilePath);

      if (string.IsNullOrWhiteSpace(dirPath))
      {
        LogError($"Couldn't get dir path from logfile path {logFilePath}");
        return;
      }

      if (!Directory.Exists(dirPath))
        Directory.CreateDirectory(dirPath);

      if (!File.Exists(logFilePath))
        using (File.Create(logFilePath))
        {
          //empty so we create empty file and close it
        };
    }
    #endregion
  }
}
