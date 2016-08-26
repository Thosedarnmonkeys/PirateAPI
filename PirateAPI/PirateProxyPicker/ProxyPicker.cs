using PirateAPI.Logging;
using PirateAPI.ProxyProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirateAPI.PirateProxyPicker
{
  public class ProxyPicker
  {
    #region private fields
    private ILogger logger;
    private List<string> locationPrefs;
    private List<string> blackListedDomains = new List<string>();
    #endregion

    #region constructor
    public ProxyPicker(List<string> locationPrefs, ILogger logger)
    {
      this.locationPrefs = locationPrefs;
      this.logger = logger;
    }
    #endregion

    #region public methods
    public Proxy BestProxy(List<Proxy> proxies)
    {
      IEnumerable<Proxy> goodProxies = from p in proxies
                                       where !blackListedDomains.Contains(p.Domain)
                                       select p;

      if (goodProxies.Count() == 0)
      {
        logger.LogError("All available proxies are blacklisted");
        return null;
      }

      IEnumerable<Proxy> matchedProxies = null;

      if (locationPrefs != null && locationPrefs.Count != 0)
      {
        foreach(string loc in locationPrefs)
        {
          if (goodProxies.Any(p => p.Country == loc))
          {
            matchedProxies = from p in goodProxies
                                    where p.Country == loc
                                    select p;
            break;
          }
        }
      }

      if (matchedProxies == null)
        matchedProxies = goodProxies;

      matchedProxies = matchedProxies.OrderBy(p => p.Speed);

      Proxy bestProxy = matchedProxies.FirstOrDefault();

      logger.LogMessage($"Picker determined best proxy was domain {bestProxy.Domain} with speed {bestProxy.Speed} and location {bestProxy.Country}");

      return bestProxy;
    }

    public void BlacklistDomain(string domain)
    {
      if (!blackListedDomains.Contains(domain))
      {
        logger.LogMessage($"Domain {domain} was blacklisted");
        blackListedDomains.Add(domain);
      }
    }
    #endregion
  }
}
