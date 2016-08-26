using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PirateAPI.ProxyProviders;
using System.Net;
using System.Runtime.Serialization.Json;
using System.IO;
using PirateAPI.Logging;

namespace PirateAPI.ProxyProviders.ThePirateBayProxyList
{
  public class ThePirateBayProxyListProvider : IProxyProvider
  {
    #region private const fields
    private const string address = "https://thepiratebay-proxylist.org/api/v1/proxies";
    #endregion

    #region private fields
    private ILogger logger;
    #endregion

    #region constructor
    public ThePirateBayProxyListProvider(ILogger logger)
    {
      this.logger = logger;
    }
    #endregion

    #region public methods
    public List<Proxy> ListProxies()
    {
      WebClient webClient = new WebClient();
      string response;

      //Get api listing of proxies
      try
      {
        response = webClient.DownloadString(address);
      }
      catch (Exception e)
      {
        logger.LogException(e, "ThePirateBayProxyListProvider.GetProxysListingString threw exception trying to download string: ");
        return new List<Proxy>();
      }

      if (string.IsNullOrWhiteSpace(response))
      {
        logger.LogError($"Response string from {address} was null or empty");
        return new List<Proxy>();
      }

      //Deserialise response to API response obj
      DataContractJsonSerializer serialiser = new DataContractJsonSerializer(typeof(ThePirateBayProxyListAPIResponse));
      byte[] responseBytes = Encoding.Unicode.GetBytes(response);
      ThePirateBayProxyListAPIResponse apiResponse;

      using (MemoryStream stream = new MemoryStream(responseBytes))
        apiResponse = serialiser.ReadObject(stream) as ThePirateBayProxyListAPIResponse;

      if (apiResponse == null)
      {
        logger.LogError($"ThePirateBayProxyListProvider.ListProxies couldn't deserialise API Response of {response}");
        return new List<Proxy>();
      }

      if (apiResponse.Proxies == null)
      {
        logger.LogError($"ThePirateBayProxyListProvider.ListProxies deserialised API response, but proxies properties was null for response {response}");
        return new List<Proxy>();
      }

      //convert api proxies to internal proxy objs
      List<Proxy> proxies = new List<Proxy>();
      foreach (ThePirateBayProxyListAPIProxy proxy in apiResponse.Proxies)
      {
        ProxySpeed speed;
        if (proxy.Speed < 0.3)
          speed = ProxySpeed.VeryFast;

        else if (proxy.Speed < 1)
          speed = ProxySpeed.Fast;

        else
          speed = ProxySpeed.Slow;

        proxies.Add(new Proxy
        {
          Domain = proxy.Domain,
          Country = proxy.Country,
          Speed = speed
        });
      }

      return proxies;
    }
    #endregion
  }
}
