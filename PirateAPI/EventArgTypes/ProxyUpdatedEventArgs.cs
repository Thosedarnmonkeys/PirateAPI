using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PirateAPI.ProxyProviders;


namespace PirateAPI.EventArgTypes
{
  public class ProxyUpdatedEventArgs : EventArgs
  {
    public Proxy Proxy { get; set; }
  }
}
