using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PirateAPI.Parsers.Torrents;
using PirateAPI.ResponseBuilders.Torznab;
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
        Link = "fakeUrl",
        PublishDate = new DateTime(2016, 09, 07),
        UploaderName = "Uploader",
        UploaderStatus = TorrentUploaderStatus.None,
        Size = 1024,
        Seeds = 1,
        Leeches = 1,
      };

      List<Torrent> torrents = new List<Torrent> { torrent };

      TorznabResponseBuilder builder = new TorznabResponseBuilder(new StubLogger());

      string response = builder.BuildResponse(torrents);
      string correctResponse = Resources.TorznabResponseSingleItem;
      correctResponse = correctResponse.Replace("\r", "");

      Assert.AreEqual(correctResponse, response);
    }

    [Test]
    public void Test5Items()
    {
      List<Torrent> torrents = new List<Torrent>
      {
        new Torrent()
        {
          Title = "Show Name S01E01 SD",
          Link = "fakeUrl",
          PublishDate = new DateTime(2016, 09, 07),
          UploaderName = "Uploader",
          UploaderStatus = TorrentUploaderStatus.None,
          Size = 1024,
          Seeds = 1,
          Leeches = 1,
        },
        new Torrent()
        {
          Title = "Show Name S01E01 SD",
          Link = "fakeUrl",
          PublishDate = new DateTime(2016, 09, 07),
          UploaderName = "Uploader",
          UploaderStatus = TorrentUploaderStatus.None,
          Size = 1024,
          Seeds = 1,
          Leeches = 1,
        },
        new Torrent()
        {
          Title = "Show Name S01E01 SD",
          Link = "fakeUrl",
          PublishDate = new DateTime(2016, 09, 07),
          UploaderName = "Uploader",
          UploaderStatus = TorrentUploaderStatus.None,
          Size = 1024,
          Seeds = 1,
          Leeches = 1,
        },
        new Torrent()
        {
          Title = "Show Name S01E01 SD",
          Link = "fakeUrl",
          PublishDate = new DateTime(2016, 09, 07),
          UploaderName = "Uploader",
          UploaderStatus = TorrentUploaderStatus.None,
          Size = 1024,
          Seeds = 1,
          Leeches = 1,
        },
        new Torrent()
        {
          Title = "Show Name S01E01 SD",
          Link = "fakeUrl",
          PublishDate = new DateTime(2016, 09, 07),
          UploaderName = "Uploader",
          UploaderStatus = TorrentUploaderStatus.None,
          Size = 1024,
          Seeds = 1,
          Leeches = 1,
        }
      };

      TorznabResponseBuilder builder = new TorznabResponseBuilder(new StubLogger());

      string response = builder.BuildResponse(torrents);
      string correctResponse = Resources.TorznabResponse5Items;
      correctResponse = correctResponse.Replace("\r", "");

      Assert.AreEqual(correctResponse, response);
    }


    [Test]
    public void TestParsesSeeds()
    {
      Torrent torrent = new Torrent()
      {
        Title = "Show Name S01E01 SD",
        Link = "fakeUrl",
        PublishDate = new DateTime(2016, 09, 07),
        UploaderName = "Uploader",
        UploaderStatus = TorrentUploaderStatus.None,
        Size = 1024,
        Seeds = 99,
        Leeches = 1,
      };

      List<Torrent> torrents = new List<Torrent> { torrent };

      TorznabResponseBuilder builder = new TorznabResponseBuilder(new StubLogger());

      string response = builder.BuildResponse(torrents);
      string correctResponse = Resources.TorznabResponseSingleItem;
      correctResponse = correctResponse.Replace("name=\"seeders\" value=\"1\"", "name=\"seeders\" value=\"99\"");
      correctResponse = correctResponse.Replace("name=\"peers\" value=\"2\"", "name=\"peers\" value=\"100\"");
      correctResponse = correctResponse.Replace("\r", "");

      Assert.AreEqual(correctResponse, response);
    }

    [Test]
    public void TestParsesLeeches()
    {
      Torrent torrent = new Torrent()
      {
        Title = "Show Name S01E01 SD",
        Link = "fakeUrl",
        PublishDate = new DateTime(2016, 09, 07),
        UploaderName = "Uploader",
        UploaderStatus = TorrentUploaderStatus.None,
        Size = 1024,
        Seeds = 1,
        Leeches = 100,
      };

      List<Torrent> torrents = new List<Torrent> { torrent };

      TorznabResponseBuilder builder = new TorznabResponseBuilder(new StubLogger());

      string response = builder.BuildResponse(torrents);
      string correctResponse = Resources.TorznabResponseSingleItem;
      correctResponse = correctResponse.Replace("name=\"peers\" value=\"2\"", "name=\"peers\" value=\"101\"");
      correctResponse = correctResponse.Replace("\r", "");

      Assert.AreEqual(correctResponse, response);
    }

    [Test]
    public void TestParsesSize()
    {
      Torrent torrent = new Torrent()
      {
        Title = "Show Name S01E01 SD",
        Link = "fakeUrl",
        PublishDate = new DateTime(2016, 09, 07),
        UploaderName = "Uploader",
        UploaderStatus = TorrentUploaderStatus.None,
        Size = 2048,
        Seeds = 1,
        Leeches = 1,
      };

      List<Torrent> torrents = new List<Torrent> { torrent };

      TorznabResponseBuilder builder = new TorznabResponseBuilder(new StubLogger());

      string response = builder.BuildResponse(torrents);
      string correctResponse = Resources.TorznabResponseSingleItem;
      correctResponse = correctResponse.Replace("<size>1024</size>", "<size>2048</size>");
      correctResponse = correctResponse.Replace("length=\"1024\"", "length=\"2048\"");
      correctResponse = correctResponse.Replace("\r", "");

      Assert.AreEqual(correctResponse, response);
    }

    [Test]
    public void TestParsesShowName()
    {
      Torrent torrent = new Torrent()
      {
        Title = "Show Name S01E02 HD",
        Link = "fakeUrl",
        PublishDate = new DateTime(2016, 09, 07),
        UploaderName = "Uploader",
        UploaderStatus = TorrentUploaderStatus.None,
        Size = 1024,
        Seeds = 1,
        Leeches = 1,
      };

      List<Torrent> torrents = new List<Torrent> { torrent };

      TorznabResponseBuilder builder = new TorznabResponseBuilder(new StubLogger());

      string response = builder.BuildResponse(torrents);
      string correctResponse = Resources.TorznabResponseSingleItem;
      correctResponse = correctResponse.Replace("<title>Show Name S01E01 SD</title>", "<title>Show Name S01E02 HD</title>");
      correctResponse = correctResponse.Replace("\r", "");

      Assert.AreEqual(correctResponse, response);
    }

    [Test]
    public void TestParsesLink()
    {
      Torrent torrent = new Torrent()
      {
        Title = "Show Name S01E01 SD",
        Link = "magnet:?xt=urn:btih:0494a80532b5b05dde567c61220d93406b7e22e7&dn=Rick+and+Morty+Season+2+%5BWEBRIP%5D+%5B1080p%5D+%5BHEVC%5D&tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&tr=udp%3A%2F%2Fzer0day.ch%3A1337&tr=udp%3A%2F%2Fopen.demonii.com%3A1337&tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&tr=udp%3A%2F%2Fexodus.desync.com%3A6969",
        PublishDate = new DateTime(2016, 09, 07),
        UploaderName = "Uploader",
        UploaderStatus = TorrentUploaderStatus.None,
        Size = 1024,
        Seeds = 1,
        Leeches = 1,
      };

      List<Torrent> torrents = new List<Torrent> { torrent };

      TorznabResponseBuilder builder = new TorznabResponseBuilder(new StubLogger());

      string response = builder.BuildResponse(torrents);
      string correctResponse = Resources.TorznabResponseSingleItem;
      correctResponse = correctResponse.Replace("fakeUrl", "magnet:?xt=urn:btih:0494a80532b5b05dde567c61220d93406b7e22e7&amp;dn=Rick+and+Morty+Season+2+%5BWEBRIP%5D+%5B1080p%5D+%5BHEVC%5D&amp;tr=udp%3A%2F%2Ftracker.leechers-paradise.org%3A6969&amp;tr=udp%3A%2F%2Fzer0day.ch%3A1337&amp;tr=udp%3A%2F%2Fopen.demonii.com%3A1337&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969");
      correctResponse = correctResponse.Replace("\r", "");

      Assert.AreEqual(correctResponse, response);
    }

    [Test]
    public void TestParsesDate()
    {
      Torrent torrent = new Torrent()
      {
        Title = "Show Name S01E01 SD",
        Link = "fakeUrl",
        PublishDate = new DateTime(2015, 12, 05, 12, 35, 7),
        UploaderName = "Uploader",
        UploaderStatus = TorrentUploaderStatus.None,
        Size = 1024,
        Seeds = 1,
        Leeches = 1,
      };

      List<Torrent> torrents = new List<Torrent> { torrent };

      TorznabResponseBuilder builder = new TorznabResponseBuilder(new StubLogger());

      string response = builder.BuildResponse(torrents);
      string correctResponse = Resources.TorznabResponseSingleItem;
      correctResponse = correctResponse.Replace("Wed, 07 Sep 2016 00:00:00 +0000", "Sat, 05 Dec 2015 12:35:07 +0000");
      correctResponse = correctResponse.Replace("\r", "");

      Assert.AreEqual(correctResponse, response);
    }
  }
}
