using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PirateAPI;


namespace PirateAPITests.Tests
{
  [TestFixture]
  public class PirateAPIHostTests
  {
    [Test]
    public void TestSimpleTestRequest()
    {
      int port = 8080;
      string webroot = "";
      List<string> proxyLocationPrefsList = new List<string>() {"uk", "us", "sd"};

      PirateAPIHost host = new PirateAPIHost(webroot, port, proxyLocationPrefsList);
      host.StartServing();

      string request = @"http://localhost:" + port + "/test";

      WebClient webClient = new WebClient();
      string response = webClient.DownloadString(request);

      Assert.AreEqual("Pirate API is up and running!", response);
    }
  }
}
