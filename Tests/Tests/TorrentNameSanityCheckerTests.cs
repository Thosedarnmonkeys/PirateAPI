using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PirateAPI.SanityCheckers;
using PirateAPITests.Tests.StubClasses;

namespace Tests.Tests
{
  [TestFixture]
  public class TorrentNameSanityCheckerTests
  {
    [Test]
    public void TestNoSeasonNoEpisodeCorrectName()
    {
      string showName = "Rick and Morty";
      string torTitle = "Rick and Morty Season 2 [WEBRIP] [1080p] [HEVC]";

      TorrentNameSanityChecker checker = new TorrentNameSanityChecker(new StubLogger());

      bool answer = checker.Check(showName, torTitle);

      Assert.IsTrue(answer);
    }

    [Test]
    public void TestNoSeasonNoEpisodeCorrectNameWithAmpersand()
    {
      string showName = "Rick and Morty";
      string torTitle = "Rick & Morty Season 2 [WEBRIP] [1080p] [HEVC]";

      TorrentNameSanityChecker checker = new TorrentNameSanityChecker(new StubLogger());

      bool answer = checker.Check(showName, torTitle);

      Assert.IsTrue(answer);
    }


    [Test]
    public void TestNoSeasonNoEpisodeWrongName()
    {
      string showName = "Rick and Morty";
      string torTitle = "Arrow.S04E23.HDTV.x264-LOL[ettv]";

      TorrentNameSanityChecker checker = new TorrentNameSanityChecker(new StubLogger());

      bool answer = checker.Check(showName, torTitle);

      Assert.IsFalse(answer);
    }

    [Test]
    public void TestNoSeasonNoEpisodeWhitespaceShowName()
    {
      string showName = " ";
      string torTitle = "Arrow.S04E23.HDTV.x264-LOL[ettv]";

      TorrentNameSanityChecker checker = new TorrentNameSanityChecker(new StubLogger());

      bool answer = checker.Check(showName, torTitle);

      Assert.IsFalse(answer);
    }

    [Test]
    public void TestNoSeasonNoEpisodeWhitespaceTorrentName()
    {
      string showName = "Rick and Morty";
      string torTitle = " ";

      TorrentNameSanityChecker checker = new TorrentNameSanityChecker(new StubLogger());

      bool answer = checker.Check(showName, torTitle);

      Assert.IsFalse(answer);
    }

    [Test]
    public void TestNoSeasonNoEpisodeMisspelledName()
    {
      string showName = "Rick and Morty";
      string torTitle = "Rick and Murty Season 2 [WEBRIP] [1080p] [HEVC]";

      TorrentNameSanityChecker checker = new TorrentNameSanityChecker(new StubLogger());

      bool answer = checker.Check(showName, torTitle);

      Assert.IsFalse(answer);
    }

    [Test]
    public void TestSingleSeasonCorrectName()
    {
      string showName = "Rick and Morty";
      int season = 2;
      string torTitle = "Rick and Morty Season 2 [WEBRIP] [1080p] [HEVC]";

      TorrentNameSanityChecker checker = new TorrentNameSanityChecker(new StubLogger());

      bool answer = checker.Check(showName, season, torTitle);

      Assert.IsTrue(answer);
    }

    [Test]
    public void TestSingleSeasonCorrectNameBracketsTouchSeason()
    {
      string showName = "Rick and Morty";
      int season = 2;
      string torTitle = "Rick and Morty Season 2[WEBRIP] [1080p] [HEVC]";

      TorrentNameSanityChecker checker = new TorrentNameSanityChecker(new StubLogger());

      bool answer = checker.Check(showName, season, torTitle);

      Assert.IsTrue(answer);
    }


    [Test]
    public void TestSingleSeasonWrongName()
    {
      string showName = "Rick and Morty";
      int season = 2;
      string torTitle = "Rick and Morty Season 1 [UNCENSORED] [BDRip] [1080p] [HEVC]";

      TorrentNameSanityChecker checker = new TorrentNameSanityChecker(new StubLogger());

      bool answer = checker.Check(showName, season, torTitle);

      Assert.IsFalse(answer);
    }

    [Test]
    public void TestSingleSeasonCorrectNameWithPunctuation()
    {
      string showName = "Rick and Morty";
      int season = 2;
      string torTitle = "Rick.and.Morty.Season.2.1080p.BluRay.x264.with.commentary.tracks";

      TorrentNameSanityChecker checker = new TorrentNameSanityChecker(new StubLogger());

      bool answer = checker.Check(showName, season, torTitle);

      Assert.IsTrue(answer);
    }

    [Test]
    public void TestSingleSeasonCorrectNameAbbrev()
    {
      string showName = "Rick and Morty";
      int season = 2;
      string torTitle = "Rick and Morty S2 [WEBRIP] [1080p] [HEVC]";

      TorrentNameSanityChecker checker = new TorrentNameSanityChecker(new StubLogger());

      bool answer = checker.Check(showName, season, torTitle);

      Assert.IsTrue(answer);
    }

    [Test]
    public void TestSingleSeasonCorrectNameAbbrevWithZero()
    {
      string showName = "Rick and Morty";
      int season = 2;
      string torTitle = "Rick and Morty S02 [WEBRIP] [1080p] [HEVC]";

      TorrentNameSanityChecker checker = new TorrentNameSanityChecker(new StubLogger());

      bool answer = checker.Check(showName, season, torTitle);

      Assert.IsTrue(answer);
    }

    [Test]
    public void TestSingleSeasonCorrectNameVerboseWithZero()
    {
      string showName = "Rick and Morty";
      int season = 2;
      string torTitle = "Rick and Morty Season 02 [WEBRIP] [1080p] [HEVC]";

      TorrentNameSanityChecker checker = new TorrentNameSanityChecker(new StubLogger());

      bool answer = checker.Check(showName, season, torTitle);

      Assert.IsTrue(answer);
    }

    [Test]
    public void TestSingleSeasonWrongNameVerboseDoesntPass()
    {
      string showName = "Rick and Morty";
      int season = 2;
      string torTitle = "Rick and Morty Season 2 Episode 4";

      TorrentNameSanityChecker checker = new TorrentNameSanityChecker(new StubLogger());

      bool answer = checker.Check(showName, season, torTitle);

      Assert.IsFalse(answer);
    }



    [Test]
    public void TestSingleEpisodeCorrectName()
    {
      string showName = "Rick and Morty";
      int season = 2;
      int episode = 1;
      string torTitle = "Rick.and.Morty.S02E01.HDTV.x264-BATV[ettv]";

      TorrentNameSanityChecker checker = new TorrentNameSanityChecker(new StubLogger());

      bool answer = checker.Check(showName, season, episode, torTitle);

      Assert.IsTrue(answer);
    }

    [Test]
    public void TestSingleEpisodeWrongName()
    {
      string showName = "Rick and Morty";
      int season = 2;
      int episode = 1;
      string torTitle = "Rick.and.Morty.S02E03.HDTV.x264-BATV";

      TorrentNameSanityChecker checker = new TorrentNameSanityChecker(new StubLogger());

      bool answer = checker.Check(showName, season, episode, torTitle);

      Assert.IsFalse(answer);
    }

    [Test]
    public void TestSingleEpisodeCorrectNameWrongFormat()
    {
      string showName = "Rick and Morty";
      int season = 2;
      int episode = 1;
      string torTitle = "Rick and Morty Season 2 Episode 1";

      TorrentNameSanityChecker checker = new TorrentNameSanityChecker(new StubLogger());

      bool answer = checker.Check(showName, season, episode, torTitle);

      Assert.IsTrue(answer);
    }

    [Test]
    public void TestSingleEpisodeWrongNameWrongFormat()
    {
      string showName = "Rick and Morty";
      int season = 2;
      int episode = 1;
      string torTitle = "Rick and Morty Season 1 Episode 1";

      TorrentNameSanityChecker checker = new TorrentNameSanityChecker(new StubLogger());

      bool answer = checker.Check(showName, season, episode, torTitle);

      Assert.IsFalse(answer);
    }

    [Test]
    public void TestSingleEpisodeCorrectNameSpaceInMiddle()
    {
      string showName = "Rick and Morty";
      int season = 2;
      int episode = 1;
      string torTitle = "Rick.and.Morty.S02 E01.HDTV.x264-BATV[ettv]";

      TorrentNameSanityChecker checker = new TorrentNameSanityChecker(new StubLogger());

      bool answer = checker.Check(showName, season, episode, torTitle);

      Assert.IsTrue(answer);
    }

    [Test]
    public void TestSingleEpisodeCorrectNameNoExtraZeros()
    {
      string showName = "Rick and Morty";
      int season = 2;
      int episode = 1;
      string torTitle = "Rick.and.Morty.S2E1.HDTV.x264-BATV[ettv]";

      TorrentNameSanityChecker checker = new TorrentNameSanityChecker(new StubLogger());

      bool answer = checker.Check(showName, season, episode, torTitle);

      Assert.IsTrue(answer);
    }
  }
}
