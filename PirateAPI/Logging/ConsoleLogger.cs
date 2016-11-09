﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirateAPI.Logging
{
  public class ConsoleLogger : AbstractLogger
  {
    #region private consts
    ConsoleColor messageColour1 = ConsoleColor.White;
    ConsoleColor messageColour2 = ConsoleColor.Gray;
    ConsoleColor errorColour1 = ConsoleColor.Red;
    ConsoleColor errorColour2 = ConsoleColor.DarkRed;
    #endregion

    #region private fields
    private bool messageColourState;
    private bool errorColourState;
    #endregion

    #region public methods
    public override void LogError(string message)
    {
      if (string.IsNullOrWhiteSpace(message))
      {
        LogError("ConsoleLogger.LogError was passed a null or empty string for message");
        return;
      }

      DateTime nowTime = DateTime.Now;
      string formattedMessage = FormatErrorMessage(nowTime, message);
      formattedMessage = AddLineBreaksIfRequired(formattedMessage, x => FormatErrorMessage(nowTime, x));
      SetConsoleErrorColour();
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

      DateTime nowTime = DateTime.Now;
      string formattedMessage = FormatExceptionMessage(nowTime, e, message);
      formattedMessage = AddLineBreaksIfRequired(formattedMessage, x => FormatExceptionMessage(nowTime, e, x));
      SetConsoleErrorColour();
      Console.WriteLine(formattedMessage);
    }

    public override void LogMessage(string message)
    {
      if (string.IsNullOrWhiteSpace(message))
      {
        LogError("ConsoleLogger was asked to LogMessage but message was null or whitespace");
        return;
      }

      DateTime nowTime = DateTime.Now;
      string formattedMessage = FormatMessage(nowTime, message);
      formattedMessage = AddLineBreaksIfRequired(formattedMessage, x => FormatMessage(nowTime, x));
      SetConsoleMessageColour();
      Console.WriteLine(formattedMessage);
    }
    #endregion

    #region private methods
    private string AddLineBreaksIfRequired(string message, Func<string, string> messageReformatMethod)
    {
      if (string.IsNullOrWhiteSpace(message) || messageReformatMethod == null)
        return null;

      if (message.Length < Console.BufferWidth)
        return message;

      List<string> messageLines = new List<string>();
      int maxMessageWidth = Console.BufferWidth - logMessagePreambleLength;
      string messageSubstr = message.Substring(logMessagePreambleLength);

      while (messageSubstr.Length > maxMessageWidth)
      {
        int lineBreakIndex = maxMessageWidth - 1;
        for (int i = 0; i < maxMessageWidth; i++)
        {
          if (messageSubstr[i] == ' ')
            lineBreakIndex = i;
        }

        string splitLine = messageSubstr.Substring(0, lineBreakIndex);
        messageLines.Add(messageReformatMethod.Invoke(splitLine));

        int whiteSpaceCompensation = messageSubstr[lineBreakIndex] == ' ' ? 1 : 0;
        messageSubstr = messageSubstr.Substring(lineBreakIndex + whiteSpaceCompensation);
      }

      messageLines.Add(messageReformatMethod.Invoke(messageSubstr));

      string formedLine = "";
      messageLines.ForEach(x => formedLine += x + "\n");
      formedLine = formedLine.Trim();
      return formedLine;
    }

    private void SetConsoleMessageColour()
    {
      messageColourState = !messageColourState;
      Console.ForegroundColor = messageColourState ? messageColour1 : messageColour2;
    }

    private void SetConsoleErrorColour()
    {
      errorColourState = !errorColourState;
      Console.ForegroundColor = errorColourState ? errorColour1 : errorColour2;
    }
    #endregion
  }
}
