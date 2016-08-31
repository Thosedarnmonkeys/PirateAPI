using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
      if (string.IsNullOrWhiteSpace(rowString))
      {
        logger.LogError($"HtmlTorrentTableRowParser.ParseRow was passed a null or empty string for {nameof(rowString)}");
        return null;
      }

      Torrent torrent = new Torrent();

      string namePattern = @"""detName"">\s*?<a.*?>(.*?)<";
      torrent.Title = CheckMatchAndGetFirst(rowString, namePattern, "Title");

      string linkPattern = @"href=""(magnet:\?.*?)""";
      torrent.Link = CheckMatchAndGetFirst(rowString, linkPattern, "Link");

      string uploaderPattern = @"<a href=""\/ user\/ (.*?)""";
      torrent.UploaderName = CheckMatchAndGetFirst(rowString, uploaderPattern, "UploaderName");

      string statusPattern = $@"<a href=""\/ user\/{torrent.UploaderName}"">.*?<img.*?title=""(.*?)""";
      string statusString = CheckMatchAndGetFirst(rowString, statusPattern, "UploaderStatus");
      torrent.UploaderStatus = ParseUploaderStatus(statusString);

      string publishPattern = @"""detDesc"">.*?Uploaded (.*?),";
      string publishString = CheckMatchAndGetFirst(rowString, publishPattern, "PublishDate");
      torrent.PublishDate = ParsePublishDateTime(publishString);



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

      return match.Value;
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
    #endregion
  }
}
