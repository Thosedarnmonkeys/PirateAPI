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
using PirateAPITests.Properties;

namespace PirateAPITests.Tests
{
  [TestFixture]
  public class PirateRequestResolverTests
  {
    [Test]
    public void TestLimitUnder30()
    {
      List<string> responseStrings = new List<string>
      {
        Resources.PiratePageTop100WithMagnets,
        Resources.PiratePageSearch
      };

      StubWebClient webClient = new StubWebClient(responseStrings);

      PirateRequestResolver resolver = new PirateRequestResolver(new StubLogger(), webClient, 100);
      PirateRequest request = new PirateRequest
      {
        Limit = 5,
        Offset = 0,
        Quality = VideoQuality.Both,
        ExtendedAttributes = true,
        ShowName = "Rick And Morty",
        PirateProxyURL = "http://fakepirateproxy.com",
      };

      List<Torrent> torrentStrings = resolver.Resolve(request);
      List<Torrent> correctResponse = new List<Torrent>
      {
        new Torrent()
        {
          Title = "Rick and Morty Season 2 [WEBRIP] [1080p] [HEVC]",
          Link = "magnet:?xt=urn:btih:0494a80532b5b05dde567c61220d93406b7e22e7&dn=Rick+and+Morty+Season+2+%5BWEBRIP%5D+%5B1080p%5D+%5BHEVC%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:08ad112d3469f45ed490ffed8253d48aa01e702d&dn=Rick+and+Morty+Season+1+%5B1080p%5D+%5BHEVC%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2015, 7, 26),
          UploaderName = "Anonymous",
          UploaderStatus = TorrentUploaderStatus.None,
          Size = 3195130865,
          Seeds = 364,
          Leeches = 60
        },
        new Torrent()
        {
          Title = "Rick and Morty Season 1 [UNCENSORED] [BDRip] [1080p] [HEVC]",
          Link = "magnet:?xt=urn:btih:e46bca998f72411f7ec43f88a1ff3460f4c43fa4&dn=Rick+and+Morty+Season+1+%5BUNCENSORED%5D+%5BBDRip%5D+%5B1080p%5D+%5BHEVC%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:8cdcb24c90c06fb1bf2c69485c76390aed50c3a5&dn=Rick+and+Morty+Season+2+Complete+720p+MKV&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:475ee9147c9c3a4c2c1f3c2a7c078d8552cf7598&dn=Rick.and.Morty.S02E10.HDTV.x264-BATV%5Bettv%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
        "http://fakepirateproxy.com/top/200",
        "http://fakepirateproxy.com/search/Rick%20And%20Morty/0/99/205,208"
      };
      Assert.AreEqual(addressesRequested, webClient.RequestsMade);
    }

    [Test]
    public void TestDifferentProxy()
    {
      List<string> responseStrings = new List<string>
      {
        Resources.PiratePageTop100WithMagnets,
        Resources.PiratePageSearch
      };

      StubWebClient webClient = new StubWebClient(responseStrings);

      PirateRequestResolver resolver = new PirateRequestResolver(new StubLogger(), webClient, 100);
      PirateRequest request = new PirateRequest
      {
        Limit = 5,
        Offset = 0,
        Quality = VideoQuality.Both,
        ExtendedAttributes = true,
        ShowName = "Rick And Morty",
        PirateProxyURL = "http://adifferentfakepirateproxy.com",
      };

      List<Torrent> torrentStrings = resolver.Resolve(request);
      List<Torrent> correctResponse = new List<Torrent>
      {
        new Torrent()
        {
          Title = "Rick and Morty Season 2 [WEBRIP] [1080p] [HEVC]",
          Link = "magnet:?xt=urn:btih:0494a80532b5b05dde567c61220d93406b7e22e7&dn=Rick+and+Morty+Season+2+%5BWEBRIP%5D+%5B1080p%5D+%5BHEVC%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:08ad112d3469f45ed490ffed8253d48aa01e702d&dn=Rick+and+Morty+Season+1+%5B1080p%5D+%5BHEVC%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2015, 7, 26),
          UploaderName = "Anonymous",
          UploaderStatus = TorrentUploaderStatus.None,
          Size = 3195130865,
          Seeds = 364,
          Leeches = 60
        },
        new Torrent()
        {
          Title = "Rick and Morty Season 1 [UNCENSORED] [BDRip] [1080p] [HEVC]",
          Link = "magnet:?xt=urn:btih:e46bca998f72411f7ec43f88a1ff3460f4c43fa4&dn=Rick+and+Morty+Season+1+%5BUNCENSORED%5D+%5BBDRip%5D+%5B1080p%5D+%5BHEVC%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:8cdcb24c90c06fb1bf2c69485c76390aed50c3a5&dn=Rick+and+Morty+Season+2+Complete+720p+MKV&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:475ee9147c9c3a4c2c1f3c2a7c078d8552cf7598&dn=Rick.and.Morty.S02E10.HDTV.x264-BATV%5Bettv%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
        "http://adifferentfakepirateproxy.com/top/200",
        "http://adifferentfakepirateproxy.com/search/Rick%20And%20Morty/0/99/205,208"
      };
      Assert.AreEqual(addressesRequested, webClient.RequestsMade);
    }

    [Test]
    public void TestShowName()
    {
      List<string> responseStrings = new List<string>
      {
        Resources.PiratePageTop100WithMagnets,
        Resources.PiratePageDifferentShowName
      };

      StubWebClient webClient = new StubWebClient(responseStrings);

      PirateRequestResolver resolver = new PirateRequestResolver(new StubLogger(), webClient, 100);
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
          Link = "magnet:?xt=urn:btih:d83c61ea0b60b641cf13f61bfbf8095113e9573d&dn=Arrow.S04E23.HDTV.x264-LOL%5Bettv%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:941a4cc66d4cb39b23a5cc9f59948eb22f72eff5&dn=Arrow.S04E15.HDTV.x264-LOL%5Bettv%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:0a3a4ac8aa690951a5d19593e8c1b83a61382baf&dn=Arrow+Season+4+Complete+x264-TJ+%5BTJET%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:c1c2a53e4bf77af86550f4b0ea64287acdda248d&dn=Arrow.S04E11.HDTV.x264-LOL%5Bettv%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:25387dadd54dade2a5ccba39b3a6e206886a00d0&dn=Arrow.S04E20.HDTV.x264-LOL%5Bettv%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
        "http://fakepirateproxy.com/top/200",
        "http://fakepirateproxy.com/search/Arrow/0/99/205,208"
      };
      Assert.AreEqual(addressesRequested, webClient.RequestsMade);
    }

    [Test]
    public void TestLimitOverPageSize()
    {
      List<string> responseStrings = new List<string>
      {
        Resources.PiratePageTop100WithMagnets,
        Resources.PiratePageSearch5Rows,
        Resources.PiratePageSearch5Rows
      };

      StubWebClient webClient = new StubWebClient(responseStrings);

      PirateRequestResolver resolver = new PirateRequestResolver(new StubLogger(), webClient, 100);
      PirateRequest request = new PirateRequest
      {
        Limit = 6,
        Offset = 0,
        Quality = VideoQuality.Both,
        ExtendedAttributes = true,
        ShowName = "Rick And Morty",
        PirateProxyURL = "http://fakepirateproxy.com",
      };

      List<Torrent> torrentStrings = resolver.Resolve(request);
      List<Torrent> correctResponse = new List<Torrent>
      {
        new Torrent()
        {
          Title = "Rick and Morty Season 2 [WEBRIP] [1080p] [HEVC]",
          Link = "magnet:?xt=urn:btih:0494a80532b5b05dde567c61220d93406b7e22e7&dn=Rick+and+Morty+Season+2+%5BWEBRIP%5D+%5B1080p%5D+%5BHEVC%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:08ad112d3469f45ed490ffed8253d48aa01e702d&dn=Rick+and+Morty+Season+1+%5B1080p%5D+%5BHEVC%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2015, 7, 26),
          UploaderName = "Anonymous",
          UploaderStatus = TorrentUploaderStatus.None,
          Size = 3195130865,
          Seeds = 364,
          Leeches = 60
        },
        new Torrent()
        {
          Title = "Rick and Morty Season 1 [UNCENSORED] [BDRip] [1080p] [HEVC]",
          Link = "magnet:?xt=urn:btih:e46bca998f72411f7ec43f88a1ff3460f4c43fa4&dn=Rick+and+Morty+Season+1+%5BUNCENSORED%5D+%5BBDRip%5D+%5B1080p%5D+%5BHEVC%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:8cdcb24c90c06fb1bf2c69485c76390aed50c3a5&dn=Rick+and+Morty+Season+2+Complete+720p+MKV&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:475ee9147c9c3a4c2c1f3c2a7c078d8552cf7598&dn=Rick.and.Morty.S02E10.HDTV.x264-BATV%5Bettv%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2015, 10, 5),
          UploaderName = "ettv",
          UploaderStatus = TorrentUploaderStatus.Vip,
          Size = 149558395,
          Seeds = 66,
          Leeches = 22
        },
        new Torrent()
        {
          Title = "Rick and Morty Season 2 [WEBRIP] [1080p] [HEVC]",
          Link = "magnet:?xt=urn:btih:0494a80532b5b05dde567c61220d93406b7e22e7&dn=Rick+and+Morty+Season+2+%5BWEBRIP%5D+%5B1080p%5D+%5BHEVC%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2015, 11, 3),
          UploaderName = ".BONE.",
          UploaderStatus = TorrentUploaderStatus.Vip,
          Size = 2394284168,
          Seeds = 590,
          Leeches = 109
        }
      };
      Assert.AreEqual(correctResponse, torrentStrings);

      List<string> addressesRequested = new List<string>
      {
        "http://fakepirateproxy.com/top/200",
        "http://fakepirateproxy.com/search/Rick%20And%20Morty/0/99/205,208",
        "http://fakepirateproxy.com/search/Rick%20And%20Morty/1/99/205,208"
      };
      Assert.AreEqual(addressesRequested, webClient.RequestsMade);
    }

    [Test]
    public void TestLimitExactlyPageSize()
    {
      List<string> responseStrings = new List<string>
      {
        Resources.PiratePageTop100WithMagnets,
        Resources.PiratePageSearch5Rows
      };

      StubWebClient webClient = new StubWebClient(responseStrings);

      PirateRequestResolver resolver = new PirateRequestResolver(new StubLogger(), webClient, 100);
      PirateRequest request = new PirateRequest
      {
        Limit = 5,
        Offset = 0,
        Quality = VideoQuality.Both,
        ExtendedAttributes = true,
        ShowName = "Rick And Morty",
        PirateProxyURL = "http://fakepirateproxy.com",
      };

      List<Torrent> torrentStrings = resolver.Resolve(request);
      List<Torrent> correctResponse = new List<Torrent>
      {
        new Torrent()
        {
          Title = "Rick and Morty Season 2 [WEBRIP] [1080p] [HEVC]",
          Link = "magnet:?xt=urn:btih:0494a80532b5b05dde567c61220d93406b7e22e7&dn=Rick+and+Morty+Season+2+%5BWEBRIP%5D+%5B1080p%5D+%5BHEVC%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:08ad112d3469f45ed490ffed8253d48aa01e702d&dn=Rick+and+Morty+Season+1+%5B1080p%5D+%5BHEVC%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2015, 7, 26),
          UploaderName = "Anonymous",
          UploaderStatus = TorrentUploaderStatus.None,
          Size = 3195130865,
          Seeds = 364,
          Leeches = 60
        },
        new Torrent()
        {
          Title = "Rick and Morty Season 1 [UNCENSORED] [BDRip] [1080p] [HEVC]",
          Link = "magnet:?xt=urn:btih:e46bca998f72411f7ec43f88a1ff3460f4c43fa4&dn=Rick+and+Morty+Season+1+%5BUNCENSORED%5D+%5BBDRip%5D+%5B1080p%5D+%5BHEVC%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:8cdcb24c90c06fb1bf2c69485c76390aed50c3a5&dn=Rick+and+Morty+Season+2+Complete+720p+MKV&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:475ee9147c9c3a4c2c1f3c2a7c078d8552cf7598&dn=Rick.and.Morty.S02E10.HDTV.x264-BATV%5Bettv%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
        "http://fakepirateproxy.com/top/200",
        "http://fakepirateproxy.com/search/Rick%20And%20Morty/0/99/205,208"
      };
      Assert.AreEqual(addressesRequested, webClient.RequestsMade);
    }

    [Test]
    public void TestNoLimit()
    {
      List<string> responseStrings = new List<string>
      {
        Resources.PiratePageTop100WithMagnets,
        Resources.PiratePageSearch5Rows,
        Resources.PiratePageNoResults
      };

      StubWebClient webClient = new StubWebClient(responseStrings);

      PirateRequestResolver resolver = new PirateRequestResolver(new StubLogger(), webClient, 100);
      PirateRequest request = new PirateRequest
      {
        Offset = 0,
        Quality = VideoQuality.Both,
        ExtendedAttributes = true,
        ShowName = "Rick And Morty",
        PirateProxyURL = "http://fakepirateproxy.com",
      };

      List<Torrent> torrentStrings = resolver.Resolve(request);
      List<Torrent> correctResponse = new List<Torrent>
      {
        new Torrent()
        {
          Title = "Rick and Morty Season 2 [WEBRIP] [1080p] [HEVC]",
          Link = "magnet:?xt=urn:btih:0494a80532b5b05dde567c61220d93406b7e22e7&dn=Rick+and+Morty+Season+2+%5BWEBRIP%5D+%5B1080p%5D+%5BHEVC%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:08ad112d3469f45ed490ffed8253d48aa01e702d&dn=Rick+and+Morty+Season+1+%5B1080p%5D+%5BHEVC%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2015, 7, 26),
          UploaderName = "Anonymous",
          UploaderStatus = TorrentUploaderStatus.None,
          Size = 3195130865,
          Seeds = 364,
          Leeches = 60
        },
        new Torrent()
        {
          Title = "Rick and Morty Season 1 [UNCENSORED] [BDRip] [1080p] [HEVC]",
          Link = "magnet:?xt=urn:btih:e46bca998f72411f7ec43f88a1ff3460f4c43fa4&dn=Rick+and+Morty+Season+1+%5BUNCENSORED%5D+%5BBDRip%5D+%5B1080p%5D+%5BHEVC%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:8cdcb24c90c06fb1bf2c69485c76390aed50c3a5&dn=Rick+and+Morty+Season+2+Complete+720p+MKV&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:475ee9147c9c3a4c2c1f3c2a7c078d8552cf7598&dn=Rick.and.Morty.S02E10.HDTV.x264-BATV%5Bettv%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
        "http://fakepirateproxy.com/top/200",
        "http://fakepirateproxy.com/search/Rick%20And%20Morty/0/99/205,208",
        "http://fakepirateproxy.com/search/Rick%20And%20Morty/1/99/205,208"
      };
      Assert.AreEqual(addressesRequested, webClient.RequestsMade);
    }

    [Test]
    public void TestOffsetGreaterThanAvailableRows()
    {
      List<string> responseStrings = new List<string>
      {
        Resources.PiratePageTop100WithMagnets,
        Resources.PiratePageSearch5Rows,
      };

      StubWebClient webClient = new StubWebClient(responseStrings);

      PirateRequestResolver resolver = new PirateRequestResolver(new StubLogger(), webClient, 100);
      PirateRequest request = new PirateRequest
      {
        Offset = 5,
        Quality = VideoQuality.Both,
        ExtendedAttributes = true,
        ShowName = "Rick And Morty",
        PirateProxyURL = "http://fakepirateproxy.com",
      };

      List<Torrent> torrentStrings = resolver.Resolve(request);
      List<Torrent> correctResponse = new List<Torrent>();
      Assert.AreEqual(correctResponse, torrentStrings);

      List<string> addressesRequested = new List<string>
      {
        "http://fakepirateproxy.com/top/200",
        "http://fakepirateproxy.com/search/Rick%20And%20Morty/0/99/205,208",
      };
      Assert.AreEqual(addressesRequested, webClient.RequestsMade);
    }


    [Test]
    public void TestOffsetLessThan30()
    {
      List<string> responseStrings = new List<string>
      {
        Resources.PiratePageTop100WithMagnets,
        Resources.PiratePageSearch
      };

      StubWebClient webClient = new StubWebClient(responseStrings);

      PirateRequestResolver resolver = new PirateRequestResolver(new StubLogger(), webClient, 100);
      PirateRequest request = new PirateRequest
      {
        Limit = 5,
        Offset = 15,
        Quality = VideoQuality.Both,
        ExtendedAttributes = true,
        ShowName = "Rick And Morty",
        PirateProxyURL = "http://fakepirateproxy.com",
      };

      List<Torrent> torrentStrings = resolver.Resolve(request);
      List<Torrent> correctResponse = new List<Torrent>
      {
        new Torrent()
        {
          Title = "Rick and Morty S01E07 HDTV x264-2HD [eztv]",
          Link = "magnet:?xt=urn:btih:607be29a58b962d9baa3b5dbd2477e2fccc2c66e&dn=Rick+and+Morty+S01E07+HDTV+x264-2HD+%5Beztv%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:f9e844d430fd38e36ece24938c3d613f875af3eb&dn=Rick.and.Morty.S02E01.HDTV.x264-BATV%5Bettv%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:39749ebdafc47a3287c52e04f6da0c6172a43143&dn=Rick.and.Morty.S02E08.720p.HDTV.x264-BATV%5BEtHD%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:61c34ea7b6e51c69b07ee0df34f2acccc3926e93&dn=Rick+And+Morty+S01E10+HDTV+x264-MiNDTHEGAP+%5Beztv%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:a5a508354eb847f681b84a28f9ed2e265ae65686&dn=Rick+and+Morty+S01E09+PROPER+HDTV+x264-2HD+%5Beztv%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
        "http://fakepirateproxy.com/top/200",
        "http://fakepirateproxy.com/search/Rick%20And%20Morty/0/99/205,208"
      };
      Assert.AreEqual(addressesRequested, webClient.RequestsMade);
    }

    [Test]
    public void TestOffsetAndLimitOver2Pages()
    {
      string responseString = Resources.PiratePageSearch5Rows;
      List<string> responseStrings = new List<string>
      {
        Resources.PiratePageTop100WithMagnets,
        Resources.PiratePageSearch5Rows,
        Resources.PiratePageSearch5Rows
      };

      StubWebClient webClient = new StubWebClient(responseStrings);

      PirateRequestResolver resolver = new PirateRequestResolver(new StubLogger(), webClient, 100);
      PirateRequest request = new PirateRequest
      {
        Limit = 6,
        Offset = 4,
        Quality = VideoQuality.Both,
        ExtendedAttributes = true,
        ShowName = "Rick And Morty",
        PirateProxyURL = "http://fakepirateproxy.com",
      };

      List<Torrent> torrentStrings = resolver.Resolve(request);
      List<Torrent> correctResponse = new List<Torrent>
      {
        new Torrent()
        {
          Title = "Rick.and.Morty.S02E10.HDTV.x264-BATV[ettv]",
          Link = "magnet:?xt=urn:btih:475ee9147c9c3a4c2c1f3c2a7c078d8552cf7598&dn=Rick.and.Morty.S02E10.HDTV.x264-BATV%5Bettv%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2015, 10, 5),
          UploaderName = "ettv",
          UploaderStatus = TorrentUploaderStatus.Vip,
          Size = 149558395,
          Seeds = 66,
          Leeches = 22
        },
        new Torrent()
        {
          Title = "Rick and Morty Season 2 [WEBRIP] [1080p] [HEVC]",
          Link = "magnet:?xt=urn:btih:0494a80532b5b05dde567c61220d93406b7e22e7&dn=Rick+and+Morty+Season+2+%5BWEBRIP%5D+%5B1080p%5D+%5BHEVC%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:08ad112d3469f45ed490ffed8253d48aa01e702d&dn=Rick+and+Morty+Season+1+%5B1080p%5D+%5BHEVC%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2015, 7, 26),
          UploaderName = "Anonymous",
          UploaderStatus = TorrentUploaderStatus.None,
          Size = 3195130865,
          Seeds = 364,
          Leeches = 60
        },
        new Torrent()
        {
          Title = "Rick and Morty Season 1 [UNCENSORED] [BDRip] [1080p] [HEVC]",
          Link = "magnet:?xt=urn:btih:e46bca998f72411f7ec43f88a1ff3460f4c43fa4&dn=Rick+and+Morty+Season+1+%5BUNCENSORED%5D+%5BBDRip%5D+%5B1080p%5D+%5BHEVC%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:8cdcb24c90c06fb1bf2c69485c76390aed50c3a5&dn=Rick+and+Morty+Season+2+Complete+720p+MKV&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:475ee9147c9c3a4c2c1f3c2a7c078d8552cf7598&dn=Rick.and.Morty.S02E10.HDTV.x264-BATV%5Bettv%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
        "http://fakepirateproxy.com/top/200",
        "http://fakepirateproxy.com/search/Rick%20And%20Morty/0/99/205,208",
        "http://fakepirateproxy.com/search/Rick%20And%20Morty/1/99/205,208"
      };
      Assert.AreEqual(addressesRequested, webClient.RequestsMade);
    }


    [Test]
    public void TestOffsetExactly30()
    {
      List<string> responseStrings = new List<string>
      {
        Resources.PiratePageTop100WithMagnets,
        Resources.PiratePageSearch
      };

      StubWebClient webClient = new StubWebClient(responseStrings);

      PirateRequestResolver resolver = new PirateRequestResolver(new StubLogger(), webClient, 100);
      PirateRequest request = new PirateRequest
      {
        Limit = 5,
        Offset = 30,
        Quality = VideoQuality.Both,
        ExtendedAttributes = true,
        ShowName = "Rick And Morty",
        PirateProxyURL = "http://fakepirateproxy.com",
      };

      List<Torrent> torrentStrings = resolver.Resolve(request);
      List<Torrent> correctResponse = new List<Torrent>
        {
          new Torrent()
          {
            Title = "Rick and Morty Season 2 [WEBRIP] [1080p] [HEVC]",
            Link =
              "magnet:?xt=urn:btih:0494a80532b5b05dde567c61220d93406b7e22e7&dn=Rick+and+Morty+Season+2+%5BWEBRIP%5D+%5B1080p%5D+%5BHEVC%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
              "magnet:?xt=urn:btih:08ad112d3469f45ed490ffed8253d48aa01e702d&dn=Rick+and+Morty+Season+1+%5B1080p%5D+%5BHEVC%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
            PublishDate = new DateTime(2015, 7, 26),
            UploaderName = "Anonymous",
            UploaderStatus = TorrentUploaderStatus.None,
            Size = 3195130865,
            Seeds = 364,
            Leeches = 60
          },
          new Torrent()
          {
            Title = "Rick and Morty Season 1 [UNCENSORED] [BDRip] [1080p] [HEVC]",
            Link =
              "magnet:?xt=urn:btih:e46bca998f72411f7ec43f88a1ff3460f4c43fa4&dn=Rick+and+Morty+Season+1+%5BUNCENSORED%5D+%5BBDRip%5D+%5B1080p%5D+%5BHEVC%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
              "magnet:?xt=urn:btih:8cdcb24c90c06fb1bf2c69485c76390aed50c3a5&dn=Rick+and+Morty+Season+2+Complete+720p+MKV&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
              "magnet:?xt=urn:btih:475ee9147c9c3a4c2c1f3c2a7c078d8552cf7598&dn=Rick.and.Morty.S02E10.HDTV.x264-BATV%5Bettv%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
        "http://fakepirateproxy.com/top/200",
        "http://fakepirateproxy.com/search/Rick%20And%20Morty/1/99/205,208"
      };
      Assert.AreEqual(addressesRequested, webClient.RequestsMade);
    }

    [Test]
    public void TestOffsetOver30()
    {
      List<string> responseStrings = new List<string>
      {
        Resources.PiratePageTop100WithMagnets,
        Resources.PiratePageSearch
      };

      StubWebClient webClient = new StubWebClient(responseStrings);

      PirateRequestResolver resolver = new PirateRequestResolver(new StubLogger(), webClient, 100);
      PirateRequest request = new PirateRequest
      {
        Limit = 5,
        Offset = 45,
        Quality = VideoQuality.Both,
        ExtendedAttributes = true,
        ShowName = "Rick And Morty",
        PirateProxyURL = "http://fakepirateproxy.com",
      };

      List<Torrent> torrentStrings = resolver.Resolve(request);
      List<Torrent> correctResponse = new List<Torrent>
      {
        new Torrent()
        {
          Title = "Rick and Morty S01E07 HDTV x264-2HD [eztv]",
          Link = "magnet:?xt=urn:btih:607be29a58b962d9baa3b5dbd2477e2fccc2c66e&dn=Rick+and+Morty+S01E07+HDTV+x264-2HD+%5Beztv%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:f9e844d430fd38e36ece24938c3d613f875af3eb&dn=Rick.and.Morty.S02E01.HDTV.x264-BATV%5Bettv%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:39749ebdafc47a3287c52e04f6da0c6172a43143&dn=Rick.and.Morty.S02E08.720p.HDTV.x264-BATV%5BEtHD%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:61c34ea7b6e51c69b07ee0df34f2acccc3926e93&dn=Rick+And+Morty+S01E10+HDTV+x264-MiNDTHEGAP+%5Beztv%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:a5a508354eb847f681b84a28f9ed2e265ae65686&dn=Rick+and+Morty+S01E09+PROPER+HDTV+x264-2HD+%5Beztv%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
        "http://fakepirateproxy.com/top/200",
        "http://fakepirateproxy.com/search/Rick%20And%20Morty/1/99/205,208"
      };
      Assert.AreEqual(addressesRequested, webClient.RequestsMade);
    }

    [Test]
    public void TestVideoQualityHd()
    {
      List<string> responseStrings = new List<string>
      {
        Resources.PiratePageTop100WithMagnets,
        Resources.PiratePageHDOnly
      };

      StubWebClient webClient = new StubWebClient(responseStrings);

      PirateRequestResolver resolver = new PirateRequestResolver(new StubLogger(), webClient, 100);
      PirateRequest request = new PirateRequest
      {
        Limit = 5,
        Offset = 0,
        Quality = VideoQuality.HD,
        ExtendedAttributes = true,
        ShowName = "Rick And Morty",
        PirateProxyURL = "http://fakepirateproxy.com",
      };

      List<Torrent> torrentStrings = resolver.Resolve(request);
      List<Torrent> correctResponse = new List<Torrent>
      {
        new Torrent()
        {
          Title = "Rick and Morty Season 2 [WEBRIP] [1080p] [HEVC]",
          Link = "magnet:?xt=urn:btih:0494a80532b5b05dde567c61220d93406b7e22e7&dn=Rick+and+Morty+Season+2+%5BWEBRIP%5D+%5B1080p%5D+%5BHEVC%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2015, 11, 3),
          UploaderName = ".BONE.",
          UploaderStatus = TorrentUploaderStatus.Vip,
          Size = 2394284168,
          Seeds = 573,
          Leeches = 112
        },
        new Torrent()
        {
          Title = "Rick and Morty Season 1 [1080p] [HEVC]",
          Link = "magnet:?xt=urn:btih:08ad112d3469f45ed490ffed8253d48aa01e702d&dn=Rick+and+Morty+Season+1+%5B1080p%5D+%5BHEVC%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2015, 7, 26),
          UploaderName = "Anonymous",
          UploaderStatus = TorrentUploaderStatus.None,
          Size = 3195130865,
          Seeds = 341,
          Leeches = 75
        },
        new Torrent()
        {
          Title = "Rick and Morty Season 1 [UNCENSORED] [BDRip] [1080p] [HEVC]",
          Link = "magnet:?xt=urn:btih:e46bca998f72411f7ec43f88a1ff3460f4c43fa4&dn=Rick+and+Morty+Season+1+%5BUNCENSORED%5D+%5BBDRip%5D+%5B1080p%5D+%5BHEVC%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:668c251eab6a3155fbe7a7ef52bd062787c49320&dn=Rick+and+Morty+Season+2%5BBDRip+1080p+AC3%5D%5BAtaraxiaPrime%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:827e404e8dc1e32542b23a41036b6e64b6bc2d66&dn=Rick+and+Morty+Season+1+%281280x720%29+%5BPhr0stY&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
        "http://fakepirateproxy.com/top/200",
        "http://fakepirateproxy.com/search/Rick%20And%20Morty/0/99/208"
      };
      Assert.AreEqual(addressesRequested, webClient.RequestsMade);
    }

    [Test]
    public void TestVideoQualitySd()
    {
      List<string> responseStrings = new List<string>
      {
        Resources.PiratePageTop100WithMagnets,
        Resources.PiratePageSDOnly
      };

      StubWebClient webClient = new StubWebClient(responseStrings);

      PirateRequestResolver resolver = new PirateRequestResolver(new StubLogger(), webClient, 100);
      PirateRequest request = new PirateRequest
      {
        Limit = 5,
        Offset = 0,
        Quality = VideoQuality.SD,
        ExtendedAttributes = true,
        ShowName = "Rick And Morty",
        PirateProxyURL = "http://fakepirateproxy.com",
      };

      List<Torrent> torrentStrings = resolver.Resolve(request);
      List<Torrent> correctResponse = new List<Torrent>
      {
        new Torrent()
        {
          Title = "Rick and Morty Season 2 Complete 720p MKV",
          Link = "magnet:?xt=urn:btih:8cdcb24c90c06fb1bf2c69485c76390aed50c3a5&dn=Rick+and+Morty+Season+2+Complete+720p+MKV&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:55dad47c3a28b1c2f482206a47dff151fac45aa4&dn=Rick+and+Morty+S01E02+HDTV+x264-KILLERS+%5Beztv%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:475ee9147c9c3a4c2c1f3c2a7c078d8552cf7598&dn=Rick.and.Morty.S02E10.HDTV.x264-BATV%5Bettv%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2015, 10, 5),
          UploaderName = "ettv",
          UploaderStatus = TorrentUploaderStatus.Vip,
          Size = 149558451,
          Seeds = 70,
          Leeches = 19
        },
        new Torrent()
        {
          Title = "Rick and Morty S01E01 HDTV x264-2HD [eztv]",
          Link = "magnet:?xt=urn:btih:d335feab9035201c28d5cd59e406d3031374d3c6&dn=Rick+and+Morty+S01E01+HDTV+x264-2HD+%5Beztv%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:2b3a3ca8b82d6c8b339159be8a15a361236b8910&dn=Rick+and+Morty+S01E03+HDTV+x264-KILLERS+%5Beztv%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
        "http://fakepirateproxy.com/top/200",
        "http://fakepirateproxy.com/search/Rick%20And%20Morty/0/99/205"
      };
      Assert.AreEqual(addressesRequested, webClient.RequestsMade);
    }

    [Test]
    public void TestSingleSeason()
    {
      List<string> responseStrings = new List<string>
      {
        Resources.PiratePageTop100WithMagnets,
        Resources.PiratePageSingleSeason,
        Resources.PiratePageNoResults
      };

      StubWebClient webClient = new StubWebClient(responseStrings);

      PirateRequestResolver resolver = new PirateRequestResolver(new StubLogger(), webClient, 100);
      PirateRequest request = new PirateRequest
      {
        Offset = 0,
        Quality = VideoQuality.Both,
        ExtendedAttributes = true,
        ShowName = "Rick And Morty",
        PirateProxyURL = "http://fakepirateproxy.com",
        Season = 2
      };

      List<Torrent> torrentStrings = resolver.Resolve(request);
      List<Torrent> correctResponse = new List<Torrent>
      {
        new Torrent()
        {
          Title = "Rick and Morty Season 2 [WEBRIP] [1080p] [HEVC]",
          Link = "magnet:?xt=urn:btih:0494a80532b5b05dde567c61220d93406b7e22e7&dn=Rick+and+Morty+Season+2+%5BWEBRIP%5D+%5B1080p%5D+%5BHEVC%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:8cdcb24c90c06fb1bf2c69485c76390aed50c3a5&dn=Rick+and+Morty+Season+2+Complete+720p+MKV&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:668c251eab6a3155fbe7a7ef52bd062787c49320&dn=Rick+and+Morty+Season+2%5BBDRip+1080p+AC3%5D%5BAtaraxiaPrime%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:d64161416fe4cba97131237d810dfc77f6640d14&dn=Rick.and.Morty.Season.2.1080p.BluRay.x264.with.commentary.tracks&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2016, 6, 28, 6, 3, 0),
          UploaderName = "fauxcon",
          UploaderStatus = TorrentUploaderStatus.None,
          Size = 9334060183,
          Seeds = 34,
          Leeches = 3
        }
      };
      Assert.AreEqual(correctResponse, torrentStrings);

      List<string> addressesRequested = new List<string>
      {
        "http://fakepirateproxy.com/top/200",
        "http://fakepirateproxy.com/search/Rick%20And%20Morty%20Season%202/0/99/205,208",
        "http://fakepirateproxy.com/search/Rick%20And%20Morty%20Season%202/1/99/205,208"
      };
      Assert.AreEqual(addressesRequested, webClient.RequestsMade);
    }

    [Test]
    public void TestSingleEpisodeNoSeason()
    {
      PirateRequestResolver resolver = new PirateRequestResolver(new StubLogger(), null, 100);
      PirateRequest request = new PirateRequest
      {
        Offset = 0,
        Quality = VideoQuality.Both,
        ExtendedAttributes = true,
        ShowName = "Rick And Morty",
        PirateProxyURL = "http://fakepirateproxy.com",
        Episode = 2
      };


      Assert.Throws<ArgumentException>(() => resolver.Resolve(request));
    }

    [Test]
    public void TestSingleEpisodeAndSeason()
    {
      List<string> responseStrings = new List<string>
      {
        Resources.PiratePageTop100WithMagnets,
        Resources.PiratePageSingleEpisode,
      };

      StubWebClient webClient = new StubWebClient(responseStrings);

      PirateRequestResolver resolver = new PirateRequestResolver(new StubLogger(), webClient, 100);
      PirateRequest request = new PirateRequest
      {
        Offset = 0,
        Limit = 5,
        Quality = VideoQuality.Both,
        ExtendedAttributes = true,
        ShowName = "Rick And Morty",
        PirateProxyURL = "http://fakepirateproxy.com",
        Season = 2,
        Episode = 1
      };

      List<Torrent> torrentStrings = resolver.Resolve(request);
      List<Torrent> correctResponse = new List<Torrent>
      {
        new Torrent()
        {
          Title = "Rick.and.Morty.S02E01.HDTV.x264-BATV[ettv]",
          Link = "magnet:?xt=urn:btih:f9e844d430fd38e36ece24938c3d613f875af3eb&dn=Rick.and.Morty.S02E01.HDTV.x264-BATV%5Bettv%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2015, 7, 27),
          UploaderName = "ettv",
          UploaderStatus = TorrentUploaderStatus.Vip,
          Size = 217987493,
          Seeds = 25,
          Leeches = 7
        },
        new Torrent()
        {
          Title = "Rick.and.Morty.S02E01.720p.HDTV.x264-BATV[EtHD]",
          Link = "magnet:?xt=urn:btih:557e9645fe4978a13344897ecef352fca9acc251&dn=Rick.and.Morty.S02E01.720p.HDTV.x264-BATV%5BEtHD%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2015, 7, 27),
          UploaderName = "ettv",
          UploaderStatus = TorrentUploaderStatus.Vip,
          Size = 749584621,
          Seeds = 17,
          Leeches = 3
        },
        new Torrent()
        {
          Title = "Rick_and_Morty_S02E01",
          Link = "magnet:?xt=urn:btih:1ffb54f72363facedd9c7c3a72470a33fcf32946&dn=Rick_and_Morty_S02E01&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2015, 7, 2),
          UploaderName = "BeefcakeChan",
          UploaderStatus = TorrentUploaderStatus.None,
          Size = 145824972,
          Seeds = 1,
          Leeches = 0
        },
        new Torrent()
        {
          Title = "Rick and Morty S02E01-E02 (Leaked) (600x300)",
          Link = "magnet:?xt=urn:btih:0649e51d1dce5d418518325f7cb973034abc1cef&dn=Rick+and+Morty+S02E01-E02+%28Leaked%29+%28600x300%29&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2015, 7, 2),
          UploaderName = "frostyon420",
          UploaderStatus = TorrentUploaderStatus.Trusted,
          Size = 370383604,
          Seeds = 1,
          Leeches = 0
        },
        new Torrent()
        {
          Title = "Rick.and.Morty.S02E01.1080p.WEB-DL.DD5.1.H.264-RARBG",
          Link = "magnet:?xt=urn:btih:74b6ac581cd54687c8fd1da25f89ab01fe098d05&dn=Rick.and.Morty.S02E01.1080p.WEB-DL.DD5.1.H.264-RARBG&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2015, 7, 27),
          UploaderName = "frostyon420",
          UploaderStatus = TorrentUploaderStatus.Trusted,
          Size = 925810288,
          Seeds = 1,
          Leeches = 0
        },
      };
      Assert.AreEqual(correctResponse, torrentStrings);

      List<string> addressesRequested = new List<string>
      {
        "http://fakepirateproxy.com/top/200",
        "http://fakepirateproxy.com/search/Rick%20And%20Morty%20S02E01/0/99/205,208",
      };
      Assert.AreEqual(addressesRequested, webClient.RequestsMade);
    }

    [Test]
    public void TestMaxAge()
    {
      List<string> responseStrings = new List<string>
      {
        Resources.PiratePageTop100WithMagnets,
        Resources.PiratePageSearch,
        Resources.PiratePageNoResults
      };

      StubWebClient webClient = new StubWebClient(responseStrings);

      PirateRequestResolver resolver = new PirateRequestResolver(new StubLogger(), webClient, 100, new DateTime(2016, 9, 6));
      PirateRequest request = new PirateRequest
      {
        Limit = 5,
        Offset = 0,
        Quality = VideoQuality.Both,
        ExtendedAttributes = true,
        ShowName = "Rick And Morty",
        PirateProxyURL = "http://fakepirateproxy.com",
        MaxAge = 250
      };

      List<Torrent> torrentStrings = resolver.Resolve(request);
      List<Torrent> correctResponse = new List<Torrent>
      {
        new Torrent()
        {
          Title = "Rick.and.Morty.Season.2.1080p.BluRay.x264.with.commentary.tracks",
          Link = "magnet:?xt=urn:btih:d64161416fe4cba97131237d810dfc77f6640d14&dn=Rick.and.Morty.Season.2.1080p.BluRay.x264.with.commentary.tracks&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2016, 6, 28, 6, 3, 0),
          UploaderName = "fauxcon",
          UploaderStatus = TorrentUploaderStatus.None,
          Size = 9334060183,
          Seeds = 39,
          Leeches = 8
        },
        new Torrent()
        {
          Title = "Rick and Morty Season 2[BDRip 1080p AC3][AtaraxiaPrime]",
          Link = "magnet:?xt=urn:btih:668c251eab6a3155fbe7a7ef52bd062787c49320&dn=Rick+and+Morty+Season+2%5BBDRip+1080p+AC3%5D%5BAtaraxiaPrime%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2016, 7, 2, 3, 14, 0),
          UploaderName = "AtaraxiaPrime",
          UploaderStatus = TorrentUploaderStatus.None,
          Size = 4955735608,
          Seeds = 22,
          Leeches = 5
        },
        new Torrent()
        {
          Title = "Rick and Morty Season 1[BDRip 1080p AC3][AtaraxiaPrime]",
          Link = "magnet:?xt=urn:btih:687a7cda14d35bf06a67964b6bda6d02328429e9&dn=Rick+and+Morty+Season+1%5BBDRip+1080p+AC3%5D%5BAtaraxiaPrime%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2016, 7, 2, 3, 12, 0),
          UploaderName = "AtaraxiaPrime",
          UploaderStatus = TorrentUploaderStatus.None,
          Size = 4830388434,
          Seeds = 13,
          Leeches = 2
        },
      };
      Assert.AreEqual(correctResponse, torrentStrings);

      List<string> addressesRequested = new List<string>
      {
        "http://fakepirateproxy.com/top/200",
        "http://fakepirateproxy.com/search/Rick%20And%20Morty/0/99/205,208",
        "http://fakepirateproxy.com/search/Rick%20And%20Morty/1/99/205,208"
      };
      Assert.AreEqual(addressesRequested, webClient.RequestsMade);
    }

    [Test]
    public void TestUnresponsiveProxy()
    {
      List<string> responseStrings = new List<string>
      {
        null
      };

      StubWebClient webClient = new StubWebClient(responseStrings);

      PirateRequestResolver resolver = new PirateRequestResolver(new StubLogger(), webClient, 100);

      PirateRequest request = new PirateRequest
      {
        Limit = 5,
        Offset = 0,
        Quality = VideoQuality.Both,
        ExtendedAttributes = true,
        ShowName = "Rick And Morty",
        PirateProxyURL = "http://fakepirateproxy.com",
      };

      List<Torrent> torrents = resolver.Resolve(request);

      Assert.IsNull(torrents);
    }

    [Test]
    public void TestNoMagnetsInSearchProxy()
    {
      List<string> responses = new List<string>()
      {
        Resources.PiratePageTop100NoMagnets,
        Resources.PiratePageSearch3RowsNoMagnets,
        Resources.RickAndMortySeason2,
        Resources.RickAndMortySeason2,
        Resources.RickAndMortySeason2,
        Resources.PiratePageNoResults
      };

      StubWebClient webClient = new StubWebClient(responses);

      PirateRequestResolver resolver = new PirateRequestResolver(new StubLogger(), webClient, 100, new DateTime(2016, 10, 31));
      PirateRequest request = new PirateRequest
      {
        Limit = 5,
        Offset = 0,
        Quality = VideoQuality.Both,
        ExtendedAttributes = true,
        ShowName = "Rick And Morty",
        PirateProxyURL = "http://fakepirateproxy.com",
      };

      List<Torrent> torrentStrings = resolver.Resolve(request);
      List<Torrent> correctResponse = new List<Torrent>
      {
        new Torrent()
        {
          Title = "Rick and Morty Season 2 [WEBRIP] [1080p] [HEVC]",
          Link = "magnet:?xt=urn:btih:0494a80532b5b05dde567c61220d93406b7e22e7&dn=Rick+and+Morty+Season+2+%5BWEBRIP%5D+%5B1080p%5D+%5BHEVC%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
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
          Link = "magnet:?xt=urn:btih:0494a80532b5b05dde567c61220d93406b7e22e7&dn=Rick+and+Morty+Season+2+%5BWEBRIP%5D+%5B1080p%5D+%5BHEVC%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2015, 7, 26),
          UploaderName = "Anonymous",
          UploaderStatus = TorrentUploaderStatus.None,
          Size = 3195130865,
          Seeds = 364,
          Leeches = 60
        },
        new Torrent()
        {
          Title = "Rick and Morty Season 1 and 2[UNCENSORED] [BDRip] [1080p] [HEVC]",
          Link = "magnet:?xt=urn:btih:0494a80532b5b05dde567c61220d93406b7e22e7&dn=Rick+and+Morty+Season+2+%5BWEBRIP%5D+%5B1080p%5D+%5BHEVC%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
          PublishDate = new DateTime(2015, 11, 03),
          UploaderName = ".BONE.",
          UploaderStatus = TorrentUploaderStatus.Vip,
          Size = 3446711255,
          Seeds = 196,
          Leeches = 67
        }
      };

      Assert.AreEqual(correctResponse, torrentStrings);

      List<string> addressesRequested = new List<string>
      {
        "http://fakepirateproxy.com/top/200",
        "http://fakepirateproxy.com/search/Rick%20And%20Morty/0/99/205,208",
        "http://fakepirateproxy.com/torrent/rickandmortyseason2",
        "http://fakepirateproxy.com/torrent/rickandmortyseason1",
        "http://fakepirateproxy.com/torrent/rickandmortyseason1and2",
        "http://fakepirateproxy.com/search/Rick%20And%20Morty/1/99/205,208"
      };
      Assert.AreEqual(addressesRequested, webClient.RequestsMade);

    }
  }
}
