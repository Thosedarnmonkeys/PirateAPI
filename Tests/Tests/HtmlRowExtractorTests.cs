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
    public void TestRealPiratePage5Rows()
    {
      string piratePage = Resources.PiratePageSearchRickAndMorty5Rows;

      HtmlRowExtractor extractor = new HtmlRowExtractor(new StubLogger());

      List<string> rows = extractor.ExtractRows(piratePage);
      List<string> correctRows = Resources.PiratePageSearchRickAndMorty5RowsSplit.Replace(Environment.NewLine, "").Split('#').ToList();

      Assert.AreEqual(correctRows, rows);
    }
  }
}
