using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    private Func<bool> okToMakeWebRequestFunc;
    #endregion

    #region constructor
    public HtmlTorrentTableRowWithoutMagnetLinkParser(string domain, Func<bool> okToMakeWebRequestFunc,IWebClient webclient, ILogger logger, CancellationToken token) : base(logger, token)
    {
      this.webClient = webclient;
      this.logger = logger;
      this.domain = domain;
      this.okToMakeWebRequestFunc = okToMakeWebRequestFunc;
    }
    #endregion

    #region public methods

    public override Torrent ParseRow(HtmlNode row)
    {
      token.ThrowIfCancellationRequested();

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

      //making a new HtmlDocument because agility is a piece of shit
      //using the below xpath will not work on the passed in HtmlNode, but taking its html and making a new HtmlDocument will
      //go fucking figure
      HtmlDocument bullshitFuckingFix = new HtmlDocument();
      bullshitFuckingFix.LoadHtml(row.OuterHtml);

      string pagePath = GetChildNodeAttributeValue(bullshitFuckingFix.DocumentNode, "//a[@title='Download this torrent using magnet']", "href");
      if (pagePath == null)
        return null;

      if (!okToMakeWebRequestFunc.Invoke())
        return null;

      token.ThrowIfCancellationRequested();

      string pageUrl = domain + pagePath;
      string torrentPage = webClient.DownloadString(pageUrl);
      if (torrentPage == null)
      {
        logger.LogError($"Tried to download page {pageUrl} but got no response");
        return null;
      }

      if (!okToMakeWebRequestFunc.Invoke())
        return null;

      token.ThrowIfCancellationRequested();

      var doc = new HtmlDocument();
      doc.LoadHtml(torrentPage);

      string magnetLink = GetChildNodeAttributeValue(doc.DocumentNode, "//a[@title='Get this torrent']", "href");
      return magnetLink?.Replace("&amp;", "&");
    }
    #endregion
  }
}
