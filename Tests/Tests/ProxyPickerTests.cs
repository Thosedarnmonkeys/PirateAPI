using NUnit.Framework;
using PirateAPI.PirateProxyPicker;
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
      Proxy bestProxy = new Proxy { Country = "uk", Domain = "domain", Speed = ProxySpeed.VeryFast };

      List<Proxy> proxies = new List<Proxy>
      {
        new Proxy {Country = "uk", Domain = "domain", Speed = ProxySpeed.Slow },
        bestProxy,
        new Proxy {Country = "uk", Domain = "domain", Speed = ProxySpeed.NotResponding },
        new Proxy {Country = "uk", Domain = "domain", Speed = ProxySpeed.Fast },
      };

      ProxyPicker picker = new ProxyPicker(locationPrefs, new StubLogger());

      Proxy chosenProxy = picker.BestProxy(proxies);

      Assert.AreEqual(bestProxy, chosenProxy);
    }

    [Test]
    public void TestBestProxyLocationFirstChoice()
    {
      List<string> locationPrefs = new List<string> { "uk", "us", "sd" };
      Proxy bestProxy = new Proxy { Country = "uk", Domain = "domain", Speed = ProxySpeed.Slow };

      List<Proxy> proxies = new List<Proxy>
      {
        new Proxy {Country = "us", Domain = "domain", Speed = ProxySpeed.Slow },
        bestProxy,
        new Proxy {Country = "sd", Domain = "domain", Speed = ProxySpeed.Slow },
        new Proxy {Country = "es", Domain = "domain", Speed = ProxySpeed.Slow },
      };

      ProxyPicker picker = new ProxyPicker(locationPrefs, new StubLogger());

      Proxy chosenProxy = picker.BestProxy(proxies);

      Assert.AreEqual(bestProxy, chosenProxy);
    }

    [Test]
    public void TestBestProxyLocationSecondChoice()
    {
      List<string> locationPrefs = new List<string> { "uk", "us", "sd" };
      Proxy bestProxy = new Proxy { Country = "us", Domain = "domain", Speed = ProxySpeed.Slow };

      List<Proxy> proxies = new List<Proxy>
      {
        new Proxy {Country = "sd", Domain = "domain", Speed = ProxySpeed.Slow },
        bestProxy,
        new Proxy {Country = "it", Domain = "domain", Speed = ProxySpeed.Slow },
        new Proxy {Country = "es", Domain = "domain", Speed = ProxySpeed.Slow },
      };

      ProxyPicker picker = new ProxyPicker(locationPrefs, new StubLogger());

      Proxy chosenProxy = picker.BestProxy(proxies);

      Assert.AreEqual(bestProxy, chosenProxy);
    }

    [Test]
    public void TestBestProxyLocationPreferLocationOverSpeed()
    {
      List<string> locationPrefs = new List<string> { "uk", "us", "sd" };
      Proxy bestProxy = new Proxy { Country = "uk", Domain = "domain", Speed = ProxySpeed.Slow };

      List<Proxy> proxies = new List<Proxy>
      {
        new Proxy {Country = "us", Domain = "domain", Speed = ProxySpeed.VeryFast},
        bestProxy,
        new Proxy {Country = "sd", Domain = "domain", Speed = ProxySpeed.Fast },
        new Proxy {Country = "es", Domain = "domain", Speed = ProxySpeed.Fast },
      };

      ProxyPicker picker = new ProxyPicker(locationPrefs, new StubLogger());

      Proxy chosenProxy = picker.BestProxy(proxies);

      Assert.AreEqual(bestProxy, chosenProxy);
    }

    [Test]
    public void TestBestProxyLocationPicksSpeedTiebreak()
    {
      List<string> locationPrefs = new List<string> { "uk", "us", "sd" };
      Proxy bestProxy = new Proxy { Country = "uk", Domain = "domain", Speed = ProxySpeed.VeryFast };

      List<Proxy> proxies = new List<Proxy>
      {
        new Proxy {Country = "uk", Domain = "domain", Speed = ProxySpeed.Fast },
        bestProxy,
        new Proxy {Country = "us", Domain = "domain", Speed = ProxySpeed.VeryFast },
        new Proxy {Country = "sd", Domain = "domain", Speed = ProxySpeed.VeryFast },
      };

      ProxyPicker picker = new ProxyPicker(locationPrefs, new StubLogger());

      Proxy chosenProxy = picker.BestProxy(proxies);

      Assert.AreEqual(bestProxy, chosenProxy);
    }

    [Test]
    public void TestBestProxyIgnoresBlacklistedDomains()
    {
      List<string> locationPrefs = new List<string> { "uk", "us", "sd" };
      Proxy bestProxy = new Proxy { Country = "us", Domain = "domain1", Speed = ProxySpeed.Fast };
      Proxy blackListProxy = new Proxy { Country = "uk", Domain = "domain2", Speed = ProxySpeed.VeryFast };

      List<Proxy> proxies = new List<Proxy>
      {
        blackListProxy,
        bestProxy,
        new Proxy {Country = "us", Domain = "domain3", Speed = ProxySpeed.Slow },
        new Proxy {Country = "sd", Domain = "domain4", Speed = ProxySpeed.VeryFast },
      };

      ProxyPicker picker = new ProxyPicker(locationPrefs, new StubLogger());

      Proxy chosenProxy = picker.BestProxy(proxies);
      Assert.AreEqual(blackListProxy, chosenProxy);

      picker.BlacklistDomain("domain2");

      chosenProxy = picker.BestProxy(proxies);
      Assert.AreEqual(bestProxy, chosenProxy);
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

      ProxyPicker picker = new ProxyPicker(locationPrefs, new StubLogger());

      picker.BlacklistDomain("domain");

      Proxy chosenProxy = picker.BestProxy(proxies);
      Assert.IsNull(chosenProxy);
    }

  }
}
