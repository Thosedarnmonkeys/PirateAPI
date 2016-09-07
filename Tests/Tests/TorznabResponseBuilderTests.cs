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
  public class TorznabResponseBuilderTests
  {
    [Test]
    public void TestSingleItem()
    {
      Torrent torrent = new Torrent()
      {
        Title = "Show Name S01E01 SD",
        Link = "MagnetLink",
        PublishDate = new DateTime(2016, 07, 09),
        UploaderName = "Uploader",
        UploaderStatus = TorrentUploaderStatus.None,
        Size = 1024,
        Seeds = 1,
        Leeches = 1
      };

      List<Torrent> torrents = new List<Torrent> { torrent };

      TorznabResponseBuilder builder = new TorznabResponseBuilder(new StubLogger());

      string response = builder.BuildResponse(torrents);
      string correctResponse = Resources.TorznabResponseSingleItem;

      Assert.AreEqual(correctResponse, response);
    }
  }
}
