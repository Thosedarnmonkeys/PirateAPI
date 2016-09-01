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
    public void TestHandleBasicRequest()
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
        }
      };
      Assert.AreEqual(correctResponse, torrentStrings);

      List<string> addressesRequested = new List<string>
      {
        "http://fakepirateproxy.com/search/Rick%20And%20Morty/0/99/205,208"
      };
      Assert.AreEqual(addressesRequested, webClient.RequestsMade);
    }
  }
}
