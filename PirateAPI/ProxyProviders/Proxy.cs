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
    #region public properties
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
    #endregion

    #region public methods
    public override bool Equals(object obj)
    {
      Proxy other = obj as Proxy;
      if (other == null)
        return false;

      if (other.Domain != Domain)
        return false;

      if (other.Speed != Speed)
        return false;

      if (other.Country != Country)
        return false;

      return true;
    } 
    #endregion
  }
}
