using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PirateAPI.WebServer;
using Tests.Tests.StubClasses;

namespace Tests.Tests
{
  [TestFixture]
  public class WebServerTests
  {
    [Test]
    public void TestBasicResponse()
    {
      string webRoot = "";
      int port = 8080;

      WebServer webServer = new WebServer(webRoot, port, GiveSimpleResponse, new StubLogger());
      bool listening = webServer.StartServing();

      Assert.IsTrue(listening);

      WebClient webClient = new WebClient();
      string response = webClient.DownloadString("http://localhost:" + port);

      Assert.AreEqual("Response", response);
    }

    [Test]
    public void TestAlternateWebRoot()
    {
      string webRoot = "webroot";
      int port = 8080;

      WebServer webServer = new WebServer(webRoot, port, GiveSimpleResponse, new StubLogger());
      bool listening = webServer.StartServing();

      Assert.IsTrue(listening);

      WebClient webClient = new WebClient();
      string response = webClient.DownloadString("http://localhost:" + port);

      Assert.IsNull(response);

      response = webClient.DownloadString("http://localhost:" + port + "something");

      Assert.IsNull(response);

      response = webClient.DownloadString("http://localhost:" + port + "webroot");

      Assert.AreEqual("Response", response);
    }

    [Test]
    public void TestAlternatePort()
    {
      string webRoot = "";
      int port = 8081;

      WebServer webServer = new WebServer(webRoot, port, GiveSimpleResponse, new StubLogger());
      bool listening = webServer.StartServing();

      Assert.IsTrue(listening);

      WebClient webClient = new WebClient();
      string response = webClient.DownloadString("http://localhost:8080");

      Assert.IsNull(response);

      response = webClient.DownloadString("http://localhost:8082");

      Assert.IsNull(response);

      response = webClient.DownloadString("http://localhost:8081");

      Assert.AreEqual("Response", response);
    }

    [Test]
    public void TestStopAndRestartServing()
    {
      string webRoot = "";
      int port = 8080;

      WebServer webServer = new WebServer(webRoot, port, GiveSimpleResponse, new StubLogger());
      bool listening = webServer.StartServing();

      Assert.IsTrue(listening);

      WebClient webClient = new WebClient();
      string response = webClient.DownloadString("http://localhost:" + port);
      Assert.AreEqual("Response", response);

      webServer.StopServing();

      try
      {
        response = webClient.DownloadString("http://localhost:" + port);
      }
      catch (Exception)
      {
        response = "Exception";
      }
      Assert.AreEqual("Exception", response);

      listening = webServer.StartServing();
      Assert.IsTrue(listening);

      response = webClient.DownloadString("http://localhost:" + port);
      Assert.AreEqual("Response", response);
    }

    [Test]
    public void TestStartServingWhileAlreadyRunning()
    {
      string webRoot = "";
      int port = 8080;

      WebServer webServer = new WebServer(webRoot, port, GiveSimpleResponse, new StubLogger());
      bool listening = webServer.StartServing();

      Assert.IsTrue(listening);

      WebClient webClient = new WebClient();
      string response = webClient.DownloadString("http://localhost:" + port);
      Assert.AreEqual("Response", response);

      listening = webServer.StartServing();

      Assert.IsTrue(listening);

      response = webClient.DownloadString("http://localhost:" + port);
      Assert.AreEqual("Response", response);
    }

    private string GiveSimpleResponse(string request)
    {
      return "Response";
    }
  }
}
