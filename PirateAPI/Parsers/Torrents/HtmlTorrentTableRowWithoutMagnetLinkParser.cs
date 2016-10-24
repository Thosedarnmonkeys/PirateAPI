using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PirateAPI.Logging;
using PirateAPI.WebClient;

namespace PirateAPI.Parsers.Torrents
{
  public class HtmlTorrentTableRowWithoutMagnetLinkParser : ITorrentRowParser
  {
    #region private fields
    private ILogger logger;
    private IWebClient webClient;
    #endregion

    #region constructor
    public HtmlTorrentTableRowWithoutMagnetLinkParser(IWebClient webclient, ILogger logger)
    {
      this.webClient = webclient;
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
