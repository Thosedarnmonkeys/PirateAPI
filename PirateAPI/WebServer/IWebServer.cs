using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirateAPI.WebServer
{
  interface IWebServer
  {
    bool StartServing();
    void StopServing();
  }
}
