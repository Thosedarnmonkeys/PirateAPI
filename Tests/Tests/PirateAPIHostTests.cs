using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using PirateAPI;
using PirateAPI.RequestResolver;
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
      int port = 8086;
      string webroot = "";
      List<string> proxyLocationPrefsList = new List<string>() { "uk", "us", "sd" };
      List<string> responses = new List<string>()
      {
        Resources.ProxyListSimple,
        Resources.PiratePageTop100WithMagnets,
        Resources.PiratePageSingleEpisode,
        Resources.PiratePageNoResults
      };
      StubWebClient client = new StubWebClient(responses);

      PirateAPIHost host = new PirateAPIHost(webroot, port, proxyLocationPrefsList, new List<string>(), new TimeSpan(1, 0, 0), false, 100, PirateRequestResolveStrategy.Series, new StubLogger(), client);
      Assert.IsTrue(host.StartServing());
      Assert.AreEqual(1, client.RequestsMade.Count);
      Assert.AreEqual("https://thepiratebay-proxylist.org/api/v1/proxies", client.RequestsMade[0]);

      string request = $"http://localhost:{port}/api?t=tvsearch&q=Rick+And+Morty&cat=5030,5040&ep=1&season=2&limit=5";
      WebClient webClient = new WebClient();
      string response = webClient.DownloadString(request);
      Assert.AreEqual(3, client.RequestsMade.Count);
      Assert.AreEqual("https://gameofbay.org/top/200", client.RequestsMade[1]);
      Assert.AreEqual("https://gameofbay.org/search/Rick%20And%20Morty%20S02E01/0/99/205,208", client.RequestsMade[2]);

      string correctResponse = Resources.TorznabResponseSingleEpisode;
      correctResponse = correctResponse.Replace("\r", "");
      correctResponse = SetSizeAndLengthTo3SigFig(correctResponse);
      response = SetSizeAndLengthTo3SigFig(response);
      Assert.AreEqual(correctResponse, response);
    }

    [Test]
    public void TestSingleSeason()
    {
      int port = 8087;
      string webroot = "";
      List<string> proxyLocationPrefsList = new List<string>() { "uk", "us", "sd" };
      List<string> responses = new List<string>()
      {
        Resources.ProxyListSimple,
        Resources.PiratePageTop100WithMagnets,
        Resources.PiratePageSingleSeason,
        Resources.PiratePageNoResults
      };
      StubWebClient client = new StubWebClient(responses);

      PirateAPIHost host = new PirateAPIHost(webroot, port, proxyLocationPrefsList, new List<string>(), new TimeSpan(1, 0, 0), false, 100, PirateRequestResolveStrategy.Series, new StubLogger(), client);
      Assert.IsTrue(host.StartServing());
      Assert.AreEqual(1, client.RequestsMade.Count);
      Assert.AreEqual("https://thepiratebay-proxylist.org/api/v1/proxies", client.RequestsMade[0]);

      string request = $"http://localhost:{port}/api?t=tvsearch&q=Rick+And+Morty&cat=5030,5040&season=2";
      WebClient webClient = new WebClient();
      string response = webClient.DownloadString(request);
      Assert.AreEqual(4, client.RequestsMade.Count);
      Assert.AreEqual("https://gameofbay.org/top/200", client.RequestsMade[1]);
      Assert.AreEqual("https://gameofbay.org/search/Rick%20And%20Morty%20Season%202/0/99/205,208", client.RequestsMade[2]);
      Assert.AreEqual("https://gameofbay.org/search/Rick%20And%20Morty%20Season%202/1/99/205,208", client.RequestsMade[3]);

      string correctResponse = Resources.TorznabResponseSingleSeason;
      correctResponse = correctResponse.Replace("\r", "");
      correctResponse = SetSizeAndLengthTo3SigFig(correctResponse);
      response = SetSizeAndLengthTo3SigFig(response);
      Assert.AreEqual(correctResponse, response);
    }

    [Test]
    public void TestDifferentLocationPriority()
    {
      int port = 8088;
      string webroot = "";
      List<string> proxyLocationPrefsList = new List<string>() {"it", "uk", "us", "sd" };
      List<string> responses = new List<string>()
      {
        Resources.ProxyListSimple,
        Resources.PiratePageTop100WithMagnets,
        Resources.PiratePageSingleEpisode,
        Resources.PiratePageNoResults
      };
      StubWebClient client = new StubWebClient(responses);

      PirateAPIHost host = new PirateAPIHost(webroot, port, proxyLocationPrefsList, new List<string>(), new TimeSpan(1, 0, 0), false, 100, PirateRequestResolveStrategy.Series, new StubLogger(), client);
      Assert.IsTrue(host.StartServing());
      Assert.AreEqual(1, client.RequestsMade.Count);
      Assert.AreEqual("https://thepiratebay-proxylist.org/api/v1/proxies", client.RequestsMade[0]);

      string request = $"http://localhost:{port}/api?t=tvsearch&q=Rick+And+Morty&cat=5030,5040&ep=1&season=2&limit=5";
      WebClient webClient = new WebClient();
      string response = webClient.DownloadString(request);
      Assert.AreEqual(3, client.RequestsMade.Count);
      Assert.AreEqual("https://thepiratebay.uk.net/top/200", client.RequestsMade[1]);
      Assert.AreEqual("https://thepiratebay.uk.net/search/Rick%20And%20Morty%20S02E01/0/99/205,208", client.RequestsMade[2]);

      string correctResponse = Resources.TorznabResponseSingleEpisode;
      correctResponse = correctResponse.Replace("\r", "");
      correctResponse = SetSizeAndLengthTo3SigFig(correctResponse);
      response = SetSizeAndLengthTo3SigFig(response);
      Assert.AreEqual(correctResponse, response);
    }

    [Test]
    public void TestDifferentWebRoot()
    {
      int port = 8089;
      string webroot = "/pirateapi";
      List<string> proxyLocationPrefsList = new List<string>() { "uk", "us", "sd" };
      List<string> responses = new List<string>()
      {
        Resources.ProxyListSimple,
        Resources.PiratePageTop100WithMagnets,
        Resources.PiratePageSingleEpisode,
        Resources.PiratePageNoResults
      };
      StubWebClient client = new StubWebClient(responses);

      PirateAPIHost host = new PirateAPIHost(webroot, port, proxyLocationPrefsList, new List<string>(), new TimeSpan(1, 0, 0), false, 100, PirateRequestResolveStrategy.Series, new StubLogger(), client);
      Assert.IsTrue(host.StartServing());
      Assert.AreEqual(1, client.RequestsMade.Count);
      Assert.AreEqual("https://thepiratebay-proxylist.org/api/v1/proxies", client.RequestsMade[0]);

      string request = $"http://localhost:{port}/pirateapi/api?t=tvsearch&q=Rick+And+Morty&cat=5030,5040&ep=1&season=2&limit=5";
      WebClient webClient = new WebClient();
      string response = webClient.DownloadString(request);
      Assert.AreEqual(3, client.RequestsMade.Count);
      Assert.AreEqual("https://gameofbay.org/top/200", client.RequestsMade[1]);
      Assert.AreEqual("https://gameofbay.org/search/Rick%20And%20Morty%20S02E01/0/99/205,208", client.RequestsMade[2]);

      string correctResponse = Resources.TorznabResponseSingleEpisode;
      correctResponse = correctResponse.Replace("\r", "");
      correctResponse = SetSizeAndLengthTo3SigFig(correctResponse);
      response = SetSizeAndLengthTo3SigFig(response);
      Assert.AreEqual(correctResponse, response);
    }

    [Test]
    public void TestProxyPickerRefreshesAfterInterval()
    {
      int port = 8090;
      string webroot = "";
      List<string> proxyLocationPrefsList = new List<string>() { "uk", "us", "sd" };
      List<string> responses = new List<string>()
      {
        Resources.ProxyListSimple,
        Resources.PiratePageTop100WithMagnets,
        Resources.PiratePageSingleEpisode,
        Resources.ProxyListBestProxyNowSlow,
        Resources.PiratePageTop100WithMagnets,
        Resources.PiratePageSingleEpisode
      };
      StubWebClient client = new StubWebClient(responses);

      PirateAPIHost host = new PirateAPIHost(webroot, port, proxyLocationPrefsList, new List<string>(), new TimeSpan(0, 0, 5), false, 100, PirateRequestResolveStrategy.Series, new StubLogger(), client);
      Assert.IsTrue(host.StartServing());
      Assert.AreEqual(1, client.RequestsMade.Count);
      Assert.AreEqual("https://thepiratebay-proxylist.org/api/v1/proxies", client.RequestsMade[0]);

      string request = $"http://localhost:{port}/api?t=tvsearch&q=Rick+And+Morty&cat=5030,5040&ep=1&season=2&limit=5";
      WebClient webClient = new WebClient();
      string response = webClient.DownloadString(request);
      Assert.AreEqual(3, client.RequestsMade.Count);
      Assert.AreEqual("https://gameofbay.org/top/200", client.RequestsMade[1]);
      Assert.AreEqual("https://gameofbay.org/search/Rick%20And%20Morty%20S02E01/0/99/205,208", client.RequestsMade[2]);

      string correctResponse = Resources.TorznabResponseSingleEpisode;
      correctResponse = correctResponse.Replace("\r", "");
      correctResponse = SetSizeAndLengthTo3SigFig(correctResponse);
      response = SetSizeAndLengthTo3SigFig(response);
      Assert.AreEqual(correctResponse, response);

      Thread.Sleep(new TimeSpan(0, 0, 7));

      Assert.AreEqual(4, client.RequestsMade.Count);
      Assert.AreEqual("https://thepiratebay-proxylist.org/api/v1/proxies", client.RequestsMade[3]);

      response = webClient.DownloadString(request);
      Assert.AreEqual(6, client.RequestsMade.Count);
      Assert.AreEqual("https://piratebay.click/top/200", client.RequestsMade[4]);
      Assert.AreEqual("https://piratebay.click/search/Rick%20And%20Morty%20S02E01/0/99/205,208", client.RequestsMade[5]);
      correctResponse = SetSizeAndLengthTo3SigFig(correctResponse);
      response = SetSizeAndLengthTo3SigFig(response);
      Assert.AreEqual(correctResponse, response);
    }

    [Test]
    public void TestProxyChangesAfterProxyFails()
    {
      int port = 8091;
      string webroot = "";
      List<string> proxyLocationPrefsList = new List<string>() { "uk", "us", "sd" };
      List<string> responses = new List<string>()
      {
        Resources.ProxyListSimple,
        null,
        null,
        null,
        Resources.PiratePageTop100WithMagnets,
        Resources.PiratePageSingleEpisode,
        Resources.PiratePageNoResults
      };
      StubWebClient client = new StubWebClient(responses);

      PirateAPIHost host = new PirateAPIHost(webroot, port, proxyLocationPrefsList, new List<string>(), new TimeSpan(1, 0, 0), false, 100, PirateRequestResolveStrategy.Series, new StubLogger(), client);
      Assert.IsTrue(host.StartServing());
      Assert.AreEqual(1, client.RequestsMade.Count);
      Assert.AreEqual("https://thepiratebay-proxylist.org/api/v1/proxies", client.RequestsMade[0]);

      string request = $"http://localhost:{port}/api?t=tvsearch&q=Rick+And+Morty&cat=5030,5040&ep=1&season=2&limit=5";
      WebClient webClient = new WebClient();
      string response = webClient.DownloadString(request);
      Assert.AreEqual(6, client.RequestsMade.Count);
      Assert.AreEqual("https://gameofbay.org/top/200", client.RequestsMade[1]);
      Assert.AreEqual("https://gameofbay.org/top/200", client.RequestsMade[2]);
      Assert.AreEqual("https://gameofbay.org/top/200", client.RequestsMade[3]);
      Assert.AreEqual("https://piratebay.click/top/200", client.RequestsMade[4]);
      Assert.AreEqual("https://piratebay.click/search/Rick%20And%20Morty%20S02E01/0/99/205,208", client.RequestsMade[5]);

      string correctResponse = Resources.TorznabResponseSingleEpisode;
      correctResponse = correctResponse.Replace("\r", "");
      correctResponse = SetSizeAndLengthTo3SigFig(correctResponse);
      response = SetSizeAndLengthTo3SigFig(response);
      Assert.AreEqual(correctResponse, response);
    }

    [Test]
    public void TestProxyPickerClearsTempBlacklistAfterInterval()
    {
      int port = 8092;
      string webroot = "";
      List<string> proxyLocationPrefsList = new List<string>() { "uk", "us", "sd" };
      List<string> responses = new List<string>()
      {
        Resources.ProxyListSimple,
        null,
        null,
        null,
        Resources.PiratePageTop100WithMagnets,
        Resources.PiratePageSingleEpisode,
        Resources.ProxyListSimple,
        Resources.PiratePageTop100WithMagnets,
        Resources.PiratePageSingleEpisode,
      };
      StubWebClient client = new StubWebClient(responses);

      PirateAPIHost host = new PirateAPIHost(webroot, port, proxyLocationPrefsList, new List<string>(), new TimeSpan(0, 0, 5), false, 100, PirateRequestResolveStrategy.Series, new StubLogger(), client);
      Assert.IsTrue(host.StartServing());
      Assert.AreEqual(1, client.RequestsMade.Count);
      Assert.AreEqual("https://thepiratebay-proxylist.org/api/v1/proxies", client.RequestsMade[0]);

      string request = $"http://localhost:{port}/api?t=tvsearch&q=Rick+And+Morty&cat=5030,5040&ep=1&season=2&limit=5";
      WebClient webClient = new WebClient();
      string response = webClient.DownloadString(request);
      Assert.AreEqual(6, client.RequestsMade.Count);
      Assert.AreEqual("https://gameofbay.org/top/200", client.RequestsMade[1]);
      Assert.AreEqual("https://gameofbay.org/top/200", client.RequestsMade[2]);
      Assert.AreEqual("https://gameofbay.org/top/200", client.RequestsMade[3]);
      Assert.AreEqual("https://piratebay.click/top/200", client.RequestsMade[4]);
      Assert.AreEqual("https://piratebay.click/search/Rick%20And%20Morty%20S02E01/0/99/205,208", client.RequestsMade[5]);

      string correctResponse = Resources.TorznabResponseSingleEpisode;
      correctResponse = correctResponse.Replace("\r", "");
      correctResponse = SetSizeAndLengthTo3SigFig(correctResponse);
      response = SetSizeAndLengthTo3SigFig(response);
      Assert.AreEqual(correctResponse, response);

      Thread.Sleep(new TimeSpan(0, 0, 7));

      Assert.AreEqual(7, client.RequestsMade.Count);
      Assert.AreEqual("https://thepiratebay-proxylist.org/api/v1/proxies", client.RequestsMade[6]);

      response = webClient.DownloadString(request);
      Assert.AreEqual(9, client.RequestsMade.Count);
      Assert.AreEqual("https://gameofbay.org/top/200", client.RequestsMade[7]);
      Assert.AreEqual("https://gameofbay.org/search/Rick%20And%20Morty%20S02E01/0/99/205,208", client.RequestsMade[8]);
      correctResponse = SetSizeAndLengthTo3SigFig(correctResponse);
      response = SetSizeAndLengthTo3SigFig(response);
      Assert.AreEqual(correctResponse, response);
    }

    [Test]
    public void TestStopServingAfterSingleRequest()
    {
      int port = 8093;
      string webroot = "";
      List<string> proxyLocationPrefsList = new List<string>() { "uk", "us", "sd" };
      List<string> responses = new List<string>()
      {
        Resources.ProxyListSimple,
        Resources.PiratePageTop100WithMagnets,
        Resources.PiratePageSingleEpisode,
        Resources.PiratePageNoResults
      };
      StubWebClient client = new StubWebClient(responses);

      PirateAPIHost host = new PirateAPIHost(webroot, port, proxyLocationPrefsList, new List<string>(), new TimeSpan(1, 0, 0), false, 100, PirateRequestResolveStrategy.Series, new StubLogger(), client);
      Assert.IsTrue(host.StartServing());
      Assert.AreEqual(1, client.RequestsMade.Count);
      Assert.AreEqual("https://thepiratebay-proxylist.org/api/v1/proxies", client.RequestsMade[0]);

      string request = $"http://localhost:{port}/api?t=tvsearch&q=Rick+And+Morty&cat=5030,5040&ep=1&season=2&limit=5";
      WebClient webClient = new WebClient();
      string response = webClient.DownloadString(request);
      Assert.AreEqual(3, client.RequestsMade.Count);
      Assert.AreEqual("https://gameofbay.org/top/200", client.RequestsMade[1]);
      Assert.AreEqual("https://gameofbay.org/search/Rick%20And%20Morty%20S02E01/0/99/205,208", client.RequestsMade[2]);

      string correctResponse = Resources.TorznabResponseSingleEpisode;
      correctResponse = correctResponse.Replace("\r", "");
      correctResponse = SetSizeAndLengthTo3SigFig(correctResponse);
      response = SetSizeAndLengthTo3SigFig(response);
      Assert.AreEqual(correctResponse, response);

      Assert.IsTrue(host.StopServing());

      Assert.Throws<WebException>(() => webClient.DownloadString(request));
    }

    [Test]
    public void TestStopServingAfterNoRequests()
    {
      int port = 8094;
      string webroot = "";
      List<string> proxyLocationPrefsList = new List<string>() { "uk", "us", "sd" };
      List<string> responses = new List<string>()
      {
        Resources.ProxyListSimple,
        Resources.PiratePageSingleEpisode,
        Resources.PiratePageNoResults
      };
      StubWebClient client = new StubWebClient(responses);

      PirateAPIHost host = new PirateAPIHost(webroot, port, proxyLocationPrefsList, new List<string>(), new TimeSpan(1, 0, 0), false, 100, PirateRequestResolveStrategy.Series, new StubLogger(), client);
      Assert.IsTrue(host.StartServing());
      Assert.IsTrue(host.StopServing());

      WebClient webClient = new WebClient();
      string request = $"http://localhost:{port}/api?t=tvsearch&q=Rick+And+Morty&cat=5030,5040&ep=1&season=2&limit=5";
      Assert.Throws<WebException>(() => webClient.DownloadString(request));
    }

    [Test]
    public void TestCapsRequest()
    {
      int port = 8095;
      string webroot = "";
      List<string> proxyLocationPrefsList = new List<string>() { "uk", "us", "sd" };
      List<string> responses = new List<string>();
      StubWebClient client = new StubWebClient(responses);

      PirateAPIHost host = new PirateAPIHost(webroot, port, proxyLocationPrefsList, new List<string>(), new TimeSpan(1, 0, 0), false, 50, PirateRequestResolveStrategy.Series, new StubLogger(), client);
      Assert.IsTrue(host.StartServing());
      Assert.AreEqual(1, client.RequestsMade.Count);
      Assert.AreEqual("https://thepiratebay-proxylist.org/api/v1/proxies", client.RequestsMade[0]);

      string request = $"http://localhost:{port}/api?t=caps";
      WebClient webClient = new WebClient();
      string response = webClient.DownloadString(request);
      string correctResponse = Resources.CapsResponseBasic;
      Assert.AreEqual(1, client.RequestsMade.Count);
      Assert.AreEqual(correctResponse, response);
    }

    [Test]
    public void TestNoShowName()
    {
      int port = 8096;
      string webroot = "";
      List<string> proxyLocationPrefsList = new List<string>() { "uk", "us", "sd" };
      List<string> responses = new List<string>()
      {
        Resources.ProxyListSimple,
        Resources.PiratePageTop100WithMagnets,
        Resources.PiratePageSingleEpisode,
        Resources.PiratePageNoResults
      };
      StubWebClient client = new StubWebClient(responses);

      PirateAPIHost host = new PirateAPIHost(webroot, port, proxyLocationPrefsList, new List<string>(), new TimeSpan(1, 0, 0), false, 100, PirateRequestResolveStrategy.Series, new StubLogger(), client);
      Assert.IsTrue(host.StartServing());
      Assert.AreEqual(1, client.RequestsMade.Count);
      Assert.AreEqual("https://thepiratebay-proxylist.org/api/v1/proxies", client.RequestsMade[0]);

      string request = $"http://localhost:{port}/api?t=tvsearch&cat=5030,5040&limit=5";
      WebClient webClient = new WebClient();
      string response = webClient.DownloadString(request);
      Assert.AreEqual(3, client.RequestsMade.Count);
      Assert.AreEqual("https://gameofbay.org/top/200", client.RequestsMade[1]);
      Assert.AreEqual("https://gameofbay.org/browse/208/0/3", client.RequestsMade[2]);

      string correctResponse = Resources.TorznabResponseSingleEpisode;
      correctResponse = correctResponse.Replace("\r", "");
      correctResponse = SetSizeAndLengthTo3SigFig(correctResponse);
      response = SetSizeAndLengthTo3SigFig(response);
      Assert.AreEqual(correctResponse, response);
    }

    [Test]
    public void TestProxyHasNoMagnetsInSearch()
    {
      int port = 8097;
      string webroot = "";
      List<string> proxyLocationPrefsList = new List<string>() { "uk", "us", "sd" };
      List<string> responses = new List<string>()
      {
        Resources.ProxyListSimple,
        Resources.PiratePageTop100NoMagnets,
        Resources.PiratePageSearch3RowsNoMagnets,
        Resources.RickAndMortySeason2,
        Resources.RickAndMortySeason2,
        Resources.RickAndMortySeason2,
        Resources.PiratePageNoResults
      };
      StubWebClient client = new StubWebClient(responses);

      PirateAPIHost apiHost = new PirateAPIHost(webroot, port, proxyLocationPrefsList, null, new TimeSpan(1, 0, 0), false, 100, PirateRequestResolveStrategy.Series, new StubLogger(), client);
      Assert.IsTrue(apiHost.StartServing());
      Assert.AreEqual(1, client.RequestsMade.Count);
      Assert.AreEqual("https://thepiratebay-proxylist.org/api/v1/proxies", client.RequestsMade[0]);

      string request = $"http://localhost:{port}/api?t=tvsearch&cat=5030,5040&q=Rick+And+Morty";
      WebClient webClient = new WebClient();
      string response = webClient.DownloadString(request);
      Assert.AreEqual(7, client.RequestsMade.Count);
      Assert.AreEqual("https://gameofbay.org/top/200", client.RequestsMade[1]);
      Assert.AreEqual("https://gameofbay.org/search/Rick%20And%20Morty/0/99/205,208", client.RequestsMade[2]);
      Assert.AreEqual("https://gameofbay.org/torrent/rickandmortyseason2", client.RequestsMade[3]);
      Assert.AreEqual("https://gameofbay.org/torrent/rickandmortyseason1", client.RequestsMade[4]);
      Assert.AreEqual("https://gameofbay.org/torrent/rickandmortyseason1and2", client.RequestsMade[5]);
      Assert.AreEqual("https://gameofbay.org/search/Rick%20And%20Morty/1/99/205,208", client.RequestsMade[6]);

      string correctResponse = Resources.TorznabResponseNoMagnets;
      correctResponse = correctResponse.Replace("\r", "");
      correctResponse = SetSizeAndLengthTo3SigFig(correctResponse);
      response = SetSizeAndLengthTo3SigFig(response);
      Assert.AreEqual(correctResponse, response);
    }

    [Test]
    public void TestNonTorznabRequest()
    {
      int port = 8098;
      string webroot = "";
      List<string> proxyLocationPrefsList = new List<string>() { "uk", "us", "sd" };
      List<string> responses = new List<string>()
      {
        Resources.ProxyListSimple,
      };
      StubWebClient client = new StubWebClient(responses);

      PirateAPIHost apiHost = new PirateAPIHost(webroot, port, proxyLocationPrefsList, null, new TimeSpan(1, 0, 0), false, 100, PirateRequestResolveStrategy.Series, new StubLogger(), client);
      Assert.IsTrue(apiHost.StartServing());
      Assert.AreEqual(1, client.RequestsMade.Count);
      Assert.AreEqual("https://thepiratebay-proxylist.org/api/v1/proxies", client.RequestsMade[0]);

      string request = $"http://localhost:{port}/favicon.ico";
      WebClient webClient = new WebClient();
      string response = webClient.DownloadString(request);
      Assert.AreEqual(1, client.RequestsMade.Count);

      Assert.AreEqual("Oh noes! Something went wrong :(", response);
    }

    [Test]
    public void TestMagnetSearchOnly()
    {
      int port = 8100;
      string webroot = "";
      List<string> proxyLocationPrefsList = new List<string>() { "uk", "us", "sd" };
      List<string> responses = new List<string>()
      {
        Resources.ProxyListSimple,
        Resources.PiratePageTop100NoMagnets,
        Resources.PiratePageTop100NoMagnets,
        Resources.PiratePageTop100WithMagnets,
        Resources.PiratePageTop100NoMagnets,
        Resources.PiratePageTop100NoMagnets,
        Resources.PiratePageTop100NoMagnets,
        Resources.PiratePageTop100WithMagnets,
        Resources.PiratePageSingleEpisode,
        Resources.PiratePageNoResults
      };

      StubWebClient client = new StubWebClient(responses);

      PirateAPIHost host = new PirateAPIHost(webroot, port, proxyLocationPrefsList, new List<string>(), new TimeSpan(1, 0, 0), true, 100, PirateRequestResolveStrategy.Series, new StubLogger(), client);
      Assert.IsTrue(host.StartServing());
      Assert.AreEqual(7, client.RequestsMade.Count);
      Assert.AreEqual("https://thepiratebay-proxylist.org/api/v1/proxies", client.RequestsMade[0]);
      Assert.AreEqual("https://gameofbay.org/top/200", client.RequestsMade[1]);
      Assert.AreEqual("https://unblockedbay.info/top/200", client.RequestsMade[2]);
      Assert.AreEqual("https://tpbunblocked.org/top/200", client.RequestsMade[3]);
      Assert.AreEqual("https://thepiratebay.uk.net/top/200", client.RequestsMade[4]);
      Assert.AreEqual("http://piratebay.host/top/200", client.RequestsMade[5]);
      Assert.AreEqual("https://piratebay.click/top/200", client.RequestsMade[6]);

      string request = $"http://localhost:{port}/api?t=tvsearch&q=Rick+And+Morty&cat=5030,5040&ep=1&season=2&limit=5";
      WebClient webClient = new WebClient();
      string response = webClient.DownloadString(request);
      Assert.AreEqual(9, client.RequestsMade.Count);
      Assert.AreEqual("https://tpbunblocked.org/top/200", client.RequestsMade[7]);
      Assert.AreEqual("https://tpbunblocked.org/search/Rick%20And%20Morty%20S02E01/0/99/205,208", client.RequestsMade[8]);

      string correctResponse = Resources.TorznabResponseSingleEpisode;
      correctResponse = correctResponse.Replace("\r", "");
      correctResponse = SetSizeAndLengthTo3SigFig(correctResponse);
      response = SetSizeAndLengthTo3SigFig(response);
      Assert.AreEqual(correctResponse, response);
    }


    private string SetSizeAndLengthTo3SigFig(string input)
    {
      string sizePattern = "<size>(.*?)</size>";
      Regex sizeRegex = new Regex(sizePattern);

      MatchCollection matches = sizeRegex.Matches(input);

      foreach (Match match in matches)
      {
        string numString = match.Groups[1].Value;
        double d = double.Parse(numString);

        double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(d))) + 1);
        double val = scale * Math.Round(d / scale, 3);
        double roundedNumber = Math.Floor(val);

        input = input.Replace($">{numString}<", $">{roundedNumber}<");
        input = input.Replace($"\"{numString}\"", $"\"{roundedNumber}\"");
      }

      return input;
    }
  }
}
