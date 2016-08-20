using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PirateAPI.ProxyProviders.ThePirateBayProxyList
{
  [DataContract]
  public class ThePirateBayProxyListAPIResponse
  {
    [DataMember(Name = "proxies")]
    public ThePirateBayProxyListAPIProxy[] Proxies { get; set; }
    [DataMember(Name = "meta")]
    public ThePirateBayProxyListAPIMeta Meta { get; set; }
  }
}
