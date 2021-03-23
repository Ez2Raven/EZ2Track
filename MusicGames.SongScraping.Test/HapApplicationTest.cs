using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using MusicGames.Domain.Models;
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

            // Assert.False(string.IsNullOrWhiteSpace(htmlDoc.ParsedText));
            // _output.WriteLine(htmlDoc.ParsedText);

            var xpath = "/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']";
            var ez2OnContentTable = htmlDoc.DocumentNode.SelectSingleNode(xpath);
            Assert.NotNull(ez2OnContentTable);
        }

        [Fact]
        public void Can_Select_DynamicHTML_As_Songs()
        {
            string url = "https://ez2on.co.kr/6K/?mode=database&pagelist=218";

            var web1 = new HtmlWeb();
            var loadUrlTask = web1.LoadFromWebAsync(url);
            var htmlDoc = loadUrlTask.Result;

            var xpath = "/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr";
            var songNodes = htmlDoc.DocumentNode.SelectNodes(xpath);

            var sequenceNumberXPath = "td[1]";
            var albumXPath = "td[2]";
            var thumbnailXpath = "td[3]/img";
            var songTitleXPath = "td[4]/a[1]";
            var songComposerXPath = "td[4]/a/span[@class='songcomposer']";

            List<Ez2OnSong> listOfSongs = new List<Ez2OnSong>();

            foreach (var songNode in songNodes)
            {
                var album = songNode.SelectSingleNode(albumXPath).InnerText.Trim();
                IGame ez2djGame;
                switch (album.ToUpper())
                {
                    case "1ST TRACKS":
                        ez2djGame = new BaseGame()
                        {
                            Title = $"EZ2DJ {album}"
                        };
                        break;
                    case "S/E":
                        ez2djGame = new BaseGame()
                        {
                            Title = $"EZ2DJ Special Edition"
                        };
                        break;
                    default:
                        ez2djGame = new Dlc();
                        break;
                }

                Song ez2djSong = new Song()
                {
                    Album = songNode.SelectSingleNode(albumXPath).InnerText,
                    Title = songNode.SelectSingleNode(songTitleXPath).FirstChild.InnerText,
                    Composer = songNode.SelectSingleNode(songComposerXPath).InnerText
                };

                Ez2OnSong song = new Ez2OnSong(ez2djSong, ez2djGame)
                {
                    SequenceNumber = Convert.ToInt32(songNode.SelectSingleNode(sequenceNumberXPath).InnerText),
                    ThumbnailUrl = songNode.SelectSingleNode(thumbnailXpath)
                        .GetAttributeValue("src", string.Empty)
                };

                listOfSongs.Add(song);
            }

            Assert.NotNull(listOfSongs);
            Assert.NotEmpty(listOfSongs);
            _output.WriteLine(listOfSongs[0].ToString());
        }
    }

    public class Ez2OnSong : GameTrack
    {
        public int SequenceNumber { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(SequenceNumber)}: {SequenceNumber}";
        }

        public Ez2OnSong(ISong song, IGame game) : base(song, game)
        {
        }
    }
}