using PirateAPI.Parsers.Torrents;
using PirateAPI.Parsers.Torznab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PirateAPI.HtmlExtractors;
using PirateAPI.Logging;
using PirateAPI.SanityCheckers;
using PirateAPI.WebClient;

namespace PirateAPI.RequestResolver
{
  public class PirateRequestResolver
  {
    #region private fields
    private ILogger logger;
    private IWebClient webClient;
    private DateTime currentDate = DateTime.Now;
    private HtmlRowExtractor rowExtractor;
    private HtmlTorrentTableRowParser rowParser;
    private TorrentNameSanityChecker checker;
    #endregion

    #region constructor
    public PirateRequestResolver(ILogger logger, IWebClient webClient, DateTime? currentDate = null)
    {
      this.logger = logger;
      this.webClient = webClient;
      if (currentDate.HasValue)
        this.currentDate = currentDate.Value;

      rowExtractor = new HtmlRowExtractor(logger);
      rowParser = new HtmlTorrentTableRowParser(logger);
      checker = new TorrentNameSanityChecker(logger);
    }
    #endregion

    #region public methods
    public List<Torrent> Resolve(PirateRequest request)
    {
      string errorMessage;
      if (!IsRequestValid(request, out errorMessage))
      {
        logger.LogError(errorMessage);
        throw new ArgumentException($"{nameof(request)} was not a valid PirateRequest");
      }

      if (!string.IsNullOrWhiteSpace(request.ShowName))
        request.ShowName = request.ShowName.Replace("+", " ");

      //Set up initial values
      int requestPage = (int)Math.Floor((double)request.Offset / 30);
      int firstPageOffset = request.Offset%30;
      bool isFirstPage = true;
      int limit = request.Limit ?? 100;
      List<Torrent> results = new List<Torrent>();

      //run till we reach required number
      while (results.Count < limit)
      {
        //Get page
        string requestUrl;
        requestUrl = string.IsNullOrWhiteSpace(request.ShowName) ? ConstructQueryForBrowsePage(request, requestPage) : ConstructQueryForTvSearchPage(request, requestPage);

        string piratePage = webClient.DownloadString(requestUrl);

        if (piratePage == null)
          return null;

        List<string> rows = rowExtractor.ExtractRows(piratePage);

        //if first page, remove up to offset
        if (isFirstPage)
        {
          MatchRowsToOffset(firstPageOffset, rows);
          isFirstPage = false;
        }

        if (rows.Count == 0)
          //we have run out of results
          break;

        //first row is always headers
        rows.RemoveAt(0);

        foreach (string row in rows)
        {
          Torrent torrent = rowParser.ParseRow(row);
          if (IsValidTorrentForRequest(request, torrent))
            results.Add(torrent);
        }

        requestPage++;
      }

      if (results.Count > limit)
        results = results.GetRange(0, limit);

      return results;
    }
    #endregion

    #region private methods
    private bool IsRequestValid(PirateRequest request, out string errorMessage)
    {
      if (request == null)
      {
        errorMessage = "PirateRequestResolver was passed a null request";
        return false;
      }

      if (request.Episode.HasValue && !request.Season.HasValue)
      {
        errorMessage = "PirateRequestResolver was passed request with episode number but no season number";
        return false;
      }

      if (string.IsNullOrWhiteSpace(request.PirateProxyURL))
      {
        errorMessage = "PirateRequestResolver was passed request with null or empty PirateProxyURL";
        return false;
      }

      errorMessage = "";
      return true;
    }

    private string ConstructQueryForTvSearchPage(PirateRequest request, int page)
    {
      string categories;
      switch (request.Quality)
      {
        case VideoQuality.SD:
          categories = "205";
          break;

        case VideoQuality.HD:
          categories = "208";
          break;

        case VideoQuality.Both:
          categories = "205,208";
          break;

        default:
          categories = "205";
          break;
      }

      string sanitisedShowName = request.ShowName.Replace(" ", "%20")
                                                 .Replace("&", "and");

      string searchArg;
      if (request.Episode.HasValue && request.Season.HasValue)
      {
        string zeroedSeasonNum = request.Season.Value < 10 ? "0" + request.Season.Value : request.Season.Value.ToString();
        string zeroedEpisodeNum = request.Episode.Value < 10 ? "0" + request.Episode.Value : request.Episode.Value.ToString();

        searchArg = $"{sanitisedShowName}%20S{zeroedSeasonNum}E{zeroedEpisodeNum}";
      }

      else if (request.Season.HasValue)
        searchArg = $"{sanitisedShowName}%20Season%20{request.Season.Value}";

      else
        searchArg = sanitisedShowName;

      return $"{request.PirateProxyURL}/search/{searchArg}/{page}/99/{categories}";
    }

    private string ConstructQueryForBrowsePage(PirateRequest request, int page)
    {
      return $"{request.PirateProxyURL}/browse/208/{page}/3";
    }

    private bool IsValidTorrentForRequest(PirateRequest request, Torrent torrent)
    {
      double age = Math.Floor((currentDate - torrent.PublishDate).TotalDays);

      if (age > request.MaxAge)
        return false;

      if (request.Season.HasValue && request.Episode.HasValue)
        return checker.Check(request.ShowName, request.Season.Value, request.Episode.Value, torrent.Title);

      if (request.Season.HasValue)
        return checker.Check(request.ShowName, request.Season.Value, torrent.Title);

      if (!string.IsNullOrWhiteSpace(request.ShowName))
        return checker.Check(request.ShowName, torrent.Title);

      return true;
    }

    private void MatchRowsToOffset(int offset, List<string> rows)
    {
      if (rows.Count == 0)
        return;

      if (offset >= rows.Count - 1)
      {
        rows.Clear();
        return;
      }

      rows.RemoveRange(1, offset);
    }
    #endregion
  }
}
