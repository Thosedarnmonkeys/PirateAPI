using NUnit.Framework;
using PirateAPI.ProxyProviders;
using PirateAPI.ProxyProviders.ThePirateBayProxyList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Tests.StubClasses;

namespace Tests.Tests
{
  [TestFixture]
  public class ThePirateBayProxyListProviderTests
  {
    [Test]
    public void TestListProxies()
    {
      ThePirateBayProxyListProvider provider = new ThePirateBayProxyListProvider(new StubLogger());

      List<Proxy> proxies = provider.ListProxies();

      Assert.IsNotEmpty(proxies);
    }
  }
}
