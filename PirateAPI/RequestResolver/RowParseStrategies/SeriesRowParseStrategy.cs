using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PirateAPI.Logging;
using PirateAPI.Parsers.Torrents;

namespace PirateAPI.RequestResolver.RowParseStrategies
{
  public class SeriesRowParseStrategy : IRowParseStrategy
  {
    #region private fields
    private ILogger logger;
    private Func<Torrent, bool> sanityCheck;
    #endregion

    #region constructor
    public SeriesRowParseStrategy(Func<Torrent, bool> sanityCheck, ILogger logger)
    {
      if (logger == null)
        throw new ArgumentNullException(nameof(logger));

      if (sanityCheck == null)
        throw new ArgumentNullException(nameof(sanityCheck));

      this.logger = logger;
      this.sanityCheck = sanityCheck;
    }
    #endregion

    #region public methods
    public List<Torrent> ParseRows(List<string> rows, ITorrentRowParser parser, int limit)
    {
      if (rows == null)
      {
        logger.LogError($"SeriesRowParseStartegy.ParseRows was passed a null values for {nameof(rows)}");
        return null;
      }

      if (parser == null)
      {
        logger.LogError($"SeriesRowParseStartegy.ParseRows was passed a null values for {nameof(parser)}");
        return null;
      }

      var results = new List<Torrent>();

      logger.LogMessage("Parsing torrents in series");
      foreach (string row in rows)
      {
        if (results.Count >= limit)
          break;

        Torrent torrent = parser.ParseRow(row);
        if (PassesSanityCheck(torrent))
        {
          results.Add(torrent);
          logger.LogMessage($"Got {results.Count}/{limit} torrents");
        }
      }

      return results;
    }

    #endregion

    #region private methods
    private bool PassesSanityCheck(Torrent torrent)
    {
      return sanityCheck.Invoke(torrent);
    }
    #endregion
  }
}
