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
using PirateAPI.ProxyInfoGatherers;
using PirateAPI.WebClient;

namespace PirateAPI.ProxyProviders.ThePirateBayProxyList
{
  public class ThePirateBayProxyListProvider : IProxyProvider
  {
    #region private const fields
    private const string address = "https://thepiratebay-proxylist.org/api/v1/proxies";
    #endregion

    #region private fields
    private ILogger logger;
    private IWebClient webClient;
    #endregion

    #region constructor
    public ThePirateBayProxyListProvider(ILogger logger, IWebClient webClient)
    {
      this.logger = logger;
      this.webClient = webClient;
    }
    #endregion

    #region public methods
    public List<Proxy> ListProxies()
    {
      return ListProxyImpl(false);
    }

    public List<Proxy> ListMagnetInSearchProxies()
    {
      return ListProxyImpl(true);
    }
    #endregion

    #region private methods

    private List<Proxy> ListProxyImpl(bool magnetsInSearchOnly)
    {
      string response;

      //Get api listing of proxies
      try
      {
        response = webClient.DownloadString(address);
      }
      catch (Exception e)
      {
        logger.LogException(e, "ThePirateBayProxyListProvider.ListProxyImpl threw exception trying to download string");
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

        string protocol = proxy.Secure ? "https" : "http";

        Proxy proxyObj = new Proxy()
        {
          Domain = $"{protocol}://{proxy.Domain}",
          Country = proxy.Country,
          Speed = speed
        };

        if (!magnetsInSearchOnly)
          proxies.Add(proxyObj);

        else if (HasMagnetsInSearch(proxyObj.Domain))
          proxies.Add(proxyObj);
      }

      return proxies;
    }

    private bool HasMagnetsInSearch(string domain)
    {
      if (string.IsNullOrWhiteSpace(domain))
      {
        logger.LogError("ThePirateBayProxyListProvider.HasMagnetsInSearch was passed a null or empty string");
        return false;
      }

      MagnetsInSearchTester tester = new MagnetsInSearchTester(webClient, logger);
      bool? result = tester.DomainHasMagnetsInSearch(domain);
      return result.HasValue && result.Value;
    }
    #endregion
  }
}
