using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PirateAPI.Logging;

namespace PirateAPI.HtmlExtractors
{
  public class HtmlRowExtractor
  {
    #region private fields
    private ILogger logger;
    #endregion

    #region constructor
    public HtmlRowExtractor(ILogger logger)
    {
      if (logger == null)
        throw new ArgumentException("HtmlRowExtractor Constructor was passed a null ILogger", nameof(logger));

      this.logger = logger;
    }
    #endregion

    #region public methods
    public List<string> ExtractRows(string html)
    {
      if (string.IsNullOrWhiteSpace(html))
      {
        logger.LogError($"HtmlRowExtractor.ExtractRows was passed a null or empty string for {nameof(html)}");
        return new List<string>();
      }

      html = html.Replace("\n", "")
                 .Replace("\t", "");

      string tableRowsRegexPattern = @"(<tr.*?>.*?<\/tr>)";
      Regex tableRowsRegex = new Regex(tableRowsRegexPattern);

      if (!tableRowsRegex.IsMatch(html))
      {
        return new List<string>();
      }

      MatchCollection matches = tableRowsRegex.Matches(html);
      List<string> rows = new List<string>();

      foreach (Match match in matches)
      {
        rows.Add(match.Value);
      }

      return rows;
    }
    #endregion
  }
}
