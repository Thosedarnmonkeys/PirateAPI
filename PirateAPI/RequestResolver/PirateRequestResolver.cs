using PirateAPI.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PirateAPI.Logging;
using PirateAPI.WebClient;

namespace PirateAPI.RequestResolver
{
  public class PirateRequestResolver
  {
    #region private fields
    private ILogger logger;
    private IWebClient webClient;
    #endregion

    #region constructor
    public PirateRequestResolver(ILogger logger, IWebClient webClient)
    {
      this.logger = logger;
      this.webClient = webClient;
    }
    #endregion

    #region public methods
    public List<string> Resolve(PirateRequest request)
    {
      if (request == null)
      {
        logger.LogError("PirateRequestResolver.Resolve was passed a null request");
        return null;
      }

      string pirateProxy = string.IsNullOrWhiteSpace(request.PirateProxyURL) ? "" : request.PirateProxyURL;
      string searchArg = string.IsNullOrWhiteSpace(request.ShowName) ? "" : request.ShowName.Replace("+", "%20");
      double page = Math.Floor((double)request.Offset / 30);
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

      List<string> rows = new List<string>();

      while (rows.Count < request.Limit)
      {
        string requestUrl = $"{pirateProxy}/search/{searchArg}/{page}/99/{categories}";
        string[] rowArray = GetRowsFromAddress(requestUrl);




        page++;
      }

      


      return rows;
    }
    #endregion

    #region private methods
    private string[] GetRowsFromAddress(string address)
    {
      string piratePage = webClient.DownloadString(address);

      string tableRowsRegexPattern = @"(<tr>.*<\/tr>)";
      Regex tableRowsRegex = new Regex(tableRowsRegexPattern);

      throw new NotImplementedException();
    }
    #endregion
  }
}
