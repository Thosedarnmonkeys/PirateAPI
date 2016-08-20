using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirateAPI.ProxyProviders
{
  public enum ProxySpeed { VeryFast, Fast, Slow, NotResponding };

  public class Proxy
  {
    public string Domain { get; set; }
    public ProxySpeed Speed { get; set; }
    public bool IsResponding
    {
      get { return Speed == ProxySpeed.NotResponding; }
    }

    private string country;
    public string Country
    {
      get { return country.ToLower(); }
      set { country = value; }
    }
  }
}
