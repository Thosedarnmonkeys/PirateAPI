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
    public ulong Size { get; set; }
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

      if (other.Size != Size)
        return false;

      if (other.Seeds != Seeds)
        return false;

      if (other.Leeches != Leeches)
        return false;

      return true;
    }
  }
}
