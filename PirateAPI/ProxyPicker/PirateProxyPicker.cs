using PirateAPI.Logging;
using PirateAPI.ProxyProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirateAPI.ProxyPicker
{
  public class PirateProxyPicker
  {
    #region private fields
    private ILogger logger;
    private List<string> locationPrefs;
    private List<string> tempBlackListedDomains = new List<string>();
    private List<string> permBlackListedDomains;
    #endregion

    #region constructor
    public PirateProxyPicker(List<string> locationPrefs, List<string> permanentBlackList, ILogger logger)
    {
      this.locationPrefs = locationPrefs;
      this.logger = logger;
      this.permBlackListedDomains = permanentBlackList ?? new List<string>();
    }
    #endregion

    #region public methods
    public Proxy BestProxy(List<Proxy> proxies)
    {
      IEnumerable<Proxy> goodProxies = from p in proxies
                                       where !permBlackListedDomains.Contains(p.Domain) &&!tempBlackListedDomains.Contains(p.Domain)
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

      logger.LogMessage($"Picker determined best proxy was domain {bestProxy.Domain} with speed {bestProxy.Speed} and location {bestProxy.Country.ToUpper()}");

      return bestProxy;
    }

    public void TempBlacklistDomain(string domain)
    {
      if (!tempBlackListedDomains.Contains(domain))
      {
        logger.LogMessage($"Domain {domain} was temporarily blacklisted");
        tempBlackListedDomains.Add(domain);
      }
    }

    public void ClearTempBlacklist()
    {
      tempBlackListedDomains.Clear();
    }
    #endregion
  }
}
