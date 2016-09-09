using NUnit.Framework;
using PirateAPI.ProxyProviders;
using PirateAPI.ProxyProviders.ThePirateBayProxyList;
using PirateAPI.WebServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PirateAPITests.Properties;
using PirateAPITests.Tests.StubClasses;

namespace PirateAPITests.Tests
{
  [TestFixture]
  public class ThePirateBayProxyListProviderTests
  {
    [Test]
    public void TestListProxies()
    {
      string responseString = Resources.ProxyListSimple;
      StubWebClient webClient = new StubWebClient(new List<string>() {responseString});

      List<Proxy> correctProxies = new List<Proxy>
      {
        new Proxy { Domain = "https://www.gameofbay.org", Country = "uk", Speed = ProxySpeed.VeryFast },
        new Proxy { Domain = "https://www.unblockedbay.info", Country = "us", Speed = ProxySpeed.VeryFast },
        new Proxy { Domain = "https://www.tpbunblocked.org", Country = "uk", Speed = ProxySpeed.Slow },
        new Proxy { Domain = "https://www.thepiratebay.uk.net", Country = "it", Speed = ProxySpeed.VeryFast },
        new Proxy { Domain = "http://www.piratebay.host", Country = "no", Speed = ProxySpeed.VeryFast },
        new Proxy { Domain = "https://www.piratebay.click", Country = "uk", Speed = ProxySpeed.Fast }
      };

      ThePirateBayProxyListProvider provider = new ThePirateBayProxyListProvider(new StubLogger(), webClient);

      List<Proxy> proxies = provider.ListProxies();

      Assert.AreEqual(correctProxies, proxies);
      Assert.AreEqual(1, webClient.RequestsMade.Count);
      Assert.AreEqual("https://thepiratebay-proxylist.org/api/v1/proxies", webClient.RequestsMade[0]);
    }
  }
}
