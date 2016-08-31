using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirateAPI.Parsers.Torznab
{
  public enum VideoQuality { HD, SD, Both}

  public class PirateRequest
  {
    #region public properties
    public string PirateProxyURL { get; set; }
    public string ShowName { get; set; }
    public int? Limit { get; set; }
    public int Offset { get; set; }
    public bool ExtendedAttributes { get; set; }
    public VideoQuality Quality { get; set; }
    public int? Season { get; set; }
    public int? Episode { get; set; }
    public int? MaxAge { get; set; }
    #endregion


    #region public methods
    public override bool Equals(object obj)
    {
      PirateRequest other = obj as PirateRequest;
      if (other == null)
        return false;

      if (other.PirateProxyURL != PirateProxyURL)
        return false;

      if (other.ShowName != ShowName)
        return false;

      if (other.Limit != Limit)
        return false;

      if (other.Offset != Offset)
        return false;

      if (other.ExtendedAttributes != ExtendedAttributes)
        return false;

      if (other.Quality != Quality)
        return false;

      if (other.Season != Season)
        return false;

      if (other.Episode != Episode)
        return false;

      if (other.MaxAge != MaxAge)
        return false;

      return true;
    }
    #endregion
  }
}
