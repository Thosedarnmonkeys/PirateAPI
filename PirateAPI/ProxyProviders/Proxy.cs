using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirateAPI.ProxyProviders
{
  //using explicit numbers to show that lower number is faster
  public enum ProxySpeed { VeryFast = 0, Fast = 1, Slow = 2, NotResponding = 3 };

  public class Proxy
  {
    public string Domain { get; set; }
    public ProxySpeed Speed { get; set; }
    public bool IsResponding
    {
      get { return Speed != ProxySpeed.NotResponding; }
    }

    private string country;
    public string Country
    {
      get { return country.ToLower(); }
      set { country = value; }
    }
  }
}
