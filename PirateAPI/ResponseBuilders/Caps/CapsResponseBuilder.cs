using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PirateAPI.Properties;

namespace PirateAPI.ResponseBuilders.Caps
{
  public class CapsResponseBuilder
  {
    public string BuildResponse()
    {
      return Resources.CapsResponseTemplate;
    }
  }
}
