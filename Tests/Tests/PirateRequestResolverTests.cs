using NUnit.Framework;
using PirateAPI.Parser;
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
      string responseString = Resources.PiratePageSearchRickAndMorty;

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

      PirateRequestResolver resolver = new PirateRequestResolver(new StubLogger(), new StubWebClient(() => responseString));

      string torrentsString = resolver.Resolve(request);

      Assert.AreEqual(correctResponse, torrentsString);
    }
  }
}
