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
      string torznabQuery = "http://localhost:8080/api?t=tvsearch&q=A+TV+Show&cat=5030,5040&extended=1&offset=0&limit=100";
      string pirateProxy = "http://fakepirateproxy.com";

      TorznabQueryParser parser = new TorznabQueryParser(new StubLogger());

      PirateRequest correctRequest = new PirateRequest
      {
        ShowName = "A+TV+Show",
        PirateProxyURL = "http://fakepirateproxy.com",
        Offset = 0,
        Limit = 100,
        ExtendedAttributes = true,
      };

      PirateRequest parsedRequest = parser.Parse(torznabQuery, pirateProxy);

      Assert.AreEqual(correctRequest, parsedRequest);

      string correctPirateURL = "http://fakepirateproxy.com/search/A%20TV%20Show/0/99/0";

      Assert.AreEqual(correctPirateURL, parsedRequest.RequestUrl);
    }
  }
}
