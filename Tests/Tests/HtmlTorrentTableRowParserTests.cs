using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PirateAPI.Parsers.Torrents;
using PirateAPITests.Tests.StubClasses;
using Tests.Properties;

namespace Tests.Tests
{
  [TestFixture]
  public class HtmlTorrentTableRowParserTests
  {
    [Test]
    public void TestParseBasicRow()
    {
      string unparsedRow = Resources.TorrentRowBasic;

      HtmlTorrentTableRowParser parser = new HtmlTorrentTableRowParser(new StubLogger());

      Torrent torrent = parser.ParseRow(unparsedRow);
      Torrent correctTorrent = new Torrent
      {
        Title = "Rick and Morty Season 2[WEBRIP][1080p][HEVC]",
        UploaderName = ".BONE.",
        Link = @"magnet:?xt=urn:btih:0494a80532b5b05dde567c61220d93406b7e22e7&amp;dn=Rick+and+Morty+Season+2+%5BWEBRIP%5D+%5B1080p%5D+%5BHEVC%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
        PublishDate = new DateTime(2016, 11, 3),
        UploaderStatus = TorrentUploaderStatus.Vip,
        Size = 2399284168,
        Seeds = 590,
        Leeches = 109
      };

      Assert.AreEqual(correctTorrent, torrent);
    }

    [Test]
    public void TestParseNoUploaderStatus()
    {
      string unparsedRow = Resources.TorrentRowNoUploaderStatus;

      HtmlTorrentTableRowParser parser = new HtmlTorrentTableRowParser(new StubLogger());

      Torrent torrent = parser.ParseRow(unparsedRow);
      Torrent correctTorrent = new Torrent
      {
        Title = "Stranger Things vol. 1 & 2 ",
        UploaderName = "Rant423",
        Link = @"magnet:?xt=urn:btih:d2a7e42ca278f125e3c27b62ad3381e713d1f6a8&amp;dn=Stranger+Things+vol.+1+%26amp%3B+2+soundtrack&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
        PublishDate = new DateTime(2016, 8, 19),
        UploaderStatus = TorrentUploaderStatus.None,
        Size = 353204324,
        Seeds = 82,
        Leeches = 14
      };

      Assert.AreEqual(correctTorrent, torrent);
    }

    [Test]
    public void TestParseTrustedUploaderStatus()
    {
      
    }
  }
}
