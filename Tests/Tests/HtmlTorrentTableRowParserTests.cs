using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PirateAPI.Parsers.Torrents;
using PirateAPITests.Tests.StubClasses;
using PirateAPITests.Properties;

namespace PirateAPITests.Tests
{
  [TestFixture]
  public class HtmlTorrentTableRowParserTests
  {
    [Test]
    public void TestParseVipUploaderStatus()
    {
      string unparsedRow = Resources.TorrentRowVipUploader;

      HtmlTorrentTableRowParser parser = new HtmlTorrentTableRowParser(new StubLogger());

      Torrent torrent = parser.ParseRow(unparsedRow);
      Torrent correctTorrent = new Torrent
      {
        Title = "Rick and Morty Season 2 [WEBRIP] [1080p] [HEVC]",
        UploaderName = ".BONE.",
        Link = @"magnet:?xt=urn:btih:0494a80532b5b05dde567c61220d93406b7e22e7&dn=Rick+and+Morty+Season+2+%5BWEBRIP%5D+%5B1080p%5D+%5BHEVC%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
        PublishDate = new DateTime(2015, 11, 3),
        UploaderStatus = TorrentUploaderStatus.Vip,
        Size = 2394284168,
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
        Title = "Stranger Things vol. 1 & 2 soundtrack",
        UploaderName = "Rant423",
        Link = @"magnet:?xt=urn:btih:d2a7e42ca278f125e3c27b62ad3381e713d1f6a8&dn=Stranger+Things+vol.+1+%26amp%3B+2+soundtrack&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
        PublishDate = new DateTime(DateTime.Now.Year, 8, 19, 12, 55, 0),
        UploaderStatus = TorrentUploaderStatus.None,
        Size = 353204324,
        Seeds = 99,
        Leeches = 17
      };

      Assert.AreEqual(correctTorrent, torrent);
    }

    [Test]
    public void TestParseTrustedUploaderStatus()
    {
      string unparsedRow = Resources.TorrentRowTrustedUploader;

      HtmlTorrentTableRowParser parser = new HtmlTorrentTableRowParser(new StubLogger());

      Torrent torrent = parser.ParseRow(unparsedRow);
      Torrent correctTorrent = new Torrent
      {
        Title = "Stranger.Things.S01E05.720p.WEBRip.x264-SKGTV[ettv]",
        UploaderName = "EtHD",
        Link = @"magnet:?xt=urn:btih:c17abf3885fd16a5e5405295ac6212c56700ac6c&dn=Stranger.Things.S01E05.720p.WEBRip.x264-SKGTV%5Bettv%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
        PublishDate = new DateTime(DateTime.Now.Year, 7, 15, 18, 23, 0),
        UploaderStatus = TorrentUploaderStatus.Trusted,
        Size = 1403763699,
        Seeds = 238,
        Leeches = 43
      };

      Assert.AreEqual(correctTorrent, torrent);
    }

    [Test]
    public void TestParses0SeedsAnd0Leeches()
    {
      string unparsedRow = Resources.TorrentRow0Seeds0Leeches;

      HtmlTorrentTableRowParser parser = new HtmlTorrentTableRowParser(new StubLogger());

      Torrent torrent = parser.ParseRow(unparsedRow);
      Torrent correctTorrent = new Torrent
      {
        Title = "Edie Brickell & New Bohemians - Stranger Things (2006)",
        UploaderName = "CanadaJoe",
        Link = @"magnet:?xt=urn:btih:64db09320c813375fad8d14782bff07286adfbfe&dn=Edie+Brickell+%26+New+Bohemians+-+Stranger+Things+%282006%29&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
        PublishDate = new DateTime(2010, 12, 24),
        UploaderStatus = TorrentUploaderStatus.None,
        Size = 136507588,
        Seeds = 0,
        Leeches = 0
      };

      Assert.AreEqual(correctTorrent, torrent);
    }

    [Test]
    public void TestParseTitleRemoveSurroundWhitespace()
    {
      string unparsedRow = Resources.TorrentRowWhiteSpaceAroundTitle;

      HtmlTorrentTableRowParser parser = new HtmlTorrentTableRowParser(new StubLogger());

      Torrent torrent = parser.ParseRow(unparsedRow);
      Torrent correctTorrent = new Torrent
      {
        Title = "Rick and Morty Season 2 [WEBRIP] [1080p] [HEVC]",
        UploaderName = ".BONE.",
        Link = @"magnet:?xt=urn:btih:0494a80532b5b05dde567c61220d93406b7e22e7&dn=Rick+and+Morty+Season+2+%5BWEBRIP%5D+%5B1080p%5D+%5BHEVC%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
        PublishDate = new DateTime(2015, 11, 3),
        UploaderStatus = TorrentUploaderStatus.Vip,
        Size = 2394284168,
        Seeds = 590,
        Leeches = 109
      };

      Assert.AreEqual(correctTorrent, torrent);
    }

    [Test]
    public void TestParseAnonymousUploader()
    {
      string unparsedRow = Resources.TorrentRowAnonymousUploader;

      HtmlTorrentTableRowParser parser = new HtmlTorrentTableRowParser(new StubLogger());

      Torrent torrent = parser.ParseRow(unparsedRow);
      Torrent correctTorrent = new Torrent
      {
        Title = "Rick and Morty Season 1 [1080p] [HEVC]",
        Link = "magnet:?xt=urn:btih:08ad112d3469f45ed490ffed8253d48aa01e702d&dn=Rick+and+Morty+Season+1+%5B1080p%5D+%5BHEVC%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
        PublishDate = new DateTime(2015, 7, 26),
        UploaderName = "Anonymous",
        UploaderStatus = TorrentUploaderStatus.None,
        Size = 3199750636,
        Seeds = 364,
        Leeches = 60
      };

      Assert.AreEqual(correctTorrent, torrent);
    }

    [Test]
    public void TestParsePublishTodayWord()
    {
      string unparsedRow = Resources.TorrentRowTodayWordPublish;

      HtmlTorrentTableRowParser parser = new HtmlTorrentTableRowParser(new StubLogger(), new DateTime(2016, 11, 8, 14, 6, 0));

      Torrent torrent = parser.ParseRow(unparsedRow);
      Torrent correctTorrent = new Torrent
      {
        Title = "BBC.World.News.2016.09.21.(Eng.Subs).SDTV.x264-[2Maverick]",
        Link = "magnet:?xt=urn:btih:2e39ea1e7bc8bb4bf336f8107b33e4cbe8520b6e&dn=BBC.World.News.2016.09.21.%28Eng.Subs%29.SDTV.x264-%5B2Maverick%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
        PublishDate = new DateTime(2016, 11, 8, 10, 40, 00),
        UploaderName = "TvTeam",
        UploaderStatus = TorrentUploaderStatus.Vip,
        Size = 196608000,
        Seeds = 0,
        Leeches = 2
      };

      Assert.AreEqual(correctTorrent, torrent);
    }

    [Test]
    public void TestParsePublishMinutesAgo()
    {
      string unparsedRow = Resources.TorrentRowMinutesAgoPublish;

      HtmlTorrentTableRowParser parser = new HtmlTorrentTableRowParser(new StubLogger(), new DateTime(2016, 11, 8, 14, 6, 0));

      Torrent torrent = parser.ParseRow(unparsedRow);
      Torrent correctTorrent = new Torrent
      {
        Title = "Mothers.and.Daughters.2016.BRRip.XviD.AC3-EVO",
        Link = "magnet:?xt=urn:btih:9a5ba6f454374e561445b793bd241d0ab6d4d716&dn=Mothers.and.Daughters.2016.BRRip.XviD.AC3-EVO&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
        PublishDate = new DateTime(2016, 11, 8, 13, 16, 00),
        UploaderName = "TvTeam",
        UploaderStatus = TorrentUploaderStatus.Vip,
        Size = 1513975971,
        Seeds = 0,
        Leeches = 0
      };

      Assert.AreEqual(correctTorrent, torrent);
    }

    [Test]
    public void TestParsePublishYdayWord()
    {
      string unparsedRow = Resources.TorrentRowYdayPublish;

      HtmlTorrentTableRowParser parser = new HtmlTorrentTableRowParser(new StubLogger(), new DateTime(2016, 11, 8, 14, 6, 0));

      Torrent torrent = parser.ParseRow(unparsedRow);
      Torrent correctTorrent = new Torrent
      {
        Title = "Occultic.Nine.S01E05.720p.HEVC.x265-MeGusta",
        Link = "magnet:?xt=urn:btih:cd73cdc6abbd19a9e319dc3607ec7c3e3d107b39&dn=Occultic.Nine.S01E05.720p.HEVC.x265-MeGusta&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
        PublishDate = new DateTime(2016, 11, 7, 20, 50, 00),
        UploaderName = "TvTeam",
        UploaderStatus = TorrentUploaderStatus.Vip,
        Size = 966,
        Seeds = 0,
        Leeches = 3
      };

      Assert.AreEqual(correctTorrent, torrent);
    }

    [Test]
    public void TestParseNonTorrentRow()
    {
      string unparsedRow = Resources.NonTorrentRow;

      HtmlTorrentTableRowParser parser = new HtmlTorrentTableRowParser(new StubLogger());

      Torrent torrent = parser.ParseRow(unparsedRow);

      Assert.IsNull(torrent);
    }
  }
}
