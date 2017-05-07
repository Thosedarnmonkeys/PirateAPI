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
    public List<Proxy> BestProxies(List<Proxy> proxies)
    {
      IEnumerable<Proxy> goodProxies = from p in proxies
                                       where !permBlackListedDomains.Contains(p.Domain) && !tempBlackListedDomains.Contains(p.Domain) && p.IsResponding == true
                                       select p;

      if (goodProxies.Count() == 0)
      {
        logger.LogError("All available proxies are blacklisted");
        return null;
      }

      List<Proxy> matchedProxies = new List<Proxy>();

      if (locationPrefs != null && locationPrefs.Count != 0)
      {
        foreach(string loc in locationPrefs)
        {
          IEnumerable<Proxy> fastProxies = from p in proxies
                                           where p.Country == loc && (p.Speed == ProxySpeed.VeryFast || p.Speed == ProxySpeed.Fast)
                                           select p;

          matchedProxies.AddRange(fastProxies);

          if (matchedProxies.Count >= 10)
            break;
        }
      }

      if (locationPrefs != null && locationPrefs.Count != 0 && matchedProxies.Count < 10)
      {
        foreach (string loc in locationPrefs)
        {
          IEnumerable<Proxy> fastProxies = from p in proxies
                                           where p.Country == loc && p.Speed == ProxySpeed.Slow
                                           select p;

          matchedProxies.AddRange(fastProxies);

          if (matchedProxies.Count >= 10)
            break;
        }
      }

      
      if (matchedProxies == null)
        matchedProxies = goodProxies.ToList();

      logger.LogMessage($"Picker chose {matchedProxies.Count} proxies with speed {matchedProxies.Max(p => p.Speed)} and above");

      return matchedProxies;
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
