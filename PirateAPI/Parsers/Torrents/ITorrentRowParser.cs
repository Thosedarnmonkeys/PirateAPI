using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace PirateAPI.Parsers.Torrents
{
  public interface ITorrentRowParser
  {
    Torrent ParseRow(HtmlNode row);
  }
}
