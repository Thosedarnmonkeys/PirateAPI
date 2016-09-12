using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PirateAPI.Logging;
using PirateAPI.Parsers.Torrents;
using PirateAPI.Properties;

namespace PirateAPI.ResponseBuilders.Torznab
{
  public class TorznabResponseBuilder
  {
    #region private fields
    private ILogger logger;
    #endregion

    #region private consts
    string dateFormat = "ddd, dd MMM yyyy HH:mm:ss +0000";
    #endregion

    #region constructor
    public TorznabResponseBuilder(ILogger logger)
    {
      this.logger = logger;
    } 
    #endregion

    #region public methods
    public string BuildResponse(List<Torrent> torrents)
    {
      if (torrents == null)
      {
        logger.LogError("TorznabResponseBuilder.BuildResponse was passed a null list of torrents");
        return "";
      }

      StringBuilder builder = new StringBuilder();
      foreach (Torrent torrent in torrents)
      {
        string item = BuildSingleItem(torrent);
        builder.Append(item);
      }

      string response = Resources.TorznabResponseTemplate;
      response = string.Format(response, builder);
      response = response.Replace("\r", "");

      return response;
    }
    #endregion

    #region private methods
    private string BuildSingleItem(Torrent torrent)
    {
      if (torrent == null)
        return string.Empty;

      StringBuilder builder = new StringBuilder();
      builder.Append("<item>\n");
      builder.Append($"<title>{torrent.Title}</title>\n");
      builder.Append($"<link>{torrent.Link}</link>\n");
      builder.Append($"<pubDate>{torrent.PublishDate.ToString(dateFormat)}</pubDate>\n");
      builder.Append($"<size>{torrent.Size}</size>\n");
      builder.Append($"<enclosure url=\"{torrent.Link}\" length=\"{torrent.Size}\" type=\"application/x-bittorrent\" />\n");
      builder.Append($"<torznab:attr name=\"seeders\" value=\"{torrent.Seeds}\" />\n");
      builder.Append($"<torznab:attr name=\"peers\" value=\"{torrent.Seeds + torrent.Leeches}\" />\n");
      builder.Append("</item>\n");

      return builder.ToString();
    }
    #endregion
  }
}
