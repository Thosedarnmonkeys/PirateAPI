using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
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
      HtmlDocument doc = new HtmlDocument();
      doc.LoadHtml(html);
      var nodes = doc.DocumentNode.SelectNodes("//tr");
      List<string> rows = nodes?.Select(n => n.OuterHtml).ToList() ?? new List<string>();
      for (int i = 0; i < rows.Count; i++)
      {
        rows[i] = rows[i].Replace("\n", "")
                         .Replace("\r", "")
                         .Replace("\t", "");
      }

      return rows;
    }
    #endregion
  }
}
