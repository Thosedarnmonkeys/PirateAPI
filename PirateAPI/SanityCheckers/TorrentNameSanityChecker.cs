using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PirateAPI.Logging;

namespace PirateAPI.SanityCheckers
{
  public class TorrentNameSanityChecker
  {
    #region private fields
    private ILogger logger;
    #endregion

    #region constructor
    public TorrentNameSanityChecker(ILogger logger)
    {
      this.logger = logger;
    } 
    #endregion

    #region public methods
    public bool Check(string showName, string input)
    {
      return CheckImpl(showName, null, null, input);
    }

    public bool Check(string showName, int season, string input)
    {
      return CheckImpl(showName, season, null, input);
    }

    public bool Check(string showName, int season, int episode, string input)
    {
      return CheckImpl(showName, season, episode, input);
    }
    #endregion


    #region private methods
    private bool CheckImpl(string showName, int? season, int? episode, string input)
    {
      if (string.IsNullOrWhiteSpace(showName))
      {
        logger.LogError($"TorrentNameSanityChecker.Check was passed a null or empty string for param {nameof(showName)}");
        return false;
      }

      if (string.IsNullOrWhiteSpace(input))
      {
        logger.LogError($"TorrentNameSanityChecker.Check was passed a null or empty string for param {nameof(input)}");
        return false;
      }

      string normalisedShowName = showName.Replace("&", "and").ToLower();

      string normalisedInput = input.Replace(".", " ")
                                    .Replace("_", " ")
                                    .Replace("-", " ")
                                    .Replace("&", "and")
                                    .ToLower();
                        
      if (!normalisedInput.Contains(normalisedShowName))
        return false;

      if (episode.HasValue && season.HasValue)
        return CheckSpecificEpisode(season.Value, episode.Value, normalisedInput);

      if (season.HasValue)
        return CheckSpecificSeason(season.Value, normalisedInput);

      if (episode.HasValue)
      {
        logger.LogError("TorrentNameSanityChecker.Check was passed an episode but no season");
        return false;
      }

      return true;
    }

    private bool CheckSpecificSeason(int season, string input)
    {
      string paddedInput = input.PadLeft(input.Length + 1)
                                .PadRight(input.Length + 2);


      List<string> exclusionStrings = new List<string>
      {
        " episode ",
        " ep ",
        " e ",
      };
      if (exclusionStrings.Any(es => paddedInput.Contains(es)))
        return false;

      string seasonString = $" season {season} ";
      if (paddedInput.Contains(seasonString))
        return true;

      string zeroedSeason = season < 10 ? "0" + season : season.ToString();
      string zeroedString = $" season {zeroedSeason} ";
      if (paddedInput.Contains(zeroedString))
        return true;

      string seasonStringAbbrev = $" s{season} ";
      if (paddedInput.Contains(seasonStringAbbrev))
        return true;

      string zeroedAbbrevString = $" s{zeroedSeason} ";
      if (paddedInput.Contains(zeroedAbbrevString))
        return true;

      return false;
    }

    private bool CheckSpecificEpisode(int season, int episode, string input)
    {
      string paddedInput = input.PadLeft(input.Length + 1)
                                .PadRight(input.Length + 2);

      string episodeString = $" s{season}e{episode} ";
      if (paddedInput.Contains(episodeString))
        return true;

      string spacedEpisodeString = $" s{season} e{episode} ";
      if (paddedInput.Contains(spacedEpisodeString))
        return true;

      string zeroedSeason = season < 10 ? "0" + season : season.ToString();
      string zeroedEpisode = episode < 10 ? "0" + episode : episode.ToString();
      string zeroedString = $" s{zeroedSeason}e{zeroedEpisode} ";
      if (paddedInput.Contains(zeroedString))
        return true;

      string spacedZeroedString = $" s{zeroedSeason} e{zeroedEpisode} ";
      if (paddedInput.Contains(spacedZeroedString))
        return true;

      string verboseString = $" season {season} episode {episode} ";
      if (paddedInput.Contains(verboseString))
        return true;

      return false;
    }
    #endregion
  }
}
