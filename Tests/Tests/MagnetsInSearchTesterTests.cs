using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PirateAPI.ProxyInfoGatherers;
using PirateAPITests.Properties;
using PirateAPITests.Tests.StubClasses;

namespace PirateAPITests.Tests
{
  [TestFixture]
  public class MagnetsInSearchTesterTests
  {
    [Test]
    public void TestSiteHasMagnets()
    {
      StubWebClient webClient = new StubWebClient(new List<string>() { Resources.PiratePageTop100WithMagnets });
      StubLogger logger = new StubLogger();
      string domain = "fakedomain.com";

      MagnetsInSearchTester tester = new MagnetsInSearchTester(webClient, logger);
      Assert.IsTrue(tester.DomainHasMagnetsInSearch(domain));
      Assert.AreEqual(1, webClient.RequestsMade.Count);
      Assert.AreEqual("fakedomain.com/top/200", webClient.RequestsMade[0]);
    }

    [Test]
    public void TestSiteDoesntHaveMagnets()
    {
      StubWebClient webClient = new StubWebClient(new List<string>() { Resources.TorrentRowNoMagnetLink });
      StubLogger logger = new StubLogger();
      string domain = "fakedomain.com";

      MagnetsInSearchTester tester = new MagnetsInSearchTester(webClient, logger);
      Assert.IsFalse(tester.DomainHasMagnetsInSearch(domain));
      Assert.AreEqual(1, webClient.RequestsMade.Count);
      Assert.AreEqual("fakedomain.com/top/200", webClient.RequestsMade[0]);
    }

    [Test]
    public void TestSiteDoesntRespond()
    {
      StubWebClient webClient = new StubWebClient(new List<string>() { null });
      StubLogger logger = new StubLogger();
      string domain = "fakedomain.com";

      MagnetsInSearchTester tester = new MagnetsInSearchTester(webClient, logger);
      Assert.IsNull(tester.DomainHasMagnetsInSearch(domain));
      Assert.AreEqual(1, webClient.RequestsMade.Count);
      Assert.AreEqual("fakedomain.com/top/200", webClient.RequestsMade[0]);
    }
  }
}
