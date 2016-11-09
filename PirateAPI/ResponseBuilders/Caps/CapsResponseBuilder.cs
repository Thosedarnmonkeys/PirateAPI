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
    public string BuildResponse(int apiLimit)
    {
      if (apiLimit < 0)
        apiLimit = 0;

      string capsTemplate = Resources.CapsResponseTemplate;
      capsTemplate = capsTemplate.Replace("{apilimit}", apiLimit.ToString());
      capsTemplate = capsTemplate.Replace("{apidefault}", apiLimit.ToString());
      return capsTemplate;
    }
  }
}
