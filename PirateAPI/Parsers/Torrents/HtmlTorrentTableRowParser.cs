using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using PirateAPI.Logging;

namespace PirateAPI.Parsers.Torrents
{
  public class HtmlTorrentTableRowParser : ITorrentRowParser
  {
    #region private fields
    private ILogger logger;
    private DateTime? fixedDateTime;
    #endregion

    #region protected fields
    protected CancellationToken token;
    #endregion

    #region private properties
    private DateTime currentDateTime
    {
      get
      {
        if (fixedDateTime.HasValue)
          return fixedDateTime.Value;

        return DateTime.Now;
      }
    }
    #endregion

    #region constructor
    public HtmlTorrentTableRowParser(ILogger logger, CancellationToken token) : this(logger, token, null) { }

    public HtmlTorrentTableRowParser(ILogger logger, CancellationToken token, DateTime? fixedDateTime)
    {
      if (logger == null)
        throw new ArgumentException("HtmlTorrentTableRowParser Constructor was passed a null ILogger", nameof(logger));

      this.logger = logger;
      this.fixedDateTime = fixedDateTime;
      this.token = token;
    }
    #endregion

    #region public methods
    public virtual Torrent ParseRow(HtmlNode row)
    {
      token.ThrowIfCancellationRequested();

      if (row == null)
      {
        logger.LogError("HtmlTorrentTableRowParser.ParseRow was passed a null row");
        return null;
      }

      if (!IsTorrentRow(row))
        return null;

      var torrent = new Torrent();

      string link = ParseLink(row);
      torrent.Link = HtmlEntity.DeEntitize(link);
      if (torrent.Link == null)
        return null;

      string title = GetChildNodeInnerText(row, "td/div/a");
      title = title.Trim();
      torrent.Title = HtmlEntity.DeEntitize(title);

      string uploaderName = GetChildNodeInnerText(row, "td/font/a");
      torrent.UploaderName = HtmlEntity.DeEntitize(uploaderName);
      if (torrent.UploaderName == null)
      {
        uploaderName = GetChildNodeInnerText(row, "td/font/i");
        torrent.UploaderName = HtmlEntity.DeEntitize(uploaderName);
      }

      string uploaderStatusString = GetChildNodeAttributeValue(row, "td/a/img", "title", 1);
      if (uploaderStatusString == null)
        uploaderStatusString = GetChildNodeAttributeValue(row, "td/a/img", "title", 2);
      torrent.UploaderStatus = ParseUploaderStatus(uploaderStatusString);

      string publishString = GetChildNodeInnerText(row, "td/font");
      publishString = CheckMatchAndGetFirst(publishString, "Uploaded (.*?),");
      torrent.PublishDate = ParsePublishDateTime(publishString);

      string sizeString = GetChildNodeInnerText(row, "td/font");
      sizeString = CheckMatchAndGetFirst(sizeString, "Size (.*),");
      if (sizeString != null)
        torrent.Size = ParseSizeULong(sizeString);

      string seedsString = GetChildNodeInnerText(row, "td", 2);
      torrent.Seeds = ParseInt(seedsString);

      string leechesString = GetChildNodeInnerText(row, "td", 3);
      torrent.Leeches = ParseInt(leechesString);

      return torrent;
    }
    #endregion

    #region protected methods
    protected virtual string ParseLink(HtmlNode row)
    {
      return  GetChildNodeAttributeValue(row, "td/a", "href");
    }

    protected string CheckMatchAndGetFirst(string input, string pattern)
    {
      Regex regex = new Regex(pattern);

      if (!regex.IsMatch(input))
      {
        logger.LogError($"Regex pattern {pattern} couldn't be matched to string {input}");
        return null;
      }

      Match match = regex.Match(input);

      return match.Groups[1].Value;
    }

    protected string GetChildNodeInnerText(HtmlNode parentNode, string childNodePath, int? childIndex = null)
    {
      HtmlNode childNode = GetChildNode(parentNode, childNodePath, childIndex);
      return childNode?.InnerText;
    }

    protected string GetChildNodeAttributeValue(HtmlNode parentNode, string childNodePath, string attributeName, int? childIndex = null)
    {
      HtmlNode childNode = GetChildNode(parentNode, childNodePath, childIndex);
      return childNode?.GetAttributeValue(attributeName, null);
    }
    #endregion

    #region private methods

    private HtmlNode GetChildNode(HtmlNode parentNode, string childNodePath, int? childIndex = null)
    {
      if (parentNode == null)
      {
        logger.LogError($"HtmlTorrentTableRowParser.GetChildNode was passed null value for {nameof(parentNode)}");
        return null;
      }

      if (childNodePath == null)
      {
        logger.LogError($"HtmlTorrentTableRowParser.GetChildNode was passed null value for {nameof(childNodePath)}");
        return null;
      }

      HtmlNodeCollection childNodes = parentNode.SelectNodes(childNodePath);
      if (childNodes == null || childNodes.Count == 0)
      {
        logger.LogError($"Got no child nodes for pattern {childNodePath} from parent node {parentNode.InnerHtml}");
        return null;
      }

      if (childIndex.HasValue && childNodes.Count <= childIndex.Value)
      {
        logger.LogError(
          $"Was asked for child with index of {childIndex}, using path {childNodePath} but there were only {childNodes.Count} matching nodes on parent node {parentNode.InnerHtml}");
        return null;
      }

      return childNodes[childIndex ?? 0];
    }

    private bool IsTorrentRow(HtmlNode row)
    {
      string classAttributeValue = row.GetAttributeValue("class", "noclass");
      if (classAttributeValue == "noclass" || classAttributeValue == "alt")
        return true;

      return false;
    }

    private TorrentUploaderStatus ParseUploaderStatus(string input)
    {
      if (string.IsNullOrWhiteSpace(input))
        return TorrentUploaderStatus.None;

      switch (input.ToLower())
      {
        case "vip":
          return TorrentUploaderStatus.Vip;

        case "trusted":
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
      input = RemoveHtmlElements(input);

      if (input.Contains("Today"))
        return ParseTodayDateString(input);

      //Y-Day or yesterday
      if (input.Contains("day"))
        return ParseYesterdayDayDateString(input);

      if (input.Contains("min"))
        return ParseMinutesAgoDateString(input);

      if (input.Contains(':'))
        return ParseThisYearDateString(input);

      return ParseGenericDateString(input);
    }

    private DateTime ParseTodayDateString(string dateString)
    {
      string timeString = CheckMatchAndGetFirst(dateString, @"^Today\s(.*)$");
      if (timeString == null)
        return DateTime.MinValue;

      string[] vals = timeString.Split(':');
      if (vals.Length != 2)
      {
        logger.LogError($"Splitting value {timeString} on char ':' resulted in {vals.Length} elements, not 2");
        return DateTime.MinValue;
      }

      int hour = ParseInt(vals[0]);
      int mins = ParseInt(vals[1]);

      return new DateTime(
        currentDateTime.Year,
        currentDateTime.Month,
        currentDateTime.Day,
        hour,
        mins,
        0);
    }

    private DateTime ParseYesterdayDayDateString(string dateString)
    {
      string timeString = CheckMatchAndGetFirst(dateString, @"^Y\sday\s(.*)$");
      if (timeString == null)
        timeString = CheckMatchAndGetFirst(dateString, @"^Yesterday\s(.*)$");

      if (timeString == null)
        return DateTime.MinValue;

      string[] vals = timeString.Split(':');
      if (vals.Length != 2)
      {
        logger.LogError($"Splitting value {timeString} on char ':' resulted in {vals.Length} elements, not 2");
        return DateTime.MinValue;
      }

      int hour = ParseInt(vals[0]);
      int mins = ParseInt(vals[1]);

      TimeSpan oneDaySpan = new TimeSpan(1, 0, 0, 0);

      return new DateTime(
        currentDateTime.Year,
        currentDateTime.Month,
        currentDateTime.Day,
        hour,
        mins,
        0) - oneDaySpan;
    }

    private DateTime ParseMinutesAgoDateString(string dateString)
    {
      string minsString = CheckMatchAndGetFirst(dateString, @"^(\d*)\smins\sago$");
      if (minsString == null)
        CheckMatchAndGetFirst(dateString, @"^(.*)\sminutes\sago$");

      if (minsString == null)
        return DateTime.MinValue;

      int minutesAgo = ParseInt(minsString);

      TimeSpan minsAgoSpan = new TimeSpan(0, minutesAgo, 0);
      return currentDateTime - minsAgoSpan;
    }

    private DateTime ParseThisYearDateString(string dateString)
    {
      string[] vals = dateString.Split();

      int day, month, year = currentDateTime.Year;

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

      return new DateTime(year, month, day, hour, minute, 0);
    }

    private DateTime ParseGenericDateString(string dateString)
    {
      string[] vals = dateString.Split();
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

      return new DateTime(year, month, day);
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

      string[] vals = input.Split();

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

      long size = (long)(qualifiedSize * multiplier);

      return size;
    }

    private int ParseInt(string input)
    {
      int val;
      if (!int.TryParse(input, out val))
      {
        logger.LogError($"Couldn't parse value {input} to int");
        return 0;
      }

      return val;
    }

    private string RemoveHtmlElements(string input)
    {
      if (!input.Contains("<") || !input.Contains(">"))
        return input;

      while (input.Contains("<"))
      {
        int openIndex = input.IndexOf("<");
        int closeIndex = input.IndexOf(">");
        input = input.Remove(openIndex, closeIndex - openIndex + 1);
      }

      return input;
    }


    #endregion
  }
}
