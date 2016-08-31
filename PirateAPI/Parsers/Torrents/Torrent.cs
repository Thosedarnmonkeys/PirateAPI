using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirateAPI.Parsers.Torrents
{
  public class Torrent
  {
    public string Title { get; set; }
    public int Id { get; set; }
    public DateTime PublishDate { get; set; }
    public string Link { get; set; }
  }
}
