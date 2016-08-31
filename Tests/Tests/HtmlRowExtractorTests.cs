using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PirateAPI.HtmlExtractors;
using PirateAPITests.Tests.StubClasses;
using Tests.Properties;

namespace Tests.Tests
{
  [TestFixture]
  public class HtmlRowExtractorTests
  {
    [Test]
    public void TestPiratePage5Rows()
    {
      string piratePage = Resources.PiratePageSearch5Rows;

      HtmlRowExtractor extractor = new HtmlRowExtractor(new StubLogger());

      List<string> rows = extractor.ExtractRows(piratePage);
      List<string> correctRows = Resources.PiratePageSearch5RowsSplit
                                  .Replace(Environment.NewLine, "")
                                  .Replace("\t", "")
                                  .Split('#')
                                  .ToList();

      Assert.AreEqual(correctRows, rows);
    }

    [Test]
    public void TestPiratePage30Rows()
    {
      string piratePage = Resources.PiratePageSearch;

      HtmlRowExtractor extractor = new HtmlRowExtractor(new StubLogger());

      List<string> rows = extractor.ExtractRows(piratePage);
      List<string> correctRows = Resources.PiratePageSearchSplit
                                  .Replace(Environment.NewLine, "")
                                  .Replace("\t", "")
                                  .Split('#')
                                  .ToList();

      Assert.AreEqual(correctRows, rows);
    }

    [Test]
    public void TestPiratePageNoResults()
    {
      string piratePage = Resources.PiratePageNoResults;

      HtmlRowExtractor extractor = new HtmlRowExtractor(new StubLogger());

      List<string> rows = extractor.ExtractRows(piratePage);
      List<string> correctRows = new List<string>();

      Assert.AreEqual(correctRows, rows);
    }

    [Test]
    public void TestEmptyPage()
    {
      string piratePage = "";

      HtmlRowExtractor extractor = new HtmlRowExtractor(new StubLogger());

      List<string> rows = extractor.ExtractRows(piratePage);
      List<string> correctRows = new List<string>();

      Assert.AreEqual(correctRows, rows);
    }
  }
}
