using NUnit.Framework;
using PirateAPI.Parser;
using PirateAPI.RequestResolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Tests
{
  [TestFixture]
  public class PirateRequestResolverTests
  {
    [Test]
    public void TestHandleBasicRequest()
    {
      DateTime pubDate = new DateTime(2016, 8, 1);

      string correctResponse = "";

      PirateRequest request = new PirateRequest
      {
        Limit = 5,
        Offset = 0,
        Quality = VideoQuality.Both,
        ExtendedAttributes = false,
        ShowName = "A+TV+Show",
        PirateProxyURL = "http://fakepirateproxy.com",
      };

      PirateRequestResolver resolver = new PirateRequestResolver();

      string torrentsString = resolver.Resolve(request);

      Assert.AreEqual(correctResponse, torrentsString);
    }
  }
}
