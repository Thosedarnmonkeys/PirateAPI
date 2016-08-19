using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirateAPI.Logging
{
  public interface ILogger
  {
    void LogMessage(string message);
    void LogException(Exception e, string message = null);
    void LogError(string message);
  }
}
