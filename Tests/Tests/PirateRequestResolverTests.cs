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
    public void TestLimitUnder30()
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
          PublishDate = new DateTime(2015, 10, 12),
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
          PublishDate = new DateTime(2015, 10, 12),
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
      string responseString = Resources.PiratePageDifferentShowName;
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
        ShowName = "Arrow",
        PirateProxyURL = "http://fakepirateproxy.com",
      };

      List<Torrent> torrentStrings = resolver.Resolve(request);
      List<Torrent> correctResponse = new List<Torrent>
      {
        new Torrent()
        {
          Title = "Arrow.S04E23.HDTV.x264-LOL[ettv]",
          Link = "magnet:?xt=urn:btih:d83c61ea0b60b641cf13f61bfbf8095113e9573d&amp;dn=Arrow.S04E23.HDTV.x264-LOL%5Bettv%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2016, 5, 26, 3, 21, 0),
          UploaderName = "ettv",
          UploaderStatus = TorrentUploaderStatus.Vip,
          Size = 315358342,
          Seeds = 287,
          Leeches = 17
        },
        new Torrent()
        {
          Title = "Arrow.S04E15.HDTV.x264-LOL[ettv]",
          Link = "magnet:?xt=urn:btih:941a4cc66d4cb39b23a5cc9f59948eb22f72eff5&amp;dn=Arrow.S04E15.HDTV.x264-LOL%5Bettv%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2016, 2, 25, 3, 1, 0),
          UploaderName = "ettv",
          UploaderStatus = TorrentUploaderStatus.Vip,
          Size = 256413261,
          Seeds = 279,
          Leeches = 8
        },
        new Torrent()
        {
          Title = "Arrow Season 4 Complete x264-TJ [TJET]",
          Link = "magnet:?xt=urn:btih:0a3a4ac8aa690951a5d19593e8c1b83a61382baf&amp;dn=Arrow+Season+4+Complete+x264-TJ+%5BTJET%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2016, 5, 26, 4, 5, 0),
          UploaderName = "CenaCme",
          UploaderStatus = TorrentUploaderStatus.Vip,
          Size = 6579307167,
          Seeds = 268,
          Leeches = 132
        },
        new Torrent()
        {
          Title = "Arrow.S04E11.HDTV.x264-LOL[ettv]",
          Link = "magnet:?xt=urn:btih:c1c2a53e4bf77af86550f4b0ea64287acdda248d&amp;dn=Arrow.S04E11.HDTV.x264-LOL%5Bettv%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2016, 1, 28, 3, 3, 0),
          UploaderName = "ettv",
          UploaderStatus = TorrentUploaderStatus.Vip,
          Size = 247661544,
          Seeds = 255,
          Leeches = 13
        },
        new Torrent()
        {
          Title = "Arrow.S04E20.HDTV.x264-LOL[ettv]",
          Link = "magnet:?xt=urn:btih:25387dadd54dade2a5ccba39b3a6e206886a00d0&amp;dn=Arrow.S04E20.HDTV.x264-LOL%5Bettv%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2016, 5, 5, 3, 0, 0),
          UploaderName = "ettv",
          UploaderStatus = TorrentUploaderStatus.Vip,
          Size = 253518567,
          Seeds = 244,
          Leeches = 12
        }
      };
      Assert.AreEqual(correctResponse, torrentStrings);

      List<string> addressesRequested = new List<string>
      {
        "http://fakepirateproxy.com/search/Arrow/0/99/205,208"
      };
      Assert.AreEqual(addressesRequested, webClient.RequestsMade);
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
        Offset = 15,
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
          Title = "Rick and Morty S01E07 HDTV x264-2HD [eztv]",
          Link = "magnet:?xt=urn:btih:607be29a58b962d9baa3b5dbd2477e2fccc2c66e&amp;dn=Rick+and+Morty+S01E07+HDTV+x264-2HD+%5Beztv%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2014, 4, 12),
          UploaderName = "eztv",
          UploaderStatus = TorrentUploaderStatus.Vip,
          Size = 148530007,
          Seeds = 31,
          Leeches = 9
        },
        new Torrent()
        {
          Title = "Rick.and.Morty.S02E01.HDTV.x264-BATV[ettv]",
          Link = "magnet:?xt=urn:btih:f9e844d430fd38e36ece24938c3d613f875af3eb&amp;dn=Rick.and.Morty.S02E01.HDTV.x264-BATV%5Bettv%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2015, 7, 27),
          UploaderName = "ettv",
          UploaderStatus = TorrentUploaderStatus.Vip,
          Size = 217987493,
          Seeds = 30,
          Leeches = 5
        },
        new Torrent()
        {
          Title = "Rick.and.Morty.S02E08.720p.HDTV.x264-BATV[EtHD]",
          Link = "magnet:?xt=urn:btih:39749ebdafc47a3287c52e04f6da0c6172a43143&amp;dn=Rick.and.Morty.S02E08.720p.HDTV.x264-BATV%5BEtHD%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2015, 9, 21),
          UploaderName = "EtHD",
          UploaderStatus = TorrentUploaderStatus.Vip,
          Size = 478054108,
          Seeds = 27,
          Leeches = 18
        },
        new Torrent()
        {
          Title = "Rick And Morty S01E10 HDTV x264-MiNDTHEGAP [eztv]",
          Link = "magnet:?xt=urn:btih:61c34ea7b6e51c69b07ee0df34f2acccc3926e93&amp;dn=Rick+And+Morty+S01E10+HDTV+x264-MiNDTHEGAP+%5Beztv%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2014, 4, 12),
          UploaderName = "eztv",
          UploaderStatus = TorrentUploaderStatus.Vip,
          Size = 180212379,
          Seeds = 26,
          Leeches = 6
        },
        new Torrent()
        {
          Title = "Rick and Morty S01E09 PROPER HDTV x264-2HD [eztv]",
          Link = "magnet:?xt=urn:btih:a5a508354eb847f681b84a28f9ed2e265ae65686&amp;dn=Rick+and+Morty+S01E09+PROPER+HDTV+x264-2HD+%5Beztv%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2014, 4, 12),
          UploaderName = "eztv",
          UploaderStatus = TorrentUploaderStatus.Vip,
          Size = 140810356,
          Seeds = 23,
          Leeches = 9
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
    public void TestOffsetExactly30()
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
        Offset = 30,
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
            Link =
              "magnet:?xt=urn:btih:0494a80532b5b05dde567c61220d93406b7e22e7&amp;dn=Rick+and+Morty+Season+2+%5BWEBRIP%5D+%5B1080p%5D+%5BHEVC%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
            Link =
              "magnet:?xt=urn:btih:08ad112d3469f45ed490ffed8253d48aa01e702d&amp;dn=Rick+and+Morty+Season+1+%5B1080p%5D+%5BHEVC%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
            Link =
              "magnet:?xt=urn:btih:e46bca998f72411f7ec43f88a1ff3460f4c43fa4&amp;dn=Rick+and+Morty+Season+1+%5BUNCENSORED%5D+%5BBDRip%5D+%5B1080p%5D+%5BHEVC%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
            Link =
              "magnet:?xt=urn:btih:8cdcb24c90c06fb1bf2c69485c76390aed50c3a5&amp;dn=Rick+and+Morty+Season+2+Complete+720p+MKV&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
            PublishDate = new DateTime(2015, 10, 12),
            UploaderName = "ToyUp",
            UploaderStatus = TorrentUploaderStatus.None,
            Size = 5443871048,
            Seeds = 131,
            Leeches = 25
          },
          new Torrent()
          {
            Title = "Rick.and.Morty.S02E10.HDTV.x264-BATV[ettv]",
            Link =
              "magnet:?xt=urn:btih:475ee9147c9c3a4c2c1f3c2a7c078d8552cf7598&amp;dn=Rick.and.Morty.S02E10.HDTV.x264-BATV%5Bettv%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
        "http://fakepirateproxy.com/search/Rick%20And%20Morty/1/99/205,208"
      };
      Assert.AreEqual(addressesRequested, webClient.RequestsMade);
    }

    [Test]
    public void TestOffsetOver30()
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
        Offset = 45,
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
          Title = "Rick and Morty S01E07 HDTV x264-2HD [eztv]",
          Link = "magnet:?xt=urn:btih:607be29a58b962d9baa3b5dbd2477e2fccc2c66e&amp;dn=Rick+and+Morty+S01E07+HDTV+x264-2HD+%5Beztv%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2014, 4, 12),
          UploaderName = "eztv",
          UploaderStatus = TorrentUploaderStatus.Vip,
          Size = 148530007,
          Seeds = 31,
          Leeches = 9
        },
        new Torrent()
        {
          Title = "Rick.and.Morty.S02E01.HDTV.x264-BATV[ettv]",
          Link = "magnet:?xt=urn:btih:f9e844d430fd38e36ece24938c3d613f875af3eb&amp;dn=Rick.and.Morty.S02E01.HDTV.x264-BATV%5Bettv%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2015, 7, 27),
          UploaderName = "ettv",
          UploaderStatus = TorrentUploaderStatus.Vip,
          Size = 217987493,
          Seeds = 30,
          Leeches = 5
        },
        new Torrent()
        {
          Title = "Rick.and.Morty.S02E08.720p.HDTV.x264-BATV[EtHD]",
          Link = "magnet:?xt=urn:btih:39749ebdafc47a3287c52e04f6da0c6172a43143&amp;dn=Rick.and.Morty.S02E08.720p.HDTV.x264-BATV%5BEtHD%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2015, 9, 21),
          UploaderName = "EtHD",
          UploaderStatus = TorrentUploaderStatus.Vip,
          Size = 478054108,
          Seeds = 27,
          Leeches = 18
        },
        new Torrent()
        {
          Title = "Rick And Morty S01E10 HDTV x264-MiNDTHEGAP [eztv]",
          Link = "magnet:?xt=urn:btih:61c34ea7b6e51c69b07ee0df34f2acccc3926e93&amp;dn=Rick+And+Morty+S01E10+HDTV+x264-MiNDTHEGAP+%5Beztv%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2014, 4, 12),
          UploaderName = "eztv",
          UploaderStatus = TorrentUploaderStatus.Vip,
          Size = 180212379,
          Seeds = 26,
          Leeches = 6
        },
        new Torrent()
        {
          Title = "Rick and Morty S01E09 PROPER HDTV x264-2HD [eztv]",
          Link = "magnet:?xt=urn:btih:a5a508354eb847f681b84a28f9ed2e265ae65686&amp;dn=Rick+and+Morty+S01E09+PROPER+HDTV+x264-2HD+%5Beztv%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2014, 4, 12),
          UploaderName = "eztv",
          UploaderStatus = TorrentUploaderStatus.Vip,
          Size = 140810356,
          Seeds = 23,
          Leeches = 9
        }
      };
      Assert.AreEqual(correctResponse, torrentStrings);

      List<string> addressesRequested = new List<string>
      {
        "http://fakepirateproxy.com/search/Rick%20And%20Morty/1/99/205,208"
      };
      Assert.AreEqual(addressesRequested, webClient.RequestsMade);
    }

    [Test]
    public void TestVideoQualityHd()
    {
      string responseString = Resources.PiratePageHDOnly;
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
        Quality = VideoQuality.HD,
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
          Size = 2399284168,
          Seeds = 573,
          Leeches = 112
        },
        new Torrent()
        {
          Title = "Rick and Morty Season 1 [1080p] [HEVC]",
          Link = "magnet:?xt=urn:btih:08ad112d3469f45ed490ffed8253d48aa01e702d&amp;dn=Rick+and+Morty+Season+1+%5B1080p%5D+%5BHEVC%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2015, 7, 26),
          UploaderName = "Anonymous",
          UploaderStatus = TorrentUploaderStatus.None,
          Size = 2458868777,
          Seeds = 341,
          Leeches = 75
        },
        new Torrent()
        {
          Title = "Rick and Morty Season 1 [UNCENSORED] [BDRip] [1080p] [HEVC]",
          Link = "magnet:?xt=urn:btih:e46bca998f72411f7ec43f88a1ff3460f4c43fa4&amp;dn=Rick+and+Morty+Season+1+%5BUNCENSORED%5D+%5BBDRip%5D+%5B1080p%5D+%5BHEVC%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2015, 11, 03),
          UploaderName = ".BONE.",
          UploaderStatus = TorrentUploaderStatus.Vip,
          Size = 3446711255,
          Seeds = 209,
          Leeches = 53
        },
        new Torrent()
        {
          Title = "Rick and Morty Season 2[BDRip 1080p AC3][AtaraxiaPrime]",
          Link = "magnet:?xt=urn:btih:668c251eab6a3155fbe7a7ef52bd062787c49320&amp;dn=Rick+and+Morty+Season+2%5BBDRip+1080p+AC3%5D%5BAtaraxiaPrime%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2016, 7, 2, 3, 14, 0),
          UploaderName = "AtaraxiaPrime",
          UploaderStatus = TorrentUploaderStatus.None,
          Size = 4955735608,
          Seeds = 49,
          Leeches = 10
        },
        new Torrent()
        {
          Title = "Rick and Morty Season 1 (1280x720) [Phr0stY",
          Link = "magnet:?xt=urn:btih:827e404e8dc1e32542b23a41036b6e64b6bc2d66&amp;dn=Rick+and+Morty+Season+1+%281280x720%29+%5BPhr0stY&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2014, 4, 15),
          UploaderName = "frostyon420",
          UploaderStatus = TorrentUploaderStatus.Trusted,
          Size = 6211457862,
          Seeds = 37,
          Leeches = 8
        }
      };
      Assert.AreEqual(correctResponse, torrentStrings);

      List<string> addressesRequested = new List<string>
      {
        "http://fakepirateproxy.com/search/Rick%20And%20Morty/0/99/208"
      };
      Assert.AreEqual(addressesRequested, webClient.RequestsMade);
    }

    [Test]
    public void TestVideoQualitySd()
    {
      string responseString = Resources.PiratePageSDOnly;
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
        Quality = VideoQuality.SD,
        ExtendedAttributes = true,
        ShowName = "Rick+And+Morty",
        PirateProxyURL = "http://fakepirateproxy.com",
      };

      List<Torrent> torrentStrings = resolver.Resolve(request);
      List<Torrent> correctResponse = new List<Torrent>
      {
        new Torrent()
        {
          Title = "Rick and Morty Season 2 Complete 720p MKV",
          Link = "magnet:?xt=urn:btih:8cdcb24c90c06fb1bf2c69485c76390aed50c3a5&amp;dn=Rick+and+Morty+Season+2+Complete+720p+MKV&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2015, 10, 12),
          UploaderName = "ToyUp",
          UploaderStatus = TorrentUploaderStatus.None,
          Size = 5439160212,
          Seeds = 119,
          Leeches = 25
        },
        new Torrent()
        {
          Title = "Rick and Morty S01E02 HDTV x264-KILLERS [eztv]",
          Link = "magnet:?xt=urn:btih:55dad47c3a28b1c2f482206a47dff151fac45aa4&amp;dn=Rick+and+Morty+S01E02+HDTV+x264-KILLERS+%5Beztv%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2014, 4, 12),
          UploaderName = "eztv",
          UploaderStatus = TorrentUploaderStatus.Vip,
          Size = 172354188,
          Seeds = 71,
          Leeches = 8
        },
        new Torrent()
        {
          Title = "Rick.and.Morty.S02E10.HDTV.x264-BATV[ettv]",
          Link = "magnet:?xt=urn:btih:475ee9147c9c3a4c2c1f3c2a7c078d8552cf7598&amp;dn=Rick.and.Morty.S02E10.HDTV.x264-BATV%5Bettv%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2015, 11, 5),
          UploaderName = "ettv",
          UploaderStatus = TorrentUploaderStatus.Vip,
          Size = 149558451,
          Seeds = 70,
          Leeches = 19
        },
        new Torrent()
        {
          Title = "Rick and Morty S01E01 HDTV x264-2HD [eztv]",
          Link = "magnet:?xt=urn:btih:d335feab9035201c28d5cd59e406d3031374d3c6&amp;dn=Rick+and+Morty+S01E01+HDTV+x264-2HD+%5Beztv%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2014, 4, 12),
          UploaderName = "eztv",
          UploaderStatus = TorrentUploaderStatus.Vip,
          Size = 154004655,
          Seeds = 69,
          Leeches = 4
        },
        new Torrent()
        {
          Title = "Rick and Morty S01E03 HDTV x264-KILLERS [eztv]",
          Link = "magnet:?xt=urn:btih:2b3a3ca8b82d6c8b339159be8a15a361236b8910&amp;dn=Rick+and+Morty+S01E03+HDTV+x264-KILLERS+%5Beztv%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2014, 4, 12),
          UploaderName = "eztv",
          UploaderStatus = TorrentUploaderStatus.Vip,
          Size = 188800474,
          Seeds = 56,
          Leeches = 12
        }
      };
      Assert.AreEqual(correctResponse, torrentStrings);

      List<string> addressesRequested = new List<string>
      {
        "http://fakepirateproxy.com/search/Rick%20And%20Morty/0/99/205"
      };
      Assert.AreEqual(addressesRequested, webClient.RequestsMade);
    }

    [Test]
    public void TestSingleSeason()
    {
      string responseString = Resources.PiratePageSingleSeason;
      List<string> responseStrings = new List<string>
      {
        responseString
      };

      StubWebClient webClient = new StubWebClient(responseStrings);

      PirateRequestResolver resolver = new PirateRequestResolver(new StubLogger(), webClient);
      PirateRequest request = new PirateRequest
      {
        Offset = 0,
        Limit = 0,
        Quality = VideoQuality.Both,
        ExtendedAttributes = true,
        ShowName = "Rick+And+Morty",
        PirateProxyURL = "http://fakepirateproxy.com",
        Season = 2
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
          Seeds = 564,
          Leeches = 126
        },
        new Torrent()
        {
          Title = "Rick and Morty Season 2 Complete 720p MKV",
          Link = "magnet:?xt=urn:btih:8cdcb24c90c06fb1bf2c69485c76390aed50c3a5&amp;dn=Rick+and+Morty+Season+2+Complete+720p+MKV&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2015, 10, 12),
          UploaderName = "ToyUp",
          UploaderStatus = TorrentUploaderStatus.None,
          Size = 5443871048,
          Seeds = 117,
          Leeches = 28
        },
        new Torrent()
        {
          Title = "Rick and Morty Season 2[BDRip 1080p AC3][AtaraxiaPrime]",
          Link = "magnet:?xt=urn:btih:668c251eab6a3155fbe7a7ef52bd062787c49320&amp;dn=Rick+and+Morty+Season+2%5BBDRip+1080p+AC3%5D%5BAtaraxiaPrime%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2016, 7, 2, 3, 14, 0),
          UploaderName = "AtaraxiaPrime",
          UploaderStatus = TorrentUploaderStatus.None,
          Size = 4955735608,
          Seeds = 43,
          Leeches = 7
        },
        new Torrent()
        {
          Title = "Rick.and.Morty.Season.2.1080p.BluRay.x264.with.commentary.tracks",
          Link = "magnet:?xt=urn:btih:d64161416fe4cba97131237d810dfc77f6640d14&amp;dn=Rick.and.Morty.Season.2.1080p.BluRay.x264.with.commentary.tracks&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2016, 7, 2, 3, 14, 0),
          UploaderName = "AtaraxiaPrime",
          UploaderStatus = TorrentUploaderStatus.None,
          Size = 9334060183,
          Seeds = 36,
          Leeches = 5
        },
      };
      Assert.AreEqual(correctResponse, torrentStrings);

      List<string> addressesRequested = new List<string>
      {
        "http://fakepirateproxy.com/search/Rick%20And%20Morty%20Season%202/0/99/205,208",
        "http://fakepirateproxy.com/search/Rick%20And%20Morty%20Season%202/1/99/205,208"
      };
      Assert.AreEqual(addressesRequested, webClient.RequestsMade);
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
