using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PirateAPI.Logging;

namespace PirateAPI.WebClient
{
  public class BasicWebClient : IWebClient
  {
    private ILogger logger;

    public BasicWebClient(ILogger logger)
    {
      if (logger == null)
        throw new ArgumentException("BasicWebClient Constructor was passed a null ILogger", nameof(logger));

      this.logger = logger;
    }

    public string DownloadString(string address)
    {
      if (string.IsNullOrWhiteSpace(address))
      {
        logger.LogError("BasicWebClient.DownloadString was passed a null or empty string for address");
        return null;
      }

      try
      {
        System.Net.WebClient client = new System.Net.WebClient();
        return client.DownloadString(address);
      }
      catch (Exception e)
      {
        logger.LogException(e, $"BasicWebClient.DownloadString threw exception while trying to download from address {address} : "); 
        return null;
      }
    }
  }
}
