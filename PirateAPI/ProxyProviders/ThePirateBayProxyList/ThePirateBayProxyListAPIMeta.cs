using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PirateAPI.ProxyProviders.ThePirateBayProxyList
{
  [DataContract]
  public class ThePirateBayProxyListAPIMeta
  {
    [DataMember(Name = "last_updated")]
    public long LastUpdated { get; set; }
  }
}
