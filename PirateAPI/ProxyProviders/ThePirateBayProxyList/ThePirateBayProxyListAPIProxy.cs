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
    [DataMember]
    public string Domain { get; set; }
    [DataMember]
    public float Speed { get; set; }
    [DataMember]
    public bool Secure { get; set; }
    [DataMember]
    public string Country { get; set; }
    [DataMember]
    public bool Probed { get; set; }
  }
}
