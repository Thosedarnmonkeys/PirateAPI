using NUnit.Framework;
using PirateAPI.Parsers.Torznab;
using PirateAPITests.Tests.StubClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirateAPITests.Tests
{
  [TestFixture]
  public class TorznabQueryParserTests
  {
    [Test]
    public void TestBasicParse()
    {
      string torznabQuery = "http://localhost:8080/api?t=tvsearch&q=A+TV+Show&cat=5030,5040&extended=0&offset=0&limit=100";
      string pirateProxy = "http://fakepirateproxy.com";

      TorznabQueryParser parser = new TorznabQueryParser(new StubLogger());

      TorznabQueryType correctQueryType = TorznabQueryType.TvSearch;
      TorznabQueryType queryType = parser.DiscernQueryType(torznabQuery);
      Assert.AreEqual(correctQueryType, queryType);

      PirateRequest correctRequest = new PirateRequest
      {
        ShowName = "A TV Show",
        PirateProxyURL = "http://fakepirateproxy.com",
        Offset = 0,
        Limit = 100,
        ExtendedAttributes = false,
        Quality = VideoQuality.Both
      };

      PirateRequest parsedRequest = parser.Parse(torznabQuery, pirateProxy);

      Assert.AreEqual(correctRequest, parsedRequest);
    }

    [Test]
    public void TestParsesLimits()
    {
      string torznabQuery = "http://localhost:8080/api?t=tvsearch&q=A+TV+Show&cat=5030,5040&extended=1&offset=0&limit=20";
      string pirateProxy = "http://fakepirateproxy.com";

      TorznabQueryParser parser = new TorznabQueryParser(new StubLogger());

      TorznabQueryType correctQueryType = TorznabQueryType.TvSearch;
      TorznabQueryType queryType = parser.DiscernQueryType(torznabQuery);
      Assert.AreEqual(correctQueryType, queryType);

      PirateRequest correctRequest = new PirateRequest
      {
        ShowName = "A TV Show",
        PirateProxyURL = "http://fakepirateproxy.com",
        Offset = 0,
        Limit = 20,
        ExtendedAttributes = true,
        Quality = VideoQuality.Both
      };

      PirateRequest parsedRequest = parser.Parse(torznabQuery, pirateProxy);

      Assert.AreEqual(correctRequest, parsedRequest);
    }

    [Test]
    public void TestParsesOffset()
    {
      string torznabQuery = "http://localhost:8080/api?t=tvsearch&q=A+TV+Show&cat=5030,5040&extended=1&offset=50&limit=100";
      string pirateProxy = "http://fakepirateproxy.com";

      TorznabQueryParser parser = new TorznabQueryParser(new StubLogger());

      TorznabQueryType correctQueryType = TorznabQueryType.TvSearch;
      TorznabQueryType queryType = parser.DiscernQueryType(torznabQuery);
      Assert.AreEqual(correctQueryType, queryType);

      PirateRequest correctRequest = new PirateRequest
      {
        ShowName = "A TV Show",
        PirateProxyURL = "http://fakepirateproxy.com",
        Offset = 50,
        Limit = 100,
        ExtendedAttributes = true,
        Quality = VideoQuality.Both
      };

      PirateRequest parsedRequest = parser.Parse(torznabQuery, pirateProxy);

      Assert.AreEqual(correctRequest, parsedRequest);
    }

    [Test]
    public void TestParsesSearchQuery()
    {
      string torznabQuery = "http://localhost:8080/api?t=tvsearch&q=A+Different+Thing&cat=5030,5040&extended=1&offset=0&limit=100";
      string pirateProxy = "http://fakepirateproxy.com";

      TorznabQueryParser parser = new TorznabQueryParser(new StubLogger());

      TorznabQueryType correctQueryType = TorznabQueryType.TvSearch;
      TorznabQueryType queryType = parser.DiscernQueryType(torznabQuery);
      Assert.AreEqual(correctQueryType, queryType);

      PirateRequest correctRequest = new PirateRequest
      {
        ShowName = "A Different Thing",
        PirateProxyURL = "http://fakepirateproxy.com",
        Offset = 0,
        Limit = 100,
        ExtendedAttributes = true,
        Quality = VideoQuality.Both
      };

      PirateRequest parsedRequest = parser.Parse(torznabQuery, pirateProxy);

      Assert.AreEqual(correctRequest, parsedRequest);
    }

    [Test]
    public void TestParsesSearchQueryWithHTMLSpaces()
    {
      string torznabQuery = "http://localhost:8080/api?t=tvsearch&q=A%20Different%20Thing&cat=5030,5040&extended=1&offset=0&limit=100";
      string pirateProxy = "http://fakepirateproxy.com";

      TorznabQueryParser parser = new TorznabQueryParser(new StubLogger());

      TorznabQueryType correctQueryType = TorznabQueryType.TvSearch;
      TorznabQueryType queryType = parser.DiscernQueryType(torznabQuery);
      Assert.AreEqual(correctQueryType, queryType);

      PirateRequest correctRequest = new PirateRequest
      {
        ShowName = "A Different Thing",
        PirateProxyURL = "http://fakepirateproxy.com",
        Offset = 0,
        Limit = 100,
        ExtendedAttributes = true,
        Quality = VideoQuality.Both
      };

      PirateRequest parsedRequest = parser.Parse(torznabQuery, pirateProxy);

      Assert.AreEqual(correctRequest, parsedRequest);
    }


    [Test]
    public void TestParsesExtendedAttributes()
    {
      string torznabQuery = "http://localhost:8080/api?t=tvsearch&q=A+TV+Show&cat=5030,5040&extended=1&offset=0&limit=100";
      string pirateProxy = "http://fakepirateproxy.com";

      TorznabQueryParser parser = new TorznabQueryParser(new StubLogger());

      TorznabQueryType correctQueryType = TorznabQueryType.TvSearch;
      TorznabQueryType queryType = parser.DiscernQueryType(torznabQuery);
      Assert.AreEqual(correctQueryType, queryType);

      PirateRequest correctRequest = new PirateRequest
      {
        ShowName = "A TV Show",
        PirateProxyURL = "http://fakepirateproxy.com",
        Offset = 0,
        Limit = 100,
        ExtendedAttributes = true,
        Quality = VideoQuality.Both
      };

      PirateRequest parsedRequest = parser.Parse(torznabQuery, pirateProxy);

      Assert.AreEqual(correctRequest, parsedRequest);
    }

    [Test]
    public void TestAddsDomainCorrectly()
    {
      string torznabQuery = "http://localhost:8080/api?t=tvsearch&q=A+TV+Show&cat=5030,5040&extended=1&offset=0&limit=100";
      string pirateProxy = "http://adifferentfakepirateproxy.com";

      TorznabQueryParser parser = new TorznabQueryParser(new StubLogger());

      TorznabQueryType correctQueryType = TorznabQueryType.TvSearch;
      TorznabQueryType queryType = parser.DiscernQueryType(torznabQuery);
      Assert.AreEqual(correctQueryType, queryType);

      PirateRequest correctRequest = new PirateRequest
      {
        ShowName = "A TV Show",
        PirateProxyURL = "http://adifferentfakepirateproxy.com",
        Offset = 0,
        Limit = 100,
        ExtendedAttributes = true,
        Quality = VideoQuality.Both
      };

      PirateRequest parsedRequest = parser.Parse(torznabQuery, pirateProxy);

      Assert.AreEqual(correctRequest, parsedRequest);
    }

    [Test]
    public void TestParsesVideoQualityHD()
    {
      string torznabQuery = "http://localhost:8080/api?t=tvsearch&q=A+TV+Show&cat=5040&extended=0&offset=0&limit=100";
      string pirateProxy = "http://fakepirateproxy.com";

      TorznabQueryParser parser = new TorznabQueryParser(new StubLogger());

      TorznabQueryType correctQueryType = TorznabQueryType.TvSearch;
      TorznabQueryType queryType = parser.DiscernQueryType(torznabQuery);
      Assert.AreEqual(correctQueryType, queryType);

      PirateRequest correctRequest = new PirateRequest
      {
        ShowName = "A TV Show",
        PirateProxyURL = "http://fakepirateproxy.com",
        Offset = 0,
        Limit = 100,
        ExtendedAttributes = false,
        Quality = VideoQuality.HD
      };

      PirateRequest parsedRequest = parser.Parse(torznabQuery, pirateProxy);

      Assert.AreEqual(correctRequest, parsedRequest);
    }

    [Test]
    public void TestParsesVideoQualitySD()
    {
      string torznabQuery = "http://localhost:8080/api?t=tvsearch&q=A+TV+Show&cat=5030&extended=0&offset=0&limit=100";
      string pirateProxy = "http://fakepirateproxy.com";

      TorznabQueryParser parser = new TorznabQueryParser(new StubLogger());

      TorznabQueryType correctQueryType = TorznabQueryType.TvSearch;
      TorznabQueryType queryType = parser.DiscernQueryType(torznabQuery);
      Assert.AreEqual(correctQueryType, queryType);

      PirateRequest correctRequest = new PirateRequest
      {
        ShowName = "A TV Show",
        PirateProxyURL = "http://fakepirateproxy.com",
        Offset = 0,
        Limit = 100,
        ExtendedAttributes = false,
        Quality = VideoQuality.SD
      };

      PirateRequest parsedRequest = parser.Parse(torznabQuery, pirateProxy);

      Assert.AreEqual(correctRequest, parsedRequest);
    }

    [Test]
    public void TestOmitsLimitIfNotPresent()
    {
      string torznabQuery = "http://localhost:8080/api?t=tvsearch&q=A+TV+Show&cat=5030,5040&extended=0&offset=0";
      string pirateProxy = "http://fakepirateproxy.com";

      TorznabQueryParser parser = new TorznabQueryParser(new StubLogger());

      TorznabQueryType correctQueryType = TorznabQueryType.TvSearch;
      TorznabQueryType queryType = parser.DiscernQueryType(torznabQuery);
      Assert.AreEqual(correctQueryType, queryType);

      PirateRequest correctRequest = new PirateRequest
      {
        ShowName = "A TV Show",
        PirateProxyURL = "http://fakepirateproxy.com",
        Offset = 0,
        Limit = null,
        ExtendedAttributes = false,
        Quality = VideoQuality.Both
      };

      PirateRequest parsedRequest = parser.Parse(torznabQuery, pirateProxy);

      Assert.AreEqual(correctRequest, parsedRequest);
    }

    [Test]
    public void TestParsesSeason()
    {
      string torznabQuery = "http://localhost:8080/api?t=tvsearch&q=A+TV+Show&cat=5030,5040&season=1";
      string pirateProxy = "http://fakepirateproxy.com";

      TorznabQueryParser parser = new TorznabQueryParser(new StubLogger());

      TorznabQueryType correctQueryType = TorznabQueryType.TvSearch;
      TorznabQueryType queryType = parser.DiscernQueryType(torznabQuery);
      Assert.AreEqual(correctQueryType, queryType);

      PirateRequest correctRequest = new PirateRequest
      {
        ShowName = "A TV Show",
        PirateProxyURL = "http://fakepirateproxy.com",
        Offset = 0,
        Limit = null,
        ExtendedAttributes = false,
        Quality = VideoQuality.Both,
        Season = 1
      };

      PirateRequest parsedRequest = parser.Parse(torznabQuery, pirateProxy);

      Assert.AreEqual(correctRequest, parsedRequest);
    }

    [Test]
    public void TestParsesEpisode()
    {
      string torznabQuery = "http://localhost:8080/api?t=tvsearch&q=A+TV+Show&cat=5030,5040&ep=10";
      string pirateProxy = "http://fakepirateproxy.com";

      TorznabQueryParser parser = new TorznabQueryParser(new StubLogger());

      TorznabQueryType correctQueryType = TorznabQueryType.TvSearch;
      TorznabQueryType queryType = parser.DiscernQueryType(torznabQuery);
      Assert.AreEqual(correctQueryType, queryType);

      PirateRequest correctRequest = new PirateRequest
      {
        ShowName = "A TV Show",
        PirateProxyURL = "http://fakepirateproxy.com",
        Offset = 0,
        Limit = null,
        ExtendedAttributes = false,
        Quality = VideoQuality.Both,
        Episode = 10
      };

      PirateRequest parsedRequest = parser.Parse(torznabQuery, pirateProxy);

      Assert.AreEqual(correctRequest, parsedRequest);
    }

    [Test]
    public void TestParsesSeasonAndEpisode()
    {
      string torznabQuery = "http://localhost:8080/api?t=tvsearch&q=A+TV+Show&cat=5030,5040&ep=12&season=3";
      string pirateProxy = "http://fakepirateproxy.com";

      TorznabQueryParser parser = new TorznabQueryParser(new StubLogger());

      TorznabQueryType correctQueryType = TorznabQueryType.TvSearch;
      TorznabQueryType queryType = parser.DiscernQueryType(torznabQuery);
      Assert.AreEqual(correctQueryType, queryType);

      PirateRequest correctRequest = new PirateRequest
      {
        ShowName = "A TV Show",
        PirateProxyURL = "http://fakepirateproxy.com",
        Offset = 0,
        Limit = null,
        ExtendedAttributes = false,
        Quality = VideoQuality.Both,
        Episode = 12,
        Season = 3
      };

      PirateRequest parsedRequest = parser.Parse(torznabQuery, pirateProxy);

      Assert.AreEqual(correctRequest, parsedRequest);
    }

    [Test]
    public void TestParsesMaxAge()
    {
      string torznabQuery = "http://localhost:8080/api?t=tvsearch&q=A+TV+Show&cat=5030,5040&maxage=30";
      string pirateProxy = "http://fakepirateproxy.com";

      TorznabQueryParser parser = new TorznabQueryParser(new StubLogger());

      TorznabQueryType correctQueryType = TorznabQueryType.TvSearch;
      TorznabQueryType queryType = parser.DiscernQueryType(torznabQuery);
      Assert.AreEqual(correctQueryType, queryType);

      PirateRequest correctRequest = new PirateRequest
      {
        ShowName = "A TV Show",
        PirateProxyURL = "http://fakepirateproxy.com",
        Quality = VideoQuality.Both,
        MaxAge = 30
      };

      PirateRequest parsedRequest = parser.Parse(torznabQuery, pirateProxy);

      Assert.AreEqual(correctRequest, parsedRequest);
    }

    [Test]
    public void TestCapsResponse()
    {
      string torznabQuery = "http://localhost:8080/api?t=caps";

      TorznabQueryParser parser = new TorznabQueryParser(new StubLogger());

      TorznabQueryType correctQueryType = TorznabQueryType.Caps;
      TorznabQueryType queryType = parser.DiscernQueryType(torznabQuery);
      Assert.AreEqual(correctQueryType, queryType);
    }

    [Test]
    public void TestIsTorznabQuerySuccess()
    {
      string torznabQuery = "http://localhost:8080/api?t=tvsearch&q=A+TV+Show&cat=5030,5040";
      TorznabQueryParser parser = new TorznabQueryParser(new StubLogger());
      Assert.IsTrue(parser.IsValidRequest(torznabQuery));
    }

    [Test]
    public void TestIsTorznabQuerySuccessOnCaps()
    {
      string torznabQuery = "http://localhost:8080/api?t=caps";
      TorznabQueryParser parser = new TorznabQueryParser(new StubLogger());
      Assert.IsTrue(parser.IsValidRequest(torznabQuery));
    }

    [Test]
    public void TestIsTorznabQueryFailureDueToMissingTEqualsParam()
    {
      string torznabQuery = "http://localhost:8080/api?q=A+TV+Show&cat=5030,5040";
      TorznabQueryParser parser = new TorznabQueryParser(new StubLogger());
      Assert.IsFalse(parser.IsValidRequest(torznabQuery));
    }

    [Test]
    public void TestIsTorznabQueryFailureDueToMissingCatParam()
    {
      string torznabQuery = "http://localhost:8080/api?t=tvsearch&q=A+TV+Show";
      TorznabQueryParser parser = new TorznabQueryParser(new StubLogger());
      Assert.IsFalse(parser.IsValidRequest(torznabQuery));
    }

    [Test]
    public void TestIsTorznabQueryFailureDueToMalformedInput()
    {
      string torznabQuery = "http://localhost:8080/ap?t=tvsearch&q=A+TV+Show&cat=5030,5040";
      TorznabQueryParser parser = new TorznabQueryParser(new StubLogger());
      Assert.IsFalse(parser.IsValidRequest(torznabQuery));
    }

    [Test]
    public void TestIsTorznabQueryFailureDueToNoParams()
    {
      string torznabQuery = "http://localhost:8080/api?";
      TorznabQueryParser parser = new TorznabQueryParser(new StubLogger());
      Assert.IsFalse(parser.IsValidRequest(torznabQuery));
    }


    [Test]
    public void TestIsTorznabQueryFailureDueToGarbage()
    {
      string torznabQuery = "http://localhost:8080/favicon.ico";
      TorznabQueryParser parser = new TorznabQueryParser(new StubLogger());
      Assert.IsFalse(parser.IsValidRequest(torznabQuery));
    }
  }
}
