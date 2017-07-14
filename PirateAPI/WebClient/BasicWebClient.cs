using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using PirateAPI.Logging;

namespace PirateAPI.WebClient
{
  public class BasicWebClient : System.Net.WebClient, IWebClient
  {
    private ILogger logger;
    private const int timeoutMillis = 3000;

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
        logger.LogMessage($"Downloading from {address}");
        Uri requestUri = new Uri(address);
        WebRequest request = GetWebRequest(requestUri);
        WebResponse response = request.GetResponse();
        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
        {
          return reader.ReadToEnd();
        }
      }
      catch (Exception e)
      {
        logger.LogException(e, $"BasicWebClient.DownloadString threw exception while trying to download from address {address} "); 
        return null;
      }
    }

    protected override WebRequest GetWebRequest(Uri address)
    {
      WebRequest request = base.GetWebRequest(address);
      if (request != null)
      request.Timeout = timeoutMillis;
      return request;
    }
  }
}
