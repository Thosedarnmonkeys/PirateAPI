using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PirateAPI;
using PirateAPITests.Tests.StubClasses;
using PirateAPITests.Properties;

namespace PirateAPITests.Tests
{
  [TestFixture]
  public class PirateAPIHostTests
  {
    [Test]
    public void TestSingleEpisode()
    {
      int port = 8080;
      string webroot = "";
      List<string> proxyLocationPrefsList = new List<string>() { "uk", "us", "sd" };
      List<string> responses = new List<string>()
      {
        Resources.ProxyListSimple,
        Resources.PiratePageSingleEpisode,
        Resources.PiratePageNoResults
      };
      StubWebClient client = new StubWebClient(responses);

      PirateAPIHost host = new PirateAPIHost(webroot, port, proxyLocationPrefsList, new StubLogger(), client);
      host.StartServing();
      Assert.AreEqual(1, client.RequestsMade.Count);
      Assert.AreEqual("https://thepiratebay-proxylist.org/api/v1/proxies", client.RequestsMade[0]);

      string request = $"http://localhost:{port}/api?t=tvsearch&q=Rick+And+Morty&cat=5030,5040&ep=1&season=2";
      WebClient webClient = new WebClient();
      string response = webClient.DownloadString(request);
      Assert.AreEqual(3, client.RequestsMade.Count);
      Assert.AreEqual("https://www.gameofbay.org/search/Rick%20And%20Morty%20S02E01/0/99/205,208", client.RequestsMade[1]);
      Assert.AreEqual("https://www.gameofbay.org/search/Rick%20And%20Morty%20S02E01/1/99/205,208", client.RequestsMade[2]);

      string correctResponse = Resources.TorznabResponseSingleEpisode;
      correctResponse = correctResponse.Replace("\r", "");
      Assert.AreEqual(correctResponse, response);
    }
  }
}
