using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PirateAPI.ResponseBuilders.Caps;
using PirateAPITests.Properties;

namespace PirateAPITests.Tests
{
  [TestFixture]
  public class CapsReponseBuilderTests
  {
    [Test]
    public void TestDefaultHalvesLimit()
    {
      string correctResponse = Resources.CapsResponseBasic;

      CapsResponseBuilder builder = new CapsResponseBuilder();
      string response = builder.BuildResponse(50);

      Assert.AreEqual(correctResponse, response);
    }

    [Test]
    public void TestLimitSetTo0()
    {
      string correctResponse = Resources.CapsResponseLimit0;

      CapsResponseBuilder builder = new CapsResponseBuilder();
      string response = builder.BuildResponse(0);

      Assert.AreEqual(correctResponse, response);
    }

    [Test]
    public void TestLimitSetTo1()
    {
      string correctResponse = Resources.CapsResponseLimit1;

      CapsResponseBuilder builder = new CapsResponseBuilder();
      string response = builder.BuildResponse(1);

      Assert.AreEqual(correctResponse, response);
    }

    [Test]
    public void TestLimitSetToOddNumber()
    {
      string correctResponse = Resources.CapsResponseLimit9;

      CapsResponseBuilder builder = new CapsResponseBuilder();
      string response = builder.BuildResponse(9);

      Assert.AreEqual(correctResponse, response);
    }

    [Test]
    public void TestLimitSetToNegativeNumber()
    {
      string correctResponse = Resources.CapsResponseLimit0;

      CapsResponseBuilder builder = new CapsResponseBuilder();
      string response = builder.BuildResponse(-10);

      Assert.AreEqual(correctResponse, response);
    }
  }
}
