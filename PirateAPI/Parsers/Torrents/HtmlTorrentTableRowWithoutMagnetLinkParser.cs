using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
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

    public override Torrent ParseRow(HtmlNode row)
    {
      Torrent torrent = base.ParseRow(row);
      if (torrent == null)
        return null;

      return string.IsNullOrWhiteSpace(torrent.Link) ? null : torrent;
    }

    #endregion

    #region protected methods
    protected override string ParseLink(HtmlNode row)
    {
      if (row == null)
        return null;

      string pagePath = GetChildNodeAttributeValue(row, "td/a", "href");
      if (pagePath == null)
        return null;

      string pageUrl = domain + pagePath;
      string torrentPage = webClient.DownloadString(pageUrl);
      if (torrentPage == null)
      {
        logger.LogError($"Tried to download page {pageUrl} but got no response");
        return null;
      }

      var doc = new HtmlDocument();
      doc.LoadHtml(torrentPage);

      string magnetLink = GetChildNodeAttributeValue(doc.DocumentNode, "//body/div/div/div/div/div/div/div/div/a", "href");
      return magnetLink?.Replace("&amp;", "&");
    }
    #endregion
  }
}
