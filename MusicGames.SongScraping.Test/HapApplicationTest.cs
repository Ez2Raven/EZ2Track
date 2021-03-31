using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Moq;
using MusicGames.Domain.Models;
using MusicGames.Domain.Validations;
using MusicGames.SongScraping.Parsers;
using Xunit;
using Xunit.Abstractions;

namespace MusicGames.SongScraping.Test
{
    public class HapApplicationTest
    {
        private readonly ITestOutputHelper _output;

        public HapApplicationTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Can_Load_DynamicHTML()
        {
            string url = "https://ez2on.co.kr/6K/?mode=database&pagelist=218";

            var web1 = new HtmlWeb();
            var loadUrlTask = web1.LoadFromWebAsync(url);
            var htmlDoc = loadUrlTask.Result;
            var xpath = "/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']";
            var ez2OnContentTable = htmlDoc.DocumentNode.SelectSingleNode(xpath);
            Assert.NotNull(ez2OnContentTable);
        }

        [Theory]
        [InlineData("4K", "218", 845)]
        [InlineData("5K", "218", 838)]
        [InlineData("6K", "218", 735)]
        [InlineData("8K", "218", 329)]
        public void Can_ParseDynamicHTML_As_GameTracks(string keyMode, string numOfSongs, int numOfGameTracks)
        {
            string url = $"https://ez2on.co.kr/{keyMode}/?mode=database&pagelist={numOfSongs}";

            var web1 = new HtmlWeb();
            var loadUrlTask = web1.LoadFromWebAsync(url);
            var htmlDoc = loadUrlTask.Result;
            var xpath = "/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr";
            var songNodes = htmlDoc.DocumentNode.SelectNodes(xpath);

            var mockLogger = new Mock<ILogger<Ez2OnParser>>();
            Ez2OnParser parser = new Ez2OnParser(mockLogger.Object);

            List<Ez2OnGameTrack> ez2OnGameTracks = parser.AggregateGameTracksForAllSongs(songNodes);
            Assert.Equal(numOfGameTracks, ez2OnGameTracks.Count);
        }


        [Theory]
        [InlineData("/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr[2]",
            "Catch The Flow", "Creatune", "97", "1ST TRACKS", null)]
        [InlineData("/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr[127]",
            "Jam (A.C Ver.)", "Itdie", "140", "6TH TRAX", null)]
        public void Can_ParseSongNode_As_ValidSongTitle(string xPath, string songTitle, string composer, string bpm,
            string album, string genre)
        {
            var doc = new HtmlDocument();
            doc.Load("ez2onDBSample.html");
            var songNode = doc.DocumentNode.SelectSingleNode(xPath);
            var mockLogger = new Mock<ILogger<Ez2OnParser>>();
            Ez2OnParser parser = new Ez2OnParser(mockLogger.Object);
            Song actualSong = parser.ParseSongFromHtmlNode(songNode);
            Assert.Equal(songTitle, actualSong.Title);
            Assert.Equal(composer, actualSong.Composer);
            Assert.Equal(bpm, actualSong.Bpm);
            Assert.Equal(album, actualSong.Album);
            Assert.Equal(genre, actualSong.Genre);
            SongValidator songValidator = new SongValidator();
            var validationResults = songValidator.Validate(actualSong);
            Assert.True(validationResults.IsValid);
        }

        [Theory]
        [InlineData("/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr[6]",
            "Envy Mask (Remaster)", "Creatune, Mario Bolden", "118", "1ST TRACKS", null)]
        [InlineData("/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr[214]",
            "Lovely Day (Remaster)", "Andy Lee", "140", "2021", null)]
        public void Can_ParseRemixedSongNode_As_ValidSongTitle(string xPath, string songTitle, string composer,
            string bpm,
            string album, string genre)
        {
            var doc = new HtmlDocument();
            doc.Load("ez2onDBSample.html");
            var songNode = doc.DocumentNode.SelectSingleNode(xPath);
            var mockLogger = new Mock<ILogger<Ez2OnParser>>();
            Ez2OnParser parser = new Ez2OnParser(mockLogger.Object);
            Song actualSong = parser.ParseSongFromHtmlNode(songNode);
            Assert.Equal(songTitle, actualSong.Title);
            Assert.Equal(composer, actualSong.Composer);
            Assert.Equal(bpm, actualSong.Bpm);
            Assert.Equal(album, actualSong.Album);
            Assert.Equal(genre, actualSong.Genre);
            SongValidator songValidator = new SongValidator();
            var validationResults = songValidator.Validate(actualSong);
            Assert.True(validationResults.IsValid);
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
            doc.Load("ez2onDBSample.html");
            var songNode = doc.DocumentNode.SelectSingleNode(xPathToEz2DjSongNode);
            var mockLogger = new Mock<ILogger<Ez2OnParser>>();
            Ez2OnParser parser = new Ez2OnParser(mockLogger.Object);
            var game = parser.InferGameFromSongAlbum(songNode);
            Assert.Equal(gameTitle, game.Title);
            Assert.False(game.IsDlc);
            GameValidator gameValidator = new GameValidator();
            var validationResult = gameValidator.Validate(game);
            Assert.True(validationResult.IsValid);
        }

        [Theory]
        [InlineData("/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr[1]",
            "td[5]/a", 3, DifficultyCategory.Easy)]
        [InlineData("/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr[1]",
            "td[6]/a", 5, DifficultyCategory.Normal)]
        [InlineData("/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr[1]",
            "td[7]/a", 7, DifficultyCategory.Hard)]
        [InlineData("/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr[1]",
            "td[8]/a", 0, DifficultyCategory.None)]
        [InlineData("/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr[6]",
            "td[5]/a", 4, DifficultyCategory.Easy)]
        [InlineData("/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr[6]",
            "td[6]/a", 8, DifficultyCategory.Normal)]
        [InlineData("/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr[6]",
            "td[7]/a", 11, DifficultyCategory.Hard)]
        [InlineData("/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr[6]",
            "td[8]/a", 14, DifficultyCategory.SuperHard)]
        public void Can_ParseSongDifficultModes(string xPathToSong, string xPathToDifficultMode, int difficultyLevel,
            DifficultyCategory difficultyCategory)
        {
            var doc = new HtmlDocument();
            doc.Load("ez2onDBSample.html");
            var songNode = doc.DocumentNode.SelectSingleNode(xPathToSong);
            var mockLogger = new Mock<ILogger<Ez2OnParser>>();
            Ez2OnParser parser = new Ez2OnParser(mockLogger.Object);
            DifficultyMode parsedMode =
                parser.ParseDifficultyModeFromSongNode(songNode, xPathToDifficultMode, difficultyCategory);
            Assert.Equal(difficultyLevel, parsedMode.Level);
            Assert.Equal(difficultyCategory, parsedMode.Category);
        }
    }
}