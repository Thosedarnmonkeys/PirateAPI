using NUnit.Framework;
using PirateAPI.Parsers.Torznab;
using PirateAPI.Parsers.Torrents;
using PirateAPI.RequestResolver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using PirateAPITests.Tests.StubClasses;
using Tests.Properties;
using Tests.Tests.StubClasses;

namespace Tests.Tests
{
  [TestFixture]
  public class PirateRequestResolverTests
  {
    [Test]
    public void TestHandleLimit5()
    {
      string responseString = Resources.PiratePageSearch;
      List<string> responseStrings = new List<string>
      {
        responseString
      };

      StubWebClient webClient = new StubWebClient(responseStrings);

      PirateRequestResolver resolver = new PirateRequestResolver(new StubLogger(), webClient);
      PirateRequest request = new PirateRequest
      {
        Limit = 5,
        Offset = 0,
        Quality = VideoQuality.Both,
        ExtendedAttributes = true,
        ShowName = "Rick+And+Morty",
        PirateProxyURL = "http://fakepirateproxy.com",
      };

      List<Torrent> torrentStrings = resolver.Resolve(request);
      List<Torrent> correctResponse = new List<Torrent>
      {
        new Torrent()
        {
          Title = "Rick and Morty Season 2 [WEBRIP] [1080p] [HEVC]",
          Link = "magnet:?xt=urn:btih:0494a80532b5b05dde567c61220d93406b7e22e7&amp;dn=Rick+and+Morty+Season+2+%5BWEBRIP%5D+%5B1080p%5D+%5BHEVC%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2015, 11, 3),
          UploaderName = ".BONE.",
          UploaderStatus = TorrentUploaderStatus.Vip,
          Size = 2394284168,
          Seeds = 590,
          Leeches = 109
        },
        new Torrent()
        {
          Title = "Rick and Morty Season 1 [1080p] [HEVC]",
          Link = "magnet:?xt=urn:btih:08ad112d3469f45ed490ffed8253d48aa01e702d&amp;dn=Rick+and+Morty+Season+1+%5B1080p%5D+%5BHEVC%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2015, 7, 26),
          UploaderName = "Anonymous",
          UploaderStatus = TorrentUploaderStatus.None,
          Size = 2458868777,
          Seeds = 364,
          Leeches = 60
        },
        new Torrent()
        {
          Title = "Rick and Morty Season 1 [UNCENSORED] [BDRip] [1080p] [HEVC]",
          Link = "magnet:?xt=urn:btih:e46bca998f72411f7ec43f88a1ff3460f4c43fa4&amp;dn=Rick+and+Morty+Season+1+%5BUNCENSORED%5D+%5BBDRip%5D+%5B1080p%5D+%5BHEVC%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2015, 11, 03),
          UploaderName = ".BONE.",
          UploaderStatus = TorrentUploaderStatus.Vip,
          Size = 3446711255,
          Seeds = 196,
          Leeches = 67
        },
        new Torrent()
        {
          Title = "Rick and Morty Season 2 Complete 720p MKV",
          Link = "magnet:?xt=urn:btih:8cdcb24c90c06fb1bf2c69485c76390aed50c3a5&amp;dn=Rick+and+Morty+Season+2+Complete+720p+MKV&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2016, 10, 12),
          UploaderName = "ToyUp",
          UploaderStatus = TorrentUploaderStatus.None,
          Size = 5443871048,
          Seeds = 131,
          Leeches = 25
        },
        new Torrent()
        {
          Title = "Rick.and.Morty.S02E10.HDTV.x264-BATV[ettv]",
          Link = "magnet:?xt=urn:btih:475ee9147c9c3a4c2c1f3c2a7c078d8552cf7598&amp;dn=Rick.and.Morty.S02E10.HDTV.x264-BATV%5Bettv%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2015, 10, 5),
          UploaderName = "ettv",
          UploaderStatus = TorrentUploaderStatus.Vip,
          Size = 149558395,
          Seeds = 66,
          Leeches = 22
        }
      };
      Assert.AreEqual(correctResponse, torrentStrings);

      List<string> addressesRequested = new List<string>
      {
        "http://fakepirateproxy.com/search/Rick%20And%20Morty/0/99/205,208"
      };
      Assert.AreEqual(addressesRequested, webClient.RequestsMade);
    }

    [Test]
    public void TestDifferentProxy()
    {
      string responseString = Resources.PiratePageSearch;
      List<string> responseStrings = new List<string>
      {
        responseString
      };

      StubWebClient webClient = new StubWebClient(responseStrings);

      PirateRequestResolver resolver = new PirateRequestResolver(new StubLogger(), webClient);
      PirateRequest request = new PirateRequest
      {
        Limit = 5,
        Offset = 0,
        Quality = VideoQuality.Both,
        ExtendedAttributes = true,
        ShowName = "Rick+And+Morty",
        PirateProxyURL = "http://adifferentfakepirateproxy.com",
      };

      List<Torrent> torrentStrings = resolver.Resolve(request);
      List<Torrent> correctResponse = new List<Torrent>
      {
        new Torrent()
        {
          Title = "Rick and Morty Season 2 [WEBRIP] [1080p] [HEVC]",
          Link = "magnet:?xt=urn:btih:0494a80532b5b05dde567c61220d93406b7e22e7&amp;dn=Rick+and+Morty+Season+2+%5BWEBRIP%5D+%5B1080p%5D+%5BHEVC%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2015, 11, 3),
          UploaderName = ".BONE.",
          UploaderStatus = TorrentUploaderStatus.Vip,
          Size = 2394284168,
          Seeds = 590,
          Leeches = 109
        },
        new Torrent()
        {
          Title = "Rick and Morty Season 1 [1080p] [HEVC]",
          Link = "magnet:?xt=urn:btih:08ad112d3469f45ed490ffed8253d48aa01e702d&amp;dn=Rick+and+Morty+Season+1+%5B1080p%5D+%5BHEVC%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2015, 7, 26),
          UploaderName = "Anonymous",
          UploaderStatus = TorrentUploaderStatus.None,
          Size = 2458868777,
          Seeds = 364,
          Leeches = 60
        },
        new Torrent()
        {
          Title = "Rick and Morty Season 1 [UNCENSORED] [BDRip] [1080p] [HEVC]",
          Link = "magnet:?xt=urn:btih:e46bca998f72411f7ec43f88a1ff3460f4c43fa4&amp;dn=Rick+and+Morty+Season+1+%5BUNCENSORED%5D+%5BBDRip%5D+%5B1080p%5D+%5BHEVC%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2015, 11, 03),
          UploaderName = ".BONE.",
          UploaderStatus = TorrentUploaderStatus.Vip,
          Size = 3446711255,
          Seeds = 196,
          Leeches = 67
        },
        new Torrent()
        {
          Title = "Rick and Morty Season 2 Complete 720p MKV",
          Link = "magnet:?xt=urn:btih:8cdcb24c90c06fb1bf2c69485c76390aed50c3a5&amp;dn=Rick+and+Morty+Season+2+Complete+720p+MKV&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2016, 10, 12),
          UploaderName = "ToyUp",
          UploaderStatus = TorrentUploaderStatus.None,
          Size = 5443871048,
          Seeds = 131,
          Leeches = 25
        },
        new Torrent()
        {
          Title = "Rick.and.Morty.S02E10.HDTV.x264-BATV[ettv]",
          Link = "magnet:?xt=urn:btih:475ee9147c9c3a4c2c1f3c2a7c078d8552cf7598&amp;dn=Rick.and.Morty.S02E10.HDTV.x264-BATV%5Bettv%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2015, 10, 5),
          UploaderName = "ettv",
          UploaderStatus = TorrentUploaderStatus.Vip,
          Size = 149558395,
          Seeds = 66,
          Leeches = 22
        }
      };
      Assert.AreEqual(correctResponse, torrentStrings);

      List<string> addressesRequested = new List<string>
      {
        "http://adifferentfakepirateproxy.com/search/Rick%20And%20Morty/0/99/205,208"
      };
      Assert.AreEqual(addressesRequested, webClient.RequestsMade);
    }

    [Test]
    public void TestShowName()
    {
      Assert.Fail();
    }

    [Test]
    public void TestLimitOver30()
    {
      Assert.Fail();
    }

    [Test]
    public void TestLimitExactly30()
    {
      Assert.Fail();
    }

    [Test]
    public void TestOffsetLessThan30()
    {
      Assert.Fail();
    }

    [Test]
    public void TestOffsetExactly30()
    {
      Assert.Fail();
    }

    [Test]
    public void TestOffsetOver30()
    {
      Assert.Fail();
    }

    [Test]
    public void TestVideoQualityHd()
    {
      Assert.Fail();
    }

    [Test]
    public void TestVideoQualitySd()
    {
      Assert.Fail();
    }

    [Test]
    public void TestSingleSeason()
    {
      Assert.Fail();
    }

    [Test]
    public void TestSingleEpisodeNoSeason()
    {
      Assert.Fail();
    }

    [Test]
    public void TestSingleEpisodeAndSeason()
    {
      Assert.Fail();
    }

    [Test]
    public void TestMaxAge()
    {
      Assert.Fail();
    }
  }
}
