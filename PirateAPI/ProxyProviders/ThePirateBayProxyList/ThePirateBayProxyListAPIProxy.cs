using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PirateAPI.ProxyProviders.ThePirateBayProxyList
{
  [DataContract]
  public class ThePirateBayProxyListAPIProxy
  {
    [DataMember(Name = "domain")]
    public string Domain { get; set; }
    [DataMember(Name = "speed")]
    public float Speed { get; set; }
    [DataMember(Name = "secure")]
    public bool Secure { get; set; }
    [DataMember(Name = "country")]
    public string Country { get; set; }
    [DataMember(Name = "probed")]
    public bool Probed { get; set; }
  }
}
