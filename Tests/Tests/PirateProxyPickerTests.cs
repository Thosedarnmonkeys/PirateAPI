using NUnit.Framework;
using PirateAPI.ProxyPicker;
using PirateAPI.ProxyProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PirateAPITests.Tests.StubClasses;

namespace PirateAPITests.Tests
{
  [TestFixture]
  public class ProxyPickerTests
  {
    [Test]
    public void TestBestProxySpeedFullSpread()
    {
      List<string> locationPrefs = new List<string> { "uk", "us", "sd" };
      List<Proxy> bestProxies = new List<Proxy>
      {
        new Proxy { Country = "uk", Domain = "domain", Speed = ProxySpeed.VeryFast },
        new Proxy { Country  = "us", Domain = "domain", Speed = ProxySpeed.VeryFast },
        new Proxy { Country  = "sd", Domain = "domain", Speed = ProxySpeed.VeryFast },
        new Proxy {Country = "uk", Domain = "domain", Speed = ProxySpeed.Fast },
        new Proxy {Country = "us", Domain = "domain", Speed = ProxySpeed.Fast },
      };

      List<Proxy> proxies = new List<Proxy>
      {
        new Proxy {Country = "uk", Domain = "domain", Speed = ProxySpeed.Slow },
        new Proxy {Country = "uk", Domain = "domain", Speed = ProxySpeed.NotResponding },
        bestProxies[0],
        bestProxies[1],
        bestProxies[2],
        bestProxies[3],
        bestProxies[4],
      };

      PirateProxyPicker picker = new PirateProxyPicker(locationPrefs, null, new StubLogger());

      List<Proxy> chosenProxies = picker.BestProxies(proxies);

      AssertListsAreEqual(bestProxies, chosenProxies);
    }

    [Test]
    public void TestBestProxyLocationFirstChoice()
    {
      List<string> locationPrefs = new List<string> { "uk", "us", "sd" };
      List<Proxy> bestProxies = new List<Proxy>
      {
        new Proxy { Country = "uk", Domain = "domain", Speed = ProxySpeed.Slow },
        new Proxy { Country = "uk", Domain = "domain", Speed = ProxySpeed.Slow },
        new Proxy { Country = "uk", Domain = "domain", Speed = ProxySpeed.Slow },
        new Proxy { Country = "us", Domain = "domain", Speed = ProxySpeed.Slow },
        new Proxy { Country = "us", Domain = "domain", Speed = ProxySpeed.Slow },
      };

      List<Proxy> proxies = new List<Proxy>
      {
        new Proxy {Country = "us", Domain = "domain", Speed = ProxySpeed.Slow },
        bestProxies[0],
        bestProxies[1],
        bestProxies[2],
        bestProxies[3],
        bestProxies[4],
        new Proxy {Country = "sd", Domain = "domain", Speed = ProxySpeed.Slow },
        new Proxy {Country = "es", Domain = "domain", Speed = ProxySpeed.Slow },
      };

      PirateProxyPicker picker = new PirateProxyPicker(locationPrefs, null, new StubLogger());

      List<Proxy> chosenProxies = picker.BestProxies(proxies);

      AssertListsAreEqual(bestProxies, chosenProxies);
    }

    [Test]
    public void TestBestProxyLocationSecondChoice()
    {
      List<string> locationPrefs = new List<string> { "uk", "us", "sd" };
      List<Proxy> bestProxies = new List<Proxy>
      {
        new Proxy { Country = "us", Domain = "domain", Speed = ProxySpeed.Slow },
        new Proxy { Country = "us", Domain = "domain", Speed = ProxySpeed.Slow },
        new Proxy { Country = "us", Domain = "domain", Speed = ProxySpeed.Slow },
        new Proxy { Country = "sd", Domain = "domain", Speed = ProxySpeed.Slow },
        new Proxy { Country = "sd", Domain = "domain", Speed = ProxySpeed.Slow },
      };

      List<Proxy> proxies = new List<Proxy>
      {
        new Proxy {Country = "sd", Domain = "domain", Speed = ProxySpeed.Slow },
        bestProxies[0],
        bestProxies[1],
        bestProxies[2],
        bestProxies[3],
        bestProxies[4],
        new Proxy {Country = "it", Domain = "domain", Speed = ProxySpeed.Slow },
        new Proxy {Country = "es", Domain = "domain", Speed = ProxySpeed.Slow },
      };

      PirateProxyPicker picker = new PirateProxyPicker(locationPrefs, null, new StubLogger());

      List<Proxy> chosenProxies = picker.BestProxies(proxies);

      AssertListsAreEqual(bestProxies, chosenProxies);
    }

    [Test]
    public void TestBestProxyLocationPreferLocationOverSpeed()
    {
      List<string> locationPrefs = new List<string> { "uk", "us", "sd" };
      List<Proxy> bestProxies = new List<Proxy>
      {
        new Proxy { Country = "uk", Domain = "domain", Speed = ProxySpeed.Slow },
        new Proxy { Country = "uk", Domain = "domain", Speed = ProxySpeed.Slow },
        new Proxy { Country = "uk", Domain = "domain", Speed = ProxySpeed.Slow },
        new Proxy { Country = "uk", Domain = "domain", Speed = ProxySpeed.Slow },
        new Proxy { Country = "uk", Domain = "domain", Speed = ProxySpeed.Slow },
      };

      List<Proxy> proxies = new List<Proxy>
      {
        new Proxy {Country = "us", Domain = "domain", Speed = ProxySpeed.VeryFast},
        bestProxies[0],
        bestProxies[1],
        bestProxies[2],
        bestProxies[3],
        bestProxies[4],
        new Proxy {Country = "sd", Domain = "domain", Speed = ProxySpeed.Fast },
        new Proxy {Country = "es", Domain = "domain", Speed = ProxySpeed.Fast },
      };

      PirateProxyPicker picker = new PirateProxyPicker(locationPrefs, null, new StubLogger());

      List<Proxy> chosenProxies = picker.BestProxies(proxies);

      AssertListsAreEqual(bestProxies, chosenProxies);
    }

    [Test]
    public void TestBestProxyLocationPicksSpeedTiebreak()
    {
      List<string> locationPrefs = new List<string> { "uk", "us", "sd" };
      List<Proxy> bestProxies = new List<Proxy>
      {
        new Proxy { Country = "uk", Domain = "domain", Speed = ProxySpeed.VeryFast },
        new Proxy { Country = "uk", Domain = "domain", Speed = ProxySpeed.VeryFast },
        new Proxy { Country = "uk", Domain = "domain", Speed = ProxySpeed.VeryFast },
        new Proxy { Country = "uk", Domain = "domain", Speed = ProxySpeed.VeryFast },
        new Proxy { Country = "uk", Domain = "domain", Speed = ProxySpeed.VeryFast },
      };

      List<Proxy> proxies = new List<Proxy>
      {
        new Proxy {Country = "uk", Domain = "domain", Speed = ProxySpeed.Slow },
        new Proxy {Country = "uk", Domain = "domain", Speed = ProxySpeed.Slow },
        new Proxy {Country = "uk", Domain = "domain", Speed = ProxySpeed.Slow },
        bestProxies[0],
        bestProxies[1],
        bestProxies[2],
        bestProxies[3],
        bestProxies[4],
        new Proxy {Country = "us", Domain = "domain", Speed = ProxySpeed.VeryFast },
        new Proxy {Country = "sd", Domain = "domain", Speed = ProxySpeed.VeryFast },
      };

      PirateProxyPicker picker = new PirateProxyPicker(locationPrefs, null, new StubLogger());

      List<Proxy> chosenProxies = picker.BestProxies(proxies);

      AssertListsAreEqual(bestProxies, chosenProxies);
    }

    [Test]
    public void TestBestProxyRespectsTempBlacklistedDomains()
    {
      List<string> locationPrefs = new List<string> { "uk", "us", "sd" };
      Proxy bestProxy = new Proxy { Country = "us", Domain = "domain1", Speed = ProxySpeed.Fast };
      Proxy blackListProxy = new Proxy { Country = "uk", Domain = "domain2", Speed = ProxySpeed.VeryFast };

      List<Proxy> bestProxies = new List<Proxy>
      {
        bestProxy,
        blackListProxy
      };

      List<Proxy> proxies = new List<Proxy>
      {
        blackListProxy,
        bestProxy,
        new Proxy {Country = "us", Domain = "domain3", Speed = ProxySpeed.Slow },
        new Proxy {Country = "sd", Domain = "domain4", Speed = ProxySpeed.VeryFast },
      };

      PirateProxyPicker picker = new PirateProxyPicker(locationPrefs, null, new StubLogger());

      List<Proxy> chosenProxies = picker.BestProxies(proxies);
      AssertListsAreEqual(bestProxies, chosenProxies);

      picker.TempBlacklistDomain("domain2");

      chosenProxies = picker.BestProxies(proxies);
      bestProxies.Remove(blackListProxy);

      AssertListsAreEqual(bestProxies, chosenProxies);
    }

    [Test]
    public void TestBestProxyIgnoresPermanentBlacklistedDomains()
    {
      List<string> locationPrefs = new List<string> { "uk", "us", "sd" };
      Proxy bestProxy = new Proxy { Country = "us", Domain = "domain1", Speed = ProxySpeed.Fast };
      Proxy blackListProxy = new Proxy { Country = "uk", Domain = "domain2", Speed = ProxySpeed.VeryFast };

      List<Proxy> bestProxies = new List<Proxy>
      {
        bestProxy,
      };

      List<Proxy> proxies = new List<Proxy>
      {
        blackListProxy,
        bestProxy,
        new Proxy {Country = "us", Domain = "domain3", Speed = ProxySpeed.Slow },
        new Proxy {Country = "sd", Domain = "domain4", Speed = ProxySpeed.VeryFast },
      };

      List<string> blacklist = new List<string>
      {
        "domain2"
      };

      PirateProxyPicker picker = new PirateProxyPicker(locationPrefs, blacklist, new StubLogger());

      List<Proxy> chosenProxies = picker.BestProxies(proxies);
      AssertListsAreEqual(bestProxies, chosenProxies);
    }

    [Test]
    public void TestBestProxyIgnoresTempAndPermanentBlacklistedDomains()
    {
      List<string> locationPrefs = new List<string> { "uk", "us", "sd" };
      Proxy nextBestProxy = new Proxy { Country = "us", Domain = "domain3", Speed = ProxySpeed.Slow };

      List<Proxy> bestProxies = new List<Proxy>
      {
        nextBestProxy
      };

      List<Proxy> proxies = new List<Proxy>
      {
        new Proxy {Country = "us", Domain = "domain1", Speed = ProxySpeed.Fast },
        new Proxy { Country = "uk", Domain = "domain2", Speed = ProxySpeed.VeryFast },
        nextBestProxy,
        new Proxy {Country = "sd", Domain = "domain4", Speed = ProxySpeed.VeryFast },
      };

      List<string> blacklist = new List<string>
      {
        "domain1"
      };

      PirateProxyPicker picker = new PirateProxyPicker(locationPrefs, blacklist, new StubLogger());

      picker.TempBlacklistDomain("domain2");

      List<Proxy> chosenProxy = picker.BestProxies(proxies);
      AssertListsAreEqual(bestProxies, chosenProxy);
    }


    [Test]
    public void TestBestProxyReturnsNullWhenAllDomainsBlackListed()
    {
      List<string> locationPrefs = new List<string> { "uk", "us", "sd" };

      List<Proxy> proxies = new List<Proxy>
      {
        new Proxy {Country = "uk", Domain = "domain", Speed = ProxySpeed.Fast},
        new Proxy {Country = "it", Domain = "domain", Speed = ProxySpeed.NotResponding },
        new Proxy {Country = "us", Domain = "domain", Speed = ProxySpeed.Slow },
        new Proxy {Country = "sd", Domain = "domain", Speed = ProxySpeed.VeryFast },
      };

      PirateProxyPicker picker = new PirateProxyPicker(locationPrefs, null, new StubLogger());

      picker.TempBlacklistDomain("domain");

      List<Proxy> chosenProxy = picker.BestProxies(proxies);
      Assert.IsNull(chosenProxy);
    }

    [Test]
    public void TestBestProxyWhenClearTempBlacklistedDomains()
    {
      List<string> locationPrefs = new List<string> { "uk", "us", "sd" };
      Proxy nextBestProxy = new Proxy { Country = "us", Domain = "domain1", Speed = ProxySpeed.Fast };
      Proxy blackListProxy = new Proxy { Country = "uk", Domain = "domain2", Speed = ProxySpeed.VeryFast };

      List<Proxy> bestProxies = new List<Proxy>
      {
        blackListProxy,
        nextBestProxy
      };


      List<Proxy> proxies = new List<Proxy>
      {
        blackListProxy,
        nextBestProxy,
        new Proxy {Country = "us", Domain = "domain3", Speed = ProxySpeed.Slow },
        new Proxy {Country = "sd", Domain = "domain4", Speed = ProxySpeed.VeryFast },
      };

      PirateProxyPicker picker = new PirateProxyPicker(locationPrefs, null, new StubLogger());

      List<Proxy> chosenProxy = picker.BestProxies(proxies);
      AssertListsAreEqual(bestProxies, chosenProxy);

      picker.TempBlacklistDomain("domain2");

      bestProxies.Remove(blackListProxy);
      chosenProxy = picker.BestProxies(proxies);
      AssertListsAreEqual(bestProxies, chosenProxy);

      picker.ClearTempBlacklist();

      bestProxies.Insert(0, blackListProxy);
      chosenProxy = picker.BestProxies(proxies);
      AssertListsAreEqual(bestProxies, chosenProxy);
    }


    private void AssertListsAreEqual(List<Proxy> l1, List<Proxy> l2)
    {
      if (l1 == null && l2 == null)
        return;

      if (l1 == null && l2 != null)
        Assert.Fail("First list was null, second wasn't");

      if (l1 != null && l2 == null)
        Assert.Fail("First list wasn't null, second one was");

      if (l1.Count != l2.Count)
        Assert.Fail($"Lists of different lengths, l1 is {l1.Count} long, l2 is {l2.Count} long");

      for (int i = 0; i < l1.Count; i++)
      {
        if (!l1[i].Equals(l2[i]))
          Assert.Fail($"Items differ at index {i}");
      }
    }
  }
}
