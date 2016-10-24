using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PirateAPI.Logging;

namespace PirateAPI.Parsers.Torrents
{
  public class HtmlTorrentTableRowParser : ITorrentRowParser
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
      if (string.IsNullOrWhiteSpace(rowString))
      {
        logger.LogError($"HtmlTorrentTableRowParser.ParseRow was passed a null or empty string for {nameof(rowString)}");
        return null;
      }

      rowString = rowString.Replace(Environment.NewLine, "")
                           .Replace("\t", "");

      Torrent torrent = new Torrent();

      string namePattern = @"""detName"">\s*?<a.*?>\s*?(\w.*?)\s*?<";
      torrent.Title = CheckMatchAndGetFirst(rowString, namePattern, "Title");

      string linkPattern = @"href=""(magnet:\?.*?)""";
      torrent.Link = CheckMatchAndGetFirst(rowString, linkPattern, "Link");

      string uploaderPattern = @"href=""\/user\/(.*?)\/?""";
      torrent.UploaderName = CheckMatchAndGetFirst(rowString, uploaderPattern, "UploaderName");

      if (torrent.UploaderName == null)
      {
        string anonymousPattern = @"ULed by <i>(.*?)<\/i>";
        torrent.UploaderName = CheckMatchAndGetFirst(rowString, anonymousPattern, "UploaderNameAnonymous");
      }

      string statusPattern = $@"href=""\/user\/{torrent.UploaderName}"">.*?<img.*?title=""(.*?)""";
      string statusString = CheckMatchAndGetFirst(rowString, statusPattern, "UploaderStatus");
      torrent.UploaderStatus = ParseUploaderStatus(statusString);

      string publishPattern = @"""detDesc"">.*?Uploaded (.*?),";
      string publishString = CheckMatchAndGetFirst(rowString, publishPattern, "PublishDate");
      torrent.PublishDate = ParsePublishDateTime(publishString);

      string sizePattern = @"""detDesc"">.*?Size (.*?),";
      string sizeString = CheckMatchAndGetFirst(rowString, sizePattern, "Size");
      torrent.Size = ParseSizeULong(sizeString);

      string seedsAndLeechesPattern = @"<td align=""right"">(.*?)<";
      Tuple<string, string> seedsAndLeechesTuple = CheckMatchAndGetFirstTuple(rowString, seedsAndLeechesPattern, "Seeds", "Leeches");
      if (seedsAndLeechesTuple != null)
      {
        torrent.Seeds = ParseInt(seedsAndLeechesTuple.Item1, "Seeds");
        torrent.Leeches = ParseInt(seedsAndLeechesTuple.Item2, "Leeches");
      }

      return torrent;
    }
    #endregion

    #region private methods
    private string CheckMatchAndGetFirst(string input, string pattern, string paramName)
    {
      Regex regex = new Regex(pattern);

      if (!regex.IsMatch(input))
      {
        logger.LogError($"Regex pattern {pattern} couldn't be matched to string {input} for Torrent prop {paramName}");
        return null;
      }

      Match match = regex.Match(input);

      return match.Groups[1].Value;
    }

    private Tuple<string, string> CheckMatchAndGetFirstTuple(string input, string pattern, string firstParam, string secondParam)
    {
      Regex regex = new Regex(pattern);

      if (!regex.IsMatch(input))
      {
        logger.LogError($"Regex pattern {pattern} couldn't be matched to string {input} for Torrent props {firstParam} and {secondParam}");
        return null;
      }

      MatchCollection matches = regex.Matches(input);

      if (matches.Count != 2)
      {
        logger.LogError($"Regex pattern {pattern} had {matches.Count} matches, instead of 2, for input {input}");
        return new Tuple<string, string>("0", "0");
      }

      string seeds = matches[0].Groups[1].Value;
      string leeches = matches[1].Groups[1].Value;
      return new Tuple<string, string>(seeds, leeches);
    }

    private TorrentUploaderStatus ParseUploaderStatus(string input)
    {
      if (string.IsNullOrWhiteSpace(input))
        return TorrentUploaderStatus.None;

      switch (input)
      {
        case "VIP":
          return TorrentUploaderStatus.Vip;

        case "Trusted":
          return TorrentUploaderStatus.Trusted;

        default:
          logger.LogError($"Couldn't parse input of {input} to TorrentUploaderStatus");
          return TorrentUploaderStatus.None;
      }
    }

    private DateTime ParsePublishDateTime(string input)
    {
      if (string.IsNullOrWhiteSpace(input))
      {
        logger.LogError($"HtmlTorrentTableRowParser.ParsePublishDateTime was passed a null or empty string for {nameof(input)}");
        return DateTime.MinValue;
      }

      input = input.Replace("&nbsp;", " ");
      input = input.Replace("Â ", " ");
      input = input.Replace("-", " ");

      DateTime dateTime;
      string[] vals = input.Split(' ');

      if (input.Contains(':'))
      {
        int day, month, year = DateTime.Now.Year;

        if (!int.TryParse(vals[0], out month))
        {
          logger.LogError($"Couldn't parse value {vals[0]} to int for month");
          return DateTime.MinValue;
        }

        if (!int.TryParse(vals[1], out day))
        {
          logger.LogError($"Couldn't parse value {vals[1]} to int for day");
          return DateTime.MinValue;
        }

        string[] time = vals[2].Split(':');

        int hour, minute;

        if (!int.TryParse(time[0], out hour))
        {
          logger.LogError($"Couldn't parse value {time[0]} to int for hour");
          return DateTime.MinValue;
        }

        if (!int.TryParse(time[1], out minute))
        {
          logger.LogError($"Couldn't parse value {time[1]} to int for minute");
          return DateTime.MinValue;
        }

        dateTime = new DateTime(year, month, day, hour, minute, 0);
      }
      else
      {
        int day, month, year;

        if (!int.TryParse(vals[0], out month))
        {
          logger.LogError($"Couldn't parse value {vals[0]} to int for month");
          return DateTime.MinValue;
        }

        if (!int.TryParse(vals[1], out day))
        {
          logger.LogError($"Couldn't parse value {vals[1]} to int for day");
          return DateTime.MinValue;
        }

        if (!int.TryParse(vals[2], out year))
        {
          logger.LogError($"Couldn't parse value {vals[2]} to int for year");
          return DateTime.MinValue;
        }

        dateTime = new DateTime(year, month, day);
      }

      return dateTime;
    }

    private long ParseSizeULong(string input)
    {
      if (string.IsNullOrWhiteSpace(input))
      {
        logger.LogError($"HtmlTorrentTableRowParser.ParseSizeLong was passed a null or empty string for {nameof(input)}");
        return 0;
      }

      input = input.Replace("&nbsp;", " ");
      input = input.Replace("Â ", " ");

      string[] vals = input.Split(' ');

      double qualifiedSize;

      if (!double.TryParse(vals[0], out qualifiedSize))
      {
        logger.LogError($"Unable to parse {vals[0]} to double for qualifiedSize");
        return 0;
      }

      long multiplier;
      switch (vals[1])
      {
        case "B":
          multiplier = 1;
          break;

        case "KiB":
          multiplier = 1024;
          break;

        case "MiB":
          multiplier = 1048576;
          break;

        case "GiB":
          multiplier = 1073741824;
          break;

        case "TiB":
          multiplier = 1099511627776;
          break;

        //Stopping here, because you should too
        
        default:
          multiplier = 1;
          logger.LogError($"Couldn't match string {vals[1]} to size quantifier");
          break;
      }

      long size = (long)(qualifiedSize*multiplier);

      return size;
    }

    private int ParseInt(string input, string paramName)
    {
      int val;
      if (!int.TryParse(input, out val))
      {
        logger.LogError($"Couldn't parse value {input} to int for {paramName}");
        return 0;
      }

      return val;
    }
    #endregion
  }
}
