using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PirateAPI.Logging;
using PirateAPI.WebClient;

namespace PirateAPI.Parsers.Torrents
{
  public class HtmlTorrentTableRowWithoutMagnetLinkParser : HtmlTorrentTableRowParser
  {
    #region private fields
    private ILogger logger;
    private IWebClient webClient;
    private readonly string domain;
    #endregion

    #region constructor
    public HtmlTorrentTableRowWithoutMagnetLinkParser(string domain, IWebClient webclient, ILogger logger) : base(logger)
    {
      this.webClient = webclient;
      this.logger = logger;
      this.domain = domain;
    }
    #endregion

    #region public methods

    public override Torrent ParseRow(string rowString)
    {
      Torrent torrent = base.ParseRow(rowString);
      if (torrent == null)
        return null;

      return string.IsNullOrWhiteSpace(torrent.Link) ? null : torrent;
    }

    #endregion

    #region protected methods
    protected override string ParseLink(string rowString)
    {
      if (string.IsNullOrWhiteSpace(rowString))
        return null;

      string pageLinkPattern = @"div><a href=""(.*?)"" title=""Download this torrent using magnet";
      string pagePath = CheckMatchAndGetFirst(rowString, pageLinkPattern, "Link");

      if (pagePath == null)
        return null;

      string pageUrl = domain + pagePath;
      string torrentPage = webClient.DownloadString(pageUrl);
      if (torrentPage == null)
      {
        logger.LogError($"Tried to download page {pageUrl} but got no response");
        return null;
      }

      string magnetPattern = @"href=""(magnet:\?.*?)""";
      string magnetLink = CheckMatchAndGetFirst(torrentPage, magnetPattern, "Link");
      return magnetLink?.Replace("&amp;", "&");
    }
    #endregion
  }
}
