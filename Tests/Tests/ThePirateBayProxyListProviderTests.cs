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
        new Proxy { Domain = "https://gameofbay.org", Country = "uk", Speed = ProxySpeed.VeryFast },
        new Proxy { Domain = "https://unblockedbay.info", Country = "us", Speed = ProxySpeed.VeryFast },
        new Proxy { Domain = "https://tpbunblocked.org", Country = "uk", Speed = ProxySpeed.Slow },
        new Proxy { Domain = "https://thepiratebay.uk.net", Country = "it", Speed = ProxySpeed.VeryFast },
        new Proxy { Domain = "http://piratebay.host", Country = "no", Speed = ProxySpeed.VeryFast },
        new Proxy { Domain = "https://piratebay.click", Country = "uk", Speed = ProxySpeed.Fast }
      };

      ThePirateBayProxyListProvider provider = new ThePirateBayProxyListProvider(new StubLogger(), webClient);

      List<Proxy> proxies = provider.ListProxies();

      Assert.AreEqual(correctProxies, proxies);
      Assert.AreEqual(1, webClient.RequestsMade.Count);
      Assert.AreEqual("https://thepiratebay-proxylist.org/api/v1/proxies", webClient.RequestsMade[0]);
    }

    [Test]
    public void TestListProxiesFastestNowSlow()
    {
      string responseString = Resources.ProxyListBestProxyNowSlow;
      StubWebClient webClient = new StubWebClient(new List<string>() { responseString });

      List<Proxy> correctProxies = new List<Proxy>
      {
        new Proxy { Domain = "https://gameofbay.org", Country = "uk", Speed = ProxySpeed.Slow },
        new Proxy { Domain = "https://unblockedbay.info", Country = "us", Speed = ProxySpeed.VeryFast },
        new Proxy { Domain = "https://tpbunblocked.org", Country = "uk", Speed = ProxySpeed.Slow },
        new Proxy { Domain = "https://thepiratebay.uk.net", Country = "it", Speed = ProxySpeed.VeryFast },
        new Proxy { Domain = "http://piratebay.host", Country = "no", Speed = ProxySpeed.VeryFast },
        new Proxy { Domain = "https://piratebay.click", Country = "uk", Speed = ProxySpeed.Fast }
      };

      ThePirateBayProxyListProvider provider = new ThePirateBayProxyListProvider(new StubLogger(), webClient);

      List<Proxy> proxies = provider.ListProxies();

      Assert.AreEqual(correctProxies, proxies);
      Assert.AreEqual(1, webClient.RequestsMade.Count);
      Assert.AreEqual("https://thepiratebay-proxylist.org/api/v1/proxies", webClient.RequestsMade[0]);
    }

    [Test]
    public void TestListMagnetOnlyProxies()
    {
      List<string> responses = new List<string>()
      {
        Resources.ProxyListSimple,
        Resources.PiratePageTop100WithMagnets,
        Resources.PiratePageTop100NoMagnets,
        Resources.PiratePageTop100WithMagnets,
        Resources.PiratePageTop100NoMagnets,
        Resources.PiratePageTop100WithMagnets,
        Resources.PiratePageTop100NoMagnets,
      };

      StubWebClient webClient = new StubWebClient(responses);

      List<Proxy> correctProxies = new List<Proxy>()
      {
        new Proxy { Domain = "https://gameofbay.org", Country = "uk", Speed = ProxySpeed.VeryFast },
        new Proxy { Domain = "https://tpbunblocked.org", Country = "uk", Speed = ProxySpeed.Slow },
        new Proxy { Domain = "http://piratebay.host", Country = "no", Speed = ProxySpeed.VeryFast }
      };

      ThePirateBayProxyListProvider provider = new ThePirateBayProxyListProvider(new StubLogger(), webClient);

      List<Proxy> proxies = provider.ListMagnetInSearchProxies();

      Assert.AreEqual(correctProxies, proxies);
      Assert.AreEqual(7, webClient.RequestsMade.Count);
      Assert.AreEqual("https://thepiratebay-proxylist.org/api/v1/proxies", webClient.RequestsMade[0]);
      Assert.AreEqual("https://gameofbay.org/top/200", webClient.RequestsMade[1]);
      Assert.AreEqual("https://unblockedbay.info/top/200", webClient.RequestsMade[2]);
      Assert.AreEqual("https://tpbunblocked.org/top/200", webClient.RequestsMade[3]);
      Assert.AreEqual("https://thepiratebay.uk.net/top/200", webClient.RequestsMade[4]);
      Assert.AreEqual("http://piratebay.host/top/200", webClient.RequestsMade[5]);
      Assert.AreEqual("https://piratebay.click/top/200", webClient.RequestsMade[6]);
    }
  }
}
