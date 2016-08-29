using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirateAPI.Parser
{
  public enum VideoQuality { HD, SD, Both}

  public class PirateRequest
  {
    #region public properties
    public string RequestUrl
    {
      get
      {
        string pirateProxy = string.IsNullOrWhiteSpace(PirateProxyURL) ? "" : PirateProxyURL;
        string searchArg = string.IsNullOrWhiteSpace(ShowName) ? "" : ShowName.Replace("+", "%20");
        string categories;
        switch (Quality)
        {
          case VideoQuality.SD:
            categories = "205";
            break;

          case VideoQuality.HD:
            categories = "208";
            break;

          case VideoQuality.Both:
            categories = "205,208";
            break;

          default:
            categories = "205";
            break;
        }

        return $"{pirateProxy}/search/{searchArg}/0/99/{categories}";
      }
    }

    public string PirateProxyURL { get; set; }
    public string ShowName { get; set; }
    public int Limit { get; set; }
    public int Offset { get; set; }
    public bool ExtendedAttributes { get; set; }
    public VideoQuality Quality { get; set; }
    #endregion


    #region public methods
    public override bool Equals(object obj)
    {
      PirateRequest other = obj as PirateRequest;
      if (other == null)
        return false;

      if (other.RequestUrl != RequestUrl)
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

      return true;
    }
    #endregion
  }
}
