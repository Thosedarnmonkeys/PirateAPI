using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PirateAPI.Logging;
using PirateAPI.WebClient;

namespace PirateAPI.ProxyInfoGatherers
{
  public class MagnetsInSearchTester
  {
    #region private fields
    private ILogger logger;
    private IWebClient webClient;
    private const string top100VideosPath = "/top/200";
    #endregion

    #region constructor

    public MagnetsInSearchTester(IWebClient webClient, ILogger logger)
    {
      this.webClient = webClient;
      this.logger = logger;
    }
    #endregion

    #region public methods
    public bool? DomainHasMagnetsInSearch(string domain)
    {
      if (string.IsNullOrWhiteSpace(domain))
      {
        logger.LogError("MagnetsInSearchTester.DomainHasMagnetsInSearch was passed a null or empty domain string");
        return null;
      }

      logger.LogMessage($"Testing domain {domain} for magnet links in search page");
      string top100Url = domain + top100VideosPath;
      string top100Page = webClient.DownloadString(top100Url);
      if (string.IsNullOrWhiteSpace(top100Page))
      {
        logger.LogError($"Domain {domain} didn't respond for url {top100Url}");
        return null;
      }

      string magnetLinkPattern = @"href=""magnet:\?.*?""";
      Regex regex = new Regex(magnetLinkPattern);

      bool hasMagnets = regex.IsMatch(top100Page);
      string hasHasNot = hasMagnets ? "has" : "doesn't have";
      logger.LogMessage($"Domain {domain} {hasHasNot} magnets in search page");

      return hasMagnets;
    } 
    #endregion
  }
}
