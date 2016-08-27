using PirateAPI.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PirateAPI.Parser
{
  public class TorznabQueryParser
  {
    private ILogger logger;

    public TorznabQueryParser(ILogger logger)
    {
      this.logger = logger;
    }

    public PirateRequest Parse(string torznabQuery, string pirateProxy)
    {
      if (string.IsNullOrWhiteSpace(torznabQuery))
        logger.LogError("PirateRequest.Parse was passed a null or empty string for torznabQuery");

      if (string.IsNullOrWhiteSpace(pirateProxy))
        logger.LogError("PirateRequest.Parse was passed a null or empty string for pirateProxy");

      string regexPattern = @"http.*\/api\?(.*)";

      Regex regex = new Regex(regexPattern);

      if (!regex.IsMatch(torznabQuery))
      {
        logger.LogError($"Torznab request {torznabQuery} didn't match pattern {regexPattern}");
        return null;
      }

      Match match = regex.Match(torznabQuery);

      if (match.Groups.Count != 1)
      {
        logger.LogError($"Torznab request {torznabQuery} had {match.Groups.Count} capture groups, not 1 as expected");
        return null;
      }

      string[] args = match.Groups[0].Value.Split('&');

    }
  }
}
