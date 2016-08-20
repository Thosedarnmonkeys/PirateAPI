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
      throw new NotImplementedException();
    }

    public void BlacklistDomain(string domain)
    {

    }
    #endregion
  }
}
