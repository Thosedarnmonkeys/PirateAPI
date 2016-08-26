using NUnit.Framework;
using PirateAPI.Parser;
using PirateAPITests.Tests.StubClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Tests
{
  [TestFixture]
  public class TorznabQueryParserTests
  {
    [Test]
    public void TestBasicParse()
    {
      string torznabQuery = "http://localhost:8080/api?t=tvsearch&cat=5030,5040&extended=1&offset=0&limit=100";
      string pirateProxy = "http://fakepirateproxy.com";

      TorznabQueryParser parser = new TorznabQueryParser(new StubLogger());

      PirateRequest correctRequest = new PirateRequest
      {
        RequestUrl = "http://fakepirateproxy.com/search/0/99/0",
        Offset = 0,
        Limit = 100,
        ExtendedAttributes = true,
      };

      PirateRequest parsedRequest = parser.Parse(torznabQuery, pirateProxy);

      Assert.AreEqual(correctRequest, parsedRequest);
    }
  }
}
