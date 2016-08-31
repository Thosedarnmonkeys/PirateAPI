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
      StubWebClient webClient = new StubWebClient(() => responseString);

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
        new Torrent() { }
      };
      Assert.AreEqual(correctResponse, torrentStrings);

      string addressRequested = "http://fakepirateproxy.com/search/Rick%20And%20Morty/0/99/205,208";
      Assert.AreEqual(addressRequested, webClient.LastRequest);
    }
  }
}
