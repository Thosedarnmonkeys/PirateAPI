using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PirateAPI.Logging;

namespace PirateAPI.Parsers.Torrents
{
  public class HtmlTorrentTableRowParser
  {
    #region private fields
    private ILogger logger;
    #endregion

    #region constructor
    public HtmlTorrentTableRowParser(ILogger logger)
    {
      if (logger == null)
        throw new ArgumentException("HtmlTorrentTableRowParser Constructor was passed a null ILogger", nameof(logger));

      this.logger = logger;
    }
    #endregion

    #region public methods
    public Torrent ParseRow(string rowString)
    {
      throw new NotImplementedException();
    }
    #endregion
  }
}
