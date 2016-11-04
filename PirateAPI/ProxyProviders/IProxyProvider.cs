﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirateAPI.ProxyProviders
{
  interface IProxyProvider
  {
    List<Proxy> ListProxies();
    List<Proxy> ListMagnetInSearchProxies();
  }
}
