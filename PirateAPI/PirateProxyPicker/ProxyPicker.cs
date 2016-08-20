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
    public ProxyPicker(List<string> locationPrefs, ILogger logger)
    {

    }

    public Proxy BestProxy(List<Proxy> proxies)
    {
      throw new NotImplementedException();
    }
  }
}
