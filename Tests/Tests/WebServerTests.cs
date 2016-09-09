using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PirateAPI.WebServer;
using PirateAPITests.Tests.StubClasses;

namespace PirateAPITests.Tests
{
  [TestFixture]
  public class WebServerTests
  {
    [Test]
    public void TestBasicResponse()
    {
      string webRoot = "";
      int port = 8080;

      BasicWebServer webServer = new BasicWebServer(webRoot, port, GiveSimpleResponse, new StubLogger());
      bool listening = webServer.StartServing();

      Assert.IsTrue(listening);

      WebClient webClient = new WebClient();
      string response = webClient.DownloadString("http://localhost:" + port);

      Assert.AreEqual("Response", response);
    }

    [Test]
    public void TestAlternateWebRoot()
    {
      string webRoot = "/webroot";
      int port = 8081;

      BasicWebServer webServer = new BasicWebServer(webRoot, port, GiveSimpleResponse, new StubLogger());
      bool listening = webServer.StartServing();

      Assert.IsTrue(listening);

      string response;
      WebClient webClient = new WebClient();

      try
      {
        response = webClient.DownloadString("http://localhost:" + port);
      }
      catch (Exception)
      {
        response = "Exception";
      }

      Assert.AreEqual("Exception", response);

      try
      {
        response = webClient.DownloadString("http://localhost:" + port + "/something");
      }
      catch (Exception)
      {
        response = "ExceptionAgain";
      }

      Assert.AreEqual("ExceptionAgain", response);

      response = webClient.DownloadString("http://localhost:" + port + "/webroot");

      Assert.AreEqual("Response", response);
    }

    [Test]
    public void TestAlternatePort()
    {
      string webRoot = "";
      int port = 8082;

      BasicWebServer webServer = new BasicWebServer(webRoot, port, GiveSimpleResponse, new StubLogger());
      bool listening = webServer.StartServing();

      Assert.IsTrue(listening);

      WebClient webClient = new WebClient();
      string response;

      try
      {
        response = webClient.DownloadString("http://localhost:8080");
      }
      catch (Exception)
      {
        response = "Exception";
      }

      Assert.AreEqual("Exception", response);

      try
      {
        response = webClient.DownloadString("http://localhost:8081");
      }
      catch (Exception)
      {
        response = "ExceptionAgain";
      }

      Assert.AreEqual("ExceptionAgain", response);

      response = webClient.DownloadString("http://localhost:8082");

      Assert.AreEqual("Response", response);
    }

    [Test]
    public void TestStopAndRestartServing()
    {
      string webRoot = "";
      int port = 8083;

      BasicWebServer webServer = new BasicWebServer(webRoot, port, GiveSimpleResponse, new StubLogger());
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
    public void TestTryStartServingWhileAlreadyRunning()
    {
      string webRoot = "";
      int port = 8084;

      BasicWebServer webServer = new BasicWebServer(webRoot, port, GiveSimpleResponse, new StubLogger());
      bool listening = webServer.StartServing();

      Assert.IsTrue(listening);

      WebClient webClient = new WebClient();
      string response = webClient.DownloadString("http://localhost:" + port);
      Assert.AreEqual("Response", response);

      listening = webServer.StartServing();

      Assert.IsFalse(listening);

      response = webClient.DownloadString("http://localhost:" + port);
      Assert.AreEqual("Response", response);
    }

    [Test]
    public void TestBasicResponseTwiceInRow()
    {
      string webRoot = "";
      int port = 8085;

      BasicWebServer webServer = new BasicWebServer(webRoot, port, GiveSimpleResponse, new StubLogger());
      bool listening = webServer.StartServing();

      Assert.IsTrue(listening);

      WebClient webClient = new WebClient();
      string response = webClient.DownloadString("http://localhost:" + port);

      Assert.AreEqual("Response", response);

      response = null;
      response = webClient.DownloadString("http://localhost:" + port);

      Assert.AreEqual("Response", response);
    }

    private string GiveSimpleResponse(string request)
    {
      return "Response";
    }
  }
}
