using CleanCode.Patterns.DataStructures;
using Crawler.SongScraping.Parsers.Ez2OnWikiWiki;
using FluentAssertions;
using Gaming.Domain.Aggregates.GameAggregate;
using Gaming.Domain.Aggregates.GameTrackAggregate;
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
            "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody";
        var ez2OnContentTable = htmlDoc.DocumentNode.SelectSingleNode(xpath);
        ez2OnContentTable.Should().NotBeNull();
    }

    [Fact]
    public void Can_ParseDynamicHTML_As_GameTracks()
    {
        var url = "https://wikiwiki.jp/ez2on/LevelList/List";

        var web1 = new HtmlWeb();
        var loadUrlTask = web1.LoadFromWebAsync(url);
        var htmlDoc = loadUrlTask.Result;
        var xpath =
            "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody";
        var songNodes = htmlDoc.DocumentNode.SelectNodes(xpath);

        var mockLogger = new Mock<ILogger<Ez2OnWikiWikiGameTrackParser>>();
        var parser = new Ez2OnWikiWikiGameTrackParser(mockLogger.Object);

        var ez2OnGameTracks = parser.Process(songNodes);
        ez2OnGameTracks.Count.Should().BeGreaterThan(0);
    }


    [Theory]
    [InlineData("/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr[2]",
        "Catch The Flow", "Creatune", "97", "1ST TRACKS", "")]
    [InlineData("/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr[127]",
        "Jam (A.C Ver.)", "Itdie", "140", "6TH TRAX", "")]
    public void Can_ParseSongNode_As_ValidSongTitle(string xPath, string songTitle, string composer, string bpm,
        string album, string genre)
    {
        var doc = new HtmlDocument();
        doc.Load("ez2onDBSample.html.txt");
        var songNode = doc.DocumentNode.SelectSingleNode(xPath);
        var mockLogger = new Mock<ILogger<Ez2OnWikiWikiGameTrackParser>>();
        var parser = new Ez2OnWikiWikiGameTrackParser(mockLogger.Object);
        var actualSong = parser.ParseSongInfo(songNode);
        Assert.Equal(songTitle, actualSong.Title);
        Assert.Equal(composer, actualSong.Composer);
        Assert.Equal(bpm, actualSong.Bpm);
        Assert.Equal(album, actualSong.Album);
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
    [InlineData("/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr[6]",
        "Envy Mask (Remaster)", "Creatune, Mario Bolden", "118", "1ST TRACKS", "")]
    [InlineData("/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr[214]",
        "Lovely Day (Remaster)", "Andy Lee", "140", "2021", "")]
    public void Can_ParseRemixedSongNode_As_ValidSongTitle(string xPath, string songTitle, string composer,
        string bpm,
        string album, string genre)
    {
        var doc = new HtmlDocument();
        doc.Load("ez2onDBSample.html.txt");
        var songNode = doc.DocumentNode.SelectSingleNode(xPath);
        var mockLogger = new Mock<ILogger<Ez2OnWikiWikiGameTrackParser>>();
        var parser = new Ez2OnWikiWikiGameTrackParser(mockLogger.Object);
        var actualSong = parser.ParseSongInfo(songNode);
        Assert.Equal(songTitle, actualSong.Title);
        Assert.Equal(composer, actualSong.Composer);
        Assert.Equal(bpm, actualSong.Bpm);
        Assert.Equal(album, actualSong.Album);
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

        Assert.True(validationResult.IsValid);
    }

    [Theory]
    [InlineData("/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr[2]",
        "EZ2DJ 1ST TRACKS")]
    [InlineData("/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr[21]",
        "EZ2DJ Special Edition")]
    [InlineData("/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr[37]",
        "EZ2DJ 2ND TRAX")]
    [InlineData("/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr[62]",
        "EZ2DJ 3RD TRAX")]
    [InlineData("/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr[82]",
        "EZ2DJ 4TH TRAX")]
    [InlineData("/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr[101]",
        "EZ2DJ PLATINUM")]
    [InlineData("/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr[117]",
        "EZ2DJ 6TH TRAX")]
    [InlineData("/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr[140]",
        "EZ2DJ 7TH TRAX")]
    [InlineData("/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr[182]",
        "EZ2ON 2008")]
    [InlineData("/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr[196]",
        "EZ2ON 2013")]
    [InlineData("/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr[207]",
        "EZ2ON 2021")]
    public void Can_ParseSongNode_As_ValidGame(string xPathToEz2DjSongNode, string gameTitle)
    {
        var doc = new HtmlDocument();
        doc.Load("ez2onDBSample.html.txt");
        var songNode = doc.DocumentNode.SelectSingleNode(xPathToEz2DjSongNode);
        var mockLogger = new Mock<ILogger<Ez2OnWikiWikiGameTrackParser>>();
        var parser = new Ez2OnWikiWikiGameTrackParser(mockLogger.Object);
        var game = parser.ParseGameInfo(songNode);
        Assert.Equal(gameTitle, game.Title);
        Assert.False(game.IsDlc);
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
    [InlineData("/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr[1]",
        "td[5]/a", 3, "Easy")]
    [InlineData("/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr[1]",
        "td[6]/a", 5, "Normal")]
    [InlineData("/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr[1]",
        "td[7]/a", 7, "Hard")]
    [InlineData("/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr[1]",
        "td[8]/a", 0, "None")]
    [InlineData("/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr[6]",
        "td[5]/a", 4, "Easy")]
    [InlineData("/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr[6]",
        "td[6]/a", 8, "Normal")]
    [InlineData("/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr[6]",
        "td[7]/a", 11, "Hard")]
    [InlineData("/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr[6]",
        "td[8]/a", 14, "SuperHard")]
    public void Can_ParseSongDifficultModes(string xPathToSong, string xPathToDifficultMode, int difficultyLevel,
        string difficultyCategory)
    {
        var doc = new HtmlDocument();
        doc.Load("ez2onDBSample.html.txt");
        var songNode = doc.DocumentNode.SelectSingleNode(xPathToSong);
        var mockLogger = new Mock<ILogger<Ez2OnWikiWikiGameTrackParser>>();
        var parser = new Ez2OnWikiWikiGameTrackParser(mockLogger.Object);
        var parsedMode =
            parser.ParseDifficultyMode(songNode, xPathToDifficultMode,
                Enumeration.FromDisplayName<DifficultyCategory>(difficultyCategory));
        Assert.Equal(difficultyLevel, parsedMode.Level);
        Assert.Equal(difficultyCategory, parsedMode.Category.Name);
    }
}
