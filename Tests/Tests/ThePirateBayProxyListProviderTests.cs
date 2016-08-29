using NUnit.Framework;
using PirateAPI.ProxyProviders;
using PirateAPI.ProxyProviders.ThePirateBayProxyList;
using PirateAPI.WebServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PirateAPITests.Tests.StubClasses;

namespace PirateAPITests.Tests
{
  [TestFixture]
  public class ThePirateBayProxyListProviderTests
  {
    [Test]
    public void TestListProxies()
    {
      List<Proxy> correctProxies = new List<Proxy>
      {
        new Proxy { Domain = "https://www.gameofbay.org", Country = "uk", Speed = ProxySpeed.VeryFast },
        new Proxy { Domain = "https://www.unblockedbay.info", Country = "us", Speed = ProxySpeed.VeryFast },
        new Proxy { Domain = "https://www.tpbunblocked.org", Country = "uk", Speed = ProxySpeed.Slow },
        new Proxy { Domain = "https://www.thepiratebay.uk.net", Country = "it", Speed = ProxySpeed.VeryFast },
        new Proxy { Domain = "http://www.piratebay.host", Country = "no", Speed = ProxySpeed.VeryFast },
        new Proxy { Domain = "https://www.piratebay.click", Country = "uk", Speed = ProxySpeed.Fast }
      };

      BasicWebServer server = new BasicWebServer("", 80, ProxyString, new StubLogger());

      ThePirateBayProxyListProvider provider = new ThePirateBayProxyListProvider(new StubLogger());

      List<Proxy> proxies = provider.ListProxies();

      Assert.AreEqual(correctProxies, proxies);
    }

    private string ProxyString(string s)
    {
      string response = "{\"proxies\":[" +
        "{\"domain\":\"gameofbay.org\",\"speed\":0.002,\"secure\":true,\"country\":\"UK\",\"probed\":true}," +
        "{\"domain\":\"unblockedbay.info\",\"speed\":0.061,\"secure\":true,\"country\":\"US\",\"probed\":true}," +
        "{\"domain\":\"tpbunblocked.org\",\"speed\":1.112,\"secure\":true,\"country\":\"UK\",\"probed\":true}," +
        "{\"domain\":\"thepiratebay.uk.net\",\"speed\":0.179,\"secure\":true,\"country\":\"IT\",\"probed\":true}," +
        "{\"domain\":\"piratebay.host\",\"speed\":0.208,\"secure\":false,\"country\":\"NO\",\"probed\":true}," +
        "{\"domain\":\"piratebay.click\",\"speed\":0.416,\"secure\":true,\"country\":\"UK\",\"probed\":true}," +
        "{\"last_updated\":1472467201}}";

      return response;
    }
  }
}
