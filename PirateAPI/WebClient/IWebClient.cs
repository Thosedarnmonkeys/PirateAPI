using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PirateAPI.Logging;

namespace PirateAPI.WebClient
{
  public interface IWebClient
  {
    string DownloadString(string address);
  }
}
