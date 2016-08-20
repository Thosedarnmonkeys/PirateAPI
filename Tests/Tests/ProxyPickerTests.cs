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
      List<string> locationPrefs = new List<string> {"uk", "us", "sd" };
      Proxy bestProxy = new Proxy { Country = "uk", Domain = "proxy.co.uk", Speed = ProxySpeed.VeryFast };

      List<Proxy> proxies = new List<Proxy>
      {
        new Proxy {Country = "uk", Domain = "proxy.co.uk", Speed = ProxySpeed.Slow },
        bestProxy,
        new Proxy {Country = "uk", Domain = "proxy.co.uk", Speed = ProxySpeed.NotResponding },
        new Proxy {Country = "uk", Domain = "proxy.co.uk", Speed = ProxySpeed.Fast },
      };

      ProxyPicker picker = new ProxyPicker(locationPrefs, new StubLogger());

      Proxy chosenProxy = picker.BestProxy(proxies);

      Assert.AreEqual(bestProxy, chosenProxy);
    }

    [Test]
    public void TestBestProxyLocationFirstChoice()
    {
      List<string> locationPrefs = new List<string> { "uk", "us", "sd" };
      Proxy bestProxy = new Proxy { Country = "uk", Domain = "proxy.co.uk", Speed = ProxySpeed.Slow };

      List<Proxy> proxies = new List<Proxy>
      {
        new Proxy {Country = "us", Domain = "proxy.co.uk", Speed = ProxySpeed.Slow },
        bestProxy,
        new Proxy {Country = "sd", Domain = "proxy.co.uk", Speed = ProxySpeed.Slow },
        new Proxy {Country = "es", Domain = "proxy.co.uk", Speed = ProxySpeed.Slow },
      };

      ProxyPicker picker = new ProxyPicker(locationPrefs, new StubLogger());

      Proxy chosenProxy = picker.BestProxy(proxies);

      Assert.AreEqual(bestProxy, chosenProxy);
    }
  }
}
