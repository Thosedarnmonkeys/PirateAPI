using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirateAPI.Parsers.Torrents
{
  public enum TorrentUploaderStatus { None, Trusted, Vip}

  public class Torrent
  {
    public string Title { get; set; }
    public string UploaderName { get; set; }
    public DateTime PublishDate { get; set; }
    public string Link { get; set; }
    public TorrentUploaderStatus UploaderStatus { get; set; }
    public long Size { get; set; }
    public int Seeds { get; set; }
    public int Leeches { get; set; }

    public override bool Equals(object obj)
    {
      Torrent other = obj as Torrent;

      if (other == null)
        return false;

      if (other.Title != Title)
        return false;

      if (other.UploaderName!= UploaderName)
        return false;

      if (other.PublishDate != PublishDate)
        return false;

      if (other.Link != Link)
        return false;

      if (other.UploaderStatus != UploaderStatus)
        return false;

      if (other.Seeds != Seeds)
        return false;

      if (other.Leeches != Leeches)
        return false;

      //I've tried 3 different methods for accurate size calculation, and they all have different answers
      //Here if you're within 3 sig fig, you're the same

      double mySize = RoundToSignificantDigits(Size, 3);
      double theirSize = RoundToSignificantDigits(other.Size, 3);

      if (!mySize.Equals(theirSize))
        return false;

      return true;
    }

    private double RoundToSignificantDigits(double d, int digits)
    {
      double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(d))) + 1);
      double val = scale * Math.Round(d / scale, digits);
      return Math.Floor(val);
    }
  }
}
