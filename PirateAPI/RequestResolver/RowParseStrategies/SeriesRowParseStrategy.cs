using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using PirateAPI.Logging;
using PirateAPI.Parsers.Torrents;

namespace PirateAPI.RequestResolver.RowParseStrategies
{
  public class SeriesRowParseStrategy : IRowParseStrategy
  {
    #region private fields
    private ILogger logger;
    private Func<Torrent, bool> sanityCheck;
    private CancellationToken token;
    #endregion

    #region constructor
    public SeriesRowParseStrategy(Func<Torrent, bool> sanityCheck, ILogger logger, CancellationToken token)
    {
      if (logger == null)
        throw new ArgumentNullException(nameof(logger));

      if (sanityCheck == null)
        throw new ArgumentNullException(nameof(sanityCheck));

      this.logger = logger;
      this.sanityCheck = sanityCheck;
      this.token = token;
    }
    #endregion

    #region public methods
    public void ParseRows(HtmlNodeCollection rows, ITorrentRowParser parser, int limit, List<Torrent> results)
    {
      token.ThrowIfCancellationRequested();

      if (rows == null)
      {
        logger.LogError($"SeriesRowParseStartegy.ParseRows was passed a null values for {nameof(rows)}");
        return;
      }

      if (parser == null)
      {
        logger.LogError($"SeriesRowParseStartegy.ParseRows was passed a null values for {nameof(parser)}");
        return;
      }

      if (results == null)
      {
        logger.LogError($"SeriesRowParseStartegy.ParseRows was passed a null values for {nameof(results)}");
        return;
      }

      logger.LogMessage("Parsing torrents in series");
      foreach (HtmlNode row in rows)
      {
        token.ThrowIfCancellationRequested();

        if (results.Count >= limit)
          break;

        Torrent torrent = parser.ParseRow(row);
        if (torrent == null)
          return;

        token.ThrowIfCancellationRequested();

        if (PassesSanityCheck(torrent))
        {
          results.Add(torrent);
          logger.LogMessage($"Got {results.Count}/{limit} torrents");
        }
      }
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
