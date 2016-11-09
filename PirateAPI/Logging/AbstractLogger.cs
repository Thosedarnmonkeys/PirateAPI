﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirateAPI.Logging
{
  public abstract class AbstractLogger : ILogger
  {
    #region protected consts
    protected const string logMessageFormat = "{0}  |  {1}  |  {2}";
    protected const string messageText = "Info ";
    protected const string errorText = "Error";
    #endregion

    #region public abstract methods
    public abstract void LogMessage(string message);
    public abstract void LogException(Exception e, string message = null);
    public abstract void LogError(string message);
    #endregion

    #region private methods
    protected string FormatErrorMessage(DateTime logDateTime, string message)
    {
      if (string.IsNullOrWhiteSpace(message))
        message = "Message passed was null or whitespace";

      return string.Format(logMessageFormat, logDateTime, errorText, message);
    }

    protected string FormatMessage(DateTime logDateTime, string message)
    {
      if (string.IsNullOrWhiteSpace(message))
        message = "Message passed was null or whitespace";

      return string.Format(logMessageFormat, logDateTime, messageText, message);
    }

    protected string FormatExceptionMessage(DateTime logDateTime, Exception e, string message = null)
    {
      return string.Format(logMessageFormat, logDateTime, errorText, (message ?? "") + (message == null || e == null ? "" : ": ") + e?.Message);
    }
    #endregion
  }
}
