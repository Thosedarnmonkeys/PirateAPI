using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using PirateAPI.Logging;
using PirateAPI.Parsers.Torrents;

namespace PirateAPI.RequestResolver.RowParseStrategies
{
  public interface IRowParseStrategy
  {
    List<Torrent> ParseRows(HtmlNodeCollection rows, ITorrentRowParser parser, int limit);
  }
}
