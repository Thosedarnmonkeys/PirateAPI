using NUnit.Framework;
using PirateAPI.Parser;
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

      TorznabQueryParser parser = new TorznabQueryParser();

      string parsedRequest = parser.Parse(torznabQuery);

      string correctResponse = "http://fakepirateproxy.com/s/?q=Show+Name&page=0&orderby=99";

      Assert.AreEqual(correctResponse, parsedRequest);
    }
  }
}
