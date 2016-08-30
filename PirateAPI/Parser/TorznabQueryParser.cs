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
    #region private consts
    private const string sdCategory = "5030";
    private const string hdCategory = "5040";
    #endregion

    #region private fields
    private ILogger logger;
    #endregion

    #region constructor
    public TorznabQueryParser(ILogger logger)
    {
      this.logger = logger;
    }
    #endregion

    #region public methods
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

      if (match.Groups.Count != 2)
      {
        logger.LogError($"Torznab request {torznabQuery} had {match.Groups.Count} capture groups, not 2 as expected");
        return null;
      }

      string[] args = match.Groups[1].Value.Split('&');

      List<Tuple<string, string>> paramsAndValues = new List<Tuple<string, string>>();

      foreach (string arg in args)
      {
        string[] paramAndValue = arg.Split('=');

        if (paramAndValue.Length != 2)
        {
          logger.LogError($"Couldn't split arg {arg} into value and parameter");
          continue;
        }

        paramsAndValues.Add(new Tuple<string, string>(paramAndValue[0], paramAndValue[1]));
      }

      PirateRequest parsedRequest = new PirateRequest() { PirateProxyURL = pirateProxy};

      foreach (Tuple<string, string> tuple in paramsAndValues)
      {
        switch (tuple.Item1)
        {
          case "extended":
            parsedRequest.ExtendedAttributes = tuple.Item2 == "1";
            break;

          case "offset":
            parsedRequest.Offset = ParseInt(tuple.Item2, tuple.Item1) ?? 0;
            break;

          case "limit":
            parsedRequest.Limit = ParseInt(tuple.Item2, tuple.Item1);
            break;

          case "q":
            parsedRequest.ShowName = tuple.Item2;
            break;

          case "cat":
            parsedRequest.Quality = ParseShowQuality(tuple.Item2);
            break;

          case "season":
            parsedRequest.Season = ParseInt(tuple.Item2, tuple.Item1);
            break;

          case "ep":
            parsedRequest.Episode = ParseInt(tuple.Item2, tuple.Item1);
            break;

          case "maxage":
            parsedRequest.MaxAge = ParseInt(tuple.Item2, tuple.Item1);
            break;
        }
      }

      return parsedRequest;
    }

    #endregion


    #region private methods
    private int? ParseInt(string value, string paramName)
    {
      int parsedValue;
      if (!int.TryParse(value, out parsedValue))
      {
        logger.LogError($"Unable to parse value {value} to int, param name of {paramName}");
        return null;
      }

      return parsedValue;
    }

    private VideoQuality ParseShowQuality(string categories)
    {
      if (string.IsNullOrWhiteSpace(categories))
      {
        logger.LogError("TorznabQueryParser.ParseShowQuality was passed a null or empty string for show quality categories, defaulting to SD");
        return VideoQuality.SD;
      }

      string[] cats = categories.Split(',');

      bool sd = false;
      bool hd = false;

      foreach (string cat in cats)
      {
        if (cat == sdCategory)
          sd = true;

        if (cat == hdCategory)
          hd = true;
      }

      VideoQuality quality = VideoQuality.SD;

      if (sd && hd)
        quality = VideoQuality.Both;

      else if (hd)
        quality = VideoQuality.HD;

      return quality;
    }
    #endregion
  }
}
