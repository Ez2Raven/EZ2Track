using CleanCode.Patterns.DataStructures;
using Crawler.SongScraping.Parsers.Ez2OnWiki;
using FluentAssertions;
using Gaming.Domain.Aggregates.GameAggregate;
using Gaming.Domain.Aggregates.GameTrackAggregate;
using Gaming.Domain.Aggregates.GameTrackAggregate.Ez2on;
using Gaming.Domain.Aggregates.MusicAggregate;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Crawler.SongScraping.Test;

public class Ez2OnWikiWikiTest
{
    private readonly ITestOutputHelper _output;

    public Ez2OnWikiWikiTest(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void Can_Load_DynamicHTML()
    {
        var url = "https://wikiwiki.jp/ez2on/LevelList/List";
        var web1 = new HtmlWeb();
        var loadUrlTask = web1.LoadFromWebAsync(url);
        var htmlDoc = loadUrlTask.Result;
        var xpath =
            "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr";
        var ez2OnContentTable = htmlDoc.DocumentNode.SelectSingleNode(xpath);
        ez2OnContentTable.Should().NotBeNull();
    }

    [Fact]
    public void Can_ParseDynamicHTML_As_GameTracks()
    {
        var url = "https://wikiwiki.jp/ez2on/LevelList/List";
        var mockLogger = new Mock<ILogger<Ez2OnWikiWikiGameTrackParser>>();
        var parser = new Ez2OnWikiWikiGameTrackParser(mockLogger.Object);
        var ez2OnGameTracks = parser.ProcessGameTracks(url);
        ez2OnGameTracks.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public void CanParseSongListUrlAsSongs()
    {
        var url = "https://wikiwiki.jp/ez2on/SongList/List";
        var mockLogger = new Mock<ILogger<Ez2OnWikiWikiGameTrackParser>>();
        var parser = new Ez2OnWikiWikiGameTrackParser(mockLogger.Object);
        var songList = parser.ProcessSongs(url);
        songList.Count.Should().BeGreaterThan(0);
        foreach (var song in songList)
        {
            _output.WriteLine(song.ToString());
        }
    }


    [Theory]
    [InlineData(
        "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr[6]",
        "8th Planet (Progressive House Remix)", "", "", "EZ2ON Reboot: R - Time Traveler", "")]
    [InlineData(
        "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr[325]",
        "風の翼 (Wing of the Wind)", "", "", "EZ2ON Reboot: R - O2Jam Collaboration DLC", "")]
    public void Can_ParseSongNode_As_ValidSongTitle(string xPath, string songTitle, string composer, string bpm,
        string album, string genre)
    {
        var doc = new HtmlDocument();
        doc.Load("Ez2OnWikiWikiSample.html");
        var songNode = doc.DocumentNode.SelectSingleNode(xPath);
        var mockLogger = new Mock<ILogger<Ez2OnWikiWikiGameTrackParser>>();
        var parser = new Ez2OnWikiWikiGameTrackParser(mockLogger.Object);
        // ez2onWikiWiki song album is inferred from game title
        var actualSong = parser.ParseSongInfoFromGameTrack(songNode, album);
        Assert.Equal(album, actualSong.Album);
        Assert.Equal(songTitle, actualSong.Title);
        Assert.Equal(composer, actualSong.Composer);
        Assert.Equal(bpm, actualSong.Bpm);
        Assert.Equal(genre, actualSong.Genre);
        var songValidator = new SongValidator();
        var validationResult = songValidator.Validate(actualSong);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                _output.WriteLine(error.ToString());
            }
        }

        validationResult.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(
        "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr[25]",
        "EZ2DJ 1ST TRACKS", false)]
    [InlineData(
        "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr[57]",
        "EZ2DJ Special Edition", false)]
    [InlineData(
        "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr[28]",
        "EZ2DJ 2ND TRAX", false)]
    [InlineData(
        "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr[1]",
        "EZ2DJ 3RD TRAX", false)]
    [InlineData(
        "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr[18]",
        "EZ2DJ 4TH TRAX", false)]
    [InlineData(
        "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr[13]",
        "EZ2DJ PLATINUM", false)]
    [InlineData(
        "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr[26]",
        "EZ2DJ 6TH TRAX", false)]
    [InlineData(
        "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr[11]",
        "EZ2DJ 7TH TRAX", false)]
    [InlineData(
        "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr[41]",
        "EZ2ON Reboot: R - 2008", false)]
    [InlineData(
        "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr[20]",
        "EZ2ON Reboot: R - 2013", false)]
    [InlineData(
        "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr[4]",
        "EZ2ON Reboot: R - 2021", false)]
    [InlineData(
        "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr[6]",
        "EZ2ON Reboot: R - Time Traveler", true)]
    [InlineData(
        "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr[7]",
        "EZ2ON Reboot: R - Prestige Pass", true)]
    [InlineData(
        "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr[15]",
        "EZ2ON Reboot: R - CodeName Violet", true)]
    [InlineData(
        "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr[39]",
        "EZ2ON Reboot: R - O2Jam Collaboration DLC", true)]
    public void Can_Parse_GameTitles(string xPathToEz2DjSongNode, string gameTitle, bool isDlc)
    {
        var doc = new HtmlDocument();
        doc.Load("Ez2OnWikiWiki-GameTracks-Sample.html");
        var songNode = doc.DocumentNode.SelectSingleNode(xPathToEz2DjSongNode);
        var mockLogger = new Mock<ILogger<Ez2OnWikiWikiGameTrackParser>>();
        var parser = new Ez2OnWikiWikiGameTrackParser(mockLogger.Object);
        var game = parser.ParseGameInfo(songNode);
        Assert.Equal(gameTitle, game.Title);
        Assert.Equal(isDlc, game.IsDlc);
        var gameValidator = new GameValidator();
        var validationResult = gameValidator.Validate(game);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                _output.WriteLine(error.ToString());
            }
        }

        Assert.True(validationResult.IsValid);
    }

    [Theory]
    [InlineData(
        "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr[39]",
        "td[3]", 5, "Easy", "4K")]
    [InlineData(
        "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr[39]",
        "td[4]", 11, "Normal", "4K")]
    [InlineData(
        "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr[39]",
        "td[5]", 15, "Hard", "4K")]
    [InlineData(
        "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr[39]",
        "td[6]", 19, "SHD", "4K")]
    [InlineData(
        "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr[39]",
        "td[7]", 6, "Easy", "5K")]
    [InlineData(
        "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr[39]",
        "td[8]", 12, "Normal", "5K")]
    [InlineData(
        "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr[39]",
        "td[9]", 16, "Hard", "5K")]
    [InlineData(
        "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr[39]",
        "td[10]", 20, "SHD", "5K")]
    [InlineData(
        "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr[39]",
        "td[11]", 6, "Easy", "6K")]
    [InlineData(
        "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr[39]",
        "td[12]", 12, "Normal", "6K")]
    [InlineData(
        "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr[39]",
        "td[13]", 17, "Hard", "6K")]
    [InlineData(
        "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr[39]",
        "td[14]", 20, "SHD", "6K")]
    [InlineData(
        "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr[39]",
        "td[15]", 9, "Easy", "8K")]
    [InlineData(
        "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr[39]",
        "td[16]", 14, "Normal", "8K")]
    [InlineData(
        "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr[39]",
        "td[17]", 17, "Hard", "8K")]
    [InlineData(
        "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr[39]",
        "td[18]", 20, "SHD", "8K")]
    public void Can_ParseSongDifficultModes(string xPathToSong, string xPathToDifficultMode, int difficultyLevel,
        string difficultyCategory, string keyMode)
    {
        var doc = new HtmlDocument();
        doc.Load("Ez2OnWikiWiki-GameTracks-Sample.html");
        var songNode = doc.DocumentNode.SelectSingleNode(xPathToSong);
        var mockLogger = new Mock<ILogger<Ez2OnWikiWikiGameTrackParser>>();
        var parser = new Ez2OnWikiWikiGameTrackParser(mockLogger.Object);
        var parsedMode =
            parser.ParseDifficultyMode(songNode, xPathToDifficultMode,
                Enumeration.FromDisplayName<DifficultyCategory>(difficultyCategory),
                Enumeration.FromDisplayName<Ez2OnKeyModes>(keyMode));
        Assert.Equal(difficultyLevel, parsedMode.Level);
        Assert.Equal(difficultyCategory, parsedMode.Category.Name);
        Assert.Equal(keyMode, parsedMode.KeyMode.Name);
    }
}
