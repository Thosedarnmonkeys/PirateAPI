using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirateAPI.Parser
{
  public class PirateRequest
  {
    public string RequestUrl { get; set; }
    public int Limit { get; set; }
    public int Offset { get; set; }
    public bool ExtendedAttributes { get; set; }
  }
}
