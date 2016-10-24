using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PirateAPI.Parsers.Torrents;
using PirateAPITests.Properties;
using PirateAPITests.Tests.StubClasses;

namespace PirateAPITests.Tests
{
  [TestFixture]
  public class HtmlTorrentTableRowWithoutMagnetLinkParserTests
  {
    [Test]
    public void TestSuccessfulParse()
    {
      List<string> response = new List<string>() {Resources.RickAndMortySeason2};
      StubWebClient webClient = new StubWebClient(response);
      StubLogger logger = new StubLogger();

      string torrentRow = Resources.TorrentRowNoMagnetLink;

      HtmlTorrentTableRowWithoutMagnetLinkParser parser = new HtmlTorrentTableRowWithoutMagnetLinkParser(webClient, logger);
      Torrent parsedTorrent = parser.ParseRow(torrentRow);

      Torrent correctTorrent = new Torrent()
      {
        Title = "Rick and Morty Season 2 [WEBRIP] [1080p] [HEVC]",
        UploaderName = ".BONE.",
        Link = @"magnet:?xt=urn:btih:0494a80532b5b05dde567c61220d93406b7e22e7&dn=Rick+and+Morty+Season+2+%5BWEBRIP%5D+%5B1080p%5D+%5BHEVC%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
        PublishDate = new DateTime(2015, 11, 3),
        UploaderStatus = TorrentUploaderStatus.Vip,
        Size = 2394284168,
        Seeds = 655,
        Leeches = 116
      };

      Assert.AreEqual(correctTorrent, parsedTorrent);
    }

    [Test]
    public void TestMagnetLinkMissing()
    {

    }

    [Test]
    public void TestDomainNotReponding()
    {
      
    }
  }
}
