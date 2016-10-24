using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirateAPI.Parsers.Torrents
{
  public interface ITorrentRowParser
  {
    Torrent ParseRow(string rowString);
  }
}
