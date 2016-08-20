using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PirateAPI.Logging;

namespace PirateAPITests.Tests.StubClasses
{
  public class StubLogger : ILogger
  {
    public void LogError(string message)
    {
    }

    public void LogException(Exception e, string message = null)
    {
    }

    public void LogMessage(string message)
    {
    }
  }
}
