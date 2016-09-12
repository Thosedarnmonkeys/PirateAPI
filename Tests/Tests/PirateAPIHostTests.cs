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
        Resources.PiratePageSingleEpisode,
        Resources.PiratePageNoResults
      };
      StubWebClient client = new StubWebClient(responses);

      PirateAPIHost host = new PirateAPIHost(webroot, port, proxyLocationPrefsList, new List<string>(), new TimeSpan(1, 0, 0),  new StubLogger(), client);
      Assert.IsTrue(host.StartServing());
      Assert.AreEqual(1, client.RequestsMade.Count);
      Assert.AreEqual("https://thepiratebay-proxylist.org/api/v1/proxies", client.RequestsMade[0]);

      string request = $"http://localhost:{port}/api?t=tvsearch&q=Rick+And+Morty&cat=5030,5040&ep=1&season=2&limit=5";
      WebClient webClient = new WebClient();
      string response = webClient.DownloadString(request);
      Assert.AreEqual(2, client.RequestsMade.Count);
      Assert.AreEqual("https://gameofbay.org/search/Rick%20And%20Morty%20S02E01/0/99/205,208", client.RequestsMade[1]);

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
        Resources.PiratePageSingleSeason,
        Resources.PiratePageNoResults
      };
      StubWebClient client = new StubWebClient(responses);

      PirateAPIHost host = new PirateAPIHost(webroot, port, proxyLocationPrefsList, new List<string>(), new TimeSpan(1, 0, 0), new StubLogger(), client);
      Assert.IsTrue(host.StartServing());
      Assert.AreEqual(1, client.RequestsMade.Count);
      Assert.AreEqual("https://thepiratebay-proxylist.org/api/v1/proxies", client.RequestsMade[0]);

      string request = $"http://localhost:{port}/api?t=tvsearch&q=Rick+And+Morty&cat=5030,5040&season=2";
      WebClient webClient = new WebClient();
      string response = webClient.DownloadString(request);
      Assert.AreEqual(3, client.RequestsMade.Count);
      Assert.AreEqual("https://gameofbay.org/search/Rick%20And%20Morty%20Season%202/0/99/205,208", client.RequestsMade[1]);
      Assert.AreEqual("https://gameofbay.org/search/Rick%20And%20Morty%20Season%202/1/99/205,208", client.RequestsMade[2]);

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
        Resources.PiratePageSingleEpisode,
        Resources.PiratePageNoResults
      };
      StubWebClient client = new StubWebClient(responses);

      PirateAPIHost host = new PirateAPIHost(webroot, port, proxyLocationPrefsList, new List<string>(), new TimeSpan(1, 0, 0), new StubLogger(), client);
      Assert.IsTrue(host.StartServing());
      Assert.AreEqual(1, client.RequestsMade.Count);
      Assert.AreEqual("https://thepiratebay-proxylist.org/api/v1/proxies", client.RequestsMade[0]);

      string request = $"http://localhost:{port}/api?t=tvsearch&q=Rick+And+Morty&cat=5030,5040&ep=1&season=2&limit=5";
      WebClient webClient = new WebClient();
      string response = webClient.DownloadString(request);
      Assert.AreEqual(2, client.RequestsMade.Count);
      Assert.AreEqual("https://thepiratebay.uk.net/search/Rick%20And%20Morty%20S02E01/0/99/205,208", client.RequestsMade[1]);

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
        Resources.PiratePageSingleEpisode,
        Resources.PiratePageNoResults
      };
      StubWebClient client = new StubWebClient(responses);

      PirateAPIHost host = new PirateAPIHost(webroot, port, proxyLocationPrefsList, new List<string>(), new TimeSpan(1, 0, 0), new StubLogger(), client);
      Assert.IsTrue(host.StartServing());
      Assert.AreEqual(1, client.RequestsMade.Count);
      Assert.AreEqual("https://thepiratebay-proxylist.org/api/v1/proxies", client.RequestsMade[0]);

      string request = $"http://localhost:{port}/pirateapi/api?t=tvsearch&q=Rick+And+Morty&cat=5030,5040&ep=1&season=2&limit=5";
      WebClient webClient = new WebClient();
      string response = webClient.DownloadString(request);
      Assert.AreEqual(2, client.RequestsMade.Count);
      Assert.AreEqual("https://gameofbay.org/search/Rick%20And%20Morty%20S02E01/0/99/205,208", client.RequestsMade[1]);

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
        Resources.PiratePageSingleEpisode,
        Resources.ProxyListBestProxyNowSlow,
        Resources.PiratePageSingleEpisode
      };
      StubWebClient client = new StubWebClient(responses);

      PirateAPIHost host = new PirateAPIHost(webroot, port, proxyLocationPrefsList, new List<string>(), new TimeSpan(0, 0, 5), new StubLogger(), client);
      Assert.IsTrue(host.StartServing());
      Assert.AreEqual(1, client.RequestsMade.Count);
      Assert.AreEqual("https://thepiratebay-proxylist.org/api/v1/proxies", client.RequestsMade[0]);

      string request = $"http://localhost:{port}/api?t=tvsearch&q=Rick+And+Morty&cat=5030,5040&ep=1&season=2&limit=5";
      WebClient webClient = new WebClient();
      string response = webClient.DownloadString(request);
      Assert.AreEqual(2, client.RequestsMade.Count);
      Assert.AreEqual("https://gameofbay.org/search/Rick%20And%20Morty%20S02E01/0/99/205,208", client.RequestsMade[1]);

      string correctResponse = Resources.TorznabResponseSingleEpisode;
      correctResponse = correctResponse.Replace("\r", "");
      correctResponse = SetSizeAndLengthTo3SigFig(correctResponse);
      response = SetSizeAndLengthTo3SigFig(response);
      Assert.AreEqual(correctResponse, response);

      Thread.Sleep(new TimeSpan(0, 0, 7));

      Assert.AreEqual(3, client.RequestsMade.Count);
      Assert.AreEqual("https://thepiratebay-proxylist.org/api/v1/proxies", client.RequestsMade[2]);

      response = webClient.DownloadString(request);
      Assert.AreEqual(4, client.RequestsMade.Count);
      Assert.AreEqual("https://piratebay.click/search/Rick%20And%20Morty%20S02E01/0/99/205,208", client.RequestsMade[3]);
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
        Resources.PiratePageSingleEpisode,
        Resources.PiratePageNoResults
      };
      StubWebClient client = new StubWebClient(responses);

      PirateAPIHost host = new PirateAPIHost(webroot, port, proxyLocationPrefsList, new List<string>(), new TimeSpan(1, 0, 0), new StubLogger(), client);
      Assert.IsTrue(host.StartServing());
      Assert.AreEqual(1, client.RequestsMade.Count);
      Assert.AreEqual("https://thepiratebay-proxylist.org/api/v1/proxies", client.RequestsMade[0]);

      string request = $"http://localhost:{port}/api?t=tvsearch&q=Rick+And+Morty&cat=5030,5040&ep=1&season=2&limit=5";
      WebClient webClient = new WebClient();
      string response = webClient.DownloadString(request);
      Assert.AreEqual(5, client.RequestsMade.Count);
      Assert.AreEqual("https://gameofbay.org/search/Rick%20And%20Morty%20S02E01/0/99/205,208", client.RequestsMade[1]);
      Assert.AreEqual("https://gameofbay.org/search/Rick%20And%20Morty%20S02E01/0/99/205,208", client.RequestsMade[2]);
      Assert.AreEqual("https://gameofbay.org/search/Rick%20And%20Morty%20S02E01/0/99/205,208", client.RequestsMade[3]);
      Assert.AreEqual("https://piratebay.click/search/Rick%20And%20Morty%20S02E01/0/99/205,208", client.RequestsMade[4]);

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
        Resources.PiratePageSingleEpisode,
        Resources.ProxyListSimple,
        Resources.PiratePageSingleEpisode,
      };
      StubWebClient client = new StubWebClient(responses);

      PirateAPIHost host = new PirateAPIHost(webroot, port, proxyLocationPrefsList, new List<string>(), new TimeSpan(0, 0, 5), new StubLogger(), client);
      Assert.IsTrue(host.StartServing());
      Assert.AreEqual(1, client.RequestsMade.Count);
      Assert.AreEqual("https://thepiratebay-proxylist.org/api/v1/proxies", client.RequestsMade[0]);

      string request = $"http://localhost:{port}/api?t=tvsearch&q=Rick+And+Morty&cat=5030,5040&ep=1&season=2&limit=5";
      WebClient webClient = new WebClient();
      string response = webClient.DownloadString(request);
      Assert.AreEqual(5, client.RequestsMade.Count);
      Assert.AreEqual("https://gameofbay.org/search/Rick%20And%20Morty%20S02E01/0/99/205,208", client.RequestsMade[1]);
      Assert.AreEqual("https://gameofbay.org/search/Rick%20And%20Morty%20S02E01/0/99/205,208", client.RequestsMade[2]);
      Assert.AreEqual("https://gameofbay.org/search/Rick%20And%20Morty%20S02E01/0/99/205,208", client.RequestsMade[3]);
      Assert.AreEqual("https://piratebay.click/search/Rick%20And%20Morty%20S02E01/0/99/205,208", client.RequestsMade[4]);

      string correctResponse = Resources.TorznabResponseSingleEpisode;
      correctResponse = correctResponse.Replace("\r", "");
      correctResponse = SetSizeAndLengthTo3SigFig(correctResponse);
      response = SetSizeAndLengthTo3SigFig(response);
      Assert.AreEqual(correctResponse, response);

      Thread.Sleep(new TimeSpan(0, 0, 7));

      Assert.AreEqual(6, client.RequestsMade.Count);
      Assert.AreEqual("https://thepiratebay-proxylist.org/api/v1/proxies", client.RequestsMade[5]);

      response = webClient.DownloadString(request);
      Assert.AreEqual(7, client.RequestsMade.Count);
      Assert.AreEqual("https://gameofbay.org/search/Rick%20And%20Morty%20S02E01/0/99/205,208", client.RequestsMade[6]);
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
        Resources.PiratePageSingleEpisode,
        Resources.PiratePageNoResults
      };
      StubWebClient client = new StubWebClient(responses);

      PirateAPIHost host = new PirateAPIHost(webroot, port, proxyLocationPrefsList, new List<string>(), new TimeSpan(1, 0, 0), new StubLogger(), client);
      Assert.IsTrue(host.StartServing());
      Assert.AreEqual(1, client.RequestsMade.Count);
      Assert.AreEqual("https://thepiratebay-proxylist.org/api/v1/proxies", client.RequestsMade[0]);

      string request = $"http://localhost:{port}/api?t=tvsearch&q=Rick+And+Morty&cat=5030,5040&ep=1&season=2&limit=5";
      WebClient webClient = new WebClient();
      string response = webClient.DownloadString(request);
      Assert.AreEqual(2, client.RequestsMade.Count);
      Assert.AreEqual("https://gameofbay.org/search/Rick%20And%20Morty%20S02E01/0/99/205,208", client.RequestsMade[1]);

      string correctResponse = Resources.TorznabResponseSingleEpisode;
      correctResponse = correctResponse.Replace("\r", "");
      correctResponse = SetSizeAndLengthTo3SigFig(correctResponse);
      response = SetSizeAndLengthTo3SigFig(response);
      Assert.AreEqual(correctResponse, response);

      Assert.IsTrue(host.StopServing());

      Assert.Throws<WebException>(() => client.DownloadString(request));
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

      PirateAPIHost host = new PirateAPIHost(webroot, port, proxyLocationPrefsList, new List<string>(), new TimeSpan(1, 0, 0), new StubLogger(), client);
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

      PirateAPIHost host = new PirateAPIHost(webroot, port, proxyLocationPrefsList, new List<string>(), new TimeSpan(1, 0, 0), new StubLogger(), client);
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
        Resources.PiratePageSingleEpisode,
        Resources.PiratePageNoResults
      };
      StubWebClient client = new StubWebClient(responses);

      PirateAPIHost host = new PirateAPIHost(webroot, port, proxyLocationPrefsList, new List<string>(), new TimeSpan(1, 0, 0), new StubLogger(), client);
      Assert.IsTrue(host.StartServing());
      Assert.AreEqual(1, client.RequestsMade.Count);
      Assert.AreEqual("https://thepiratebay-proxylist.org/api/v1/proxies", client.RequestsMade[0]);

      string request = $"http://localhost:{port}/api?t=tvsearch&cat=5030,5040&limit=5";
      WebClient webClient = new WebClient();
      string response = webClient.DownloadString(request);
      Assert.AreEqual(2, client.RequestsMade.Count);
      Assert.AreEqual("https://gameofbay.org/browse/208/0/3", client.RequestsMade[1]);

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
