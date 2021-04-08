using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using MusicGames.Domain.Models;
using MusicGames.Domain.Validations;

namespace MusicGames.SongScraping.Parsers
{
    public class Ez2OnParser
    {
        private readonly ILogger<Ez2OnParser> _logger;

        public Ez2OnParser(ILogger<Ez2OnParser> logger)
        {
            _logger = logger;
        }

        public string XPathToSequenceNumber { get; set; } = "td[1]";
        public string XPathToAlbum { get; set; } = "td[2]";
        public string XPathToThumbnail { get; set; } = "td[3]/img";
        public string XPathToSongTitle { get; set; } = "td[4]/a[1]";
        public string XPathToSongRemixTag { get; set; } = "td[4]/a/span[@class='remix']";
        public string XPathToSongComposer { get; set; } = "td[4]/a/span[@class='songcomposer']";
        public string XPathToEzDifficultyLevel { get; set; } = "td[5]/a";
        public string XPathToNmDifficultyLevel { get; set; } = "td[6]/a";
        public string XPathToHdDifficultyLevel { get; set; } = "td[7]/a";
        public string XPathToShdDifficultyLevel { get; set; } = "td[8]/a";
        public string XPathToSongBpm { get; set; } = "td[9]";


        public List<Ez2OnGameTrack> AggregateGameTracksForAllSongs(HtmlNodeCollection songNodes)
        {
            List<Ez2OnGameTrack> ez2OnGameTracks = new List<Ez2OnGameTrack>();
            foreach (var songNode in songNodes)
            {
                ez2OnGameTracks.AddRange(ParseGameTracksFromSingleSong(songNode));
            }

            return ez2OnGameTracks;
        }

        private List<Ez2OnGameTrack> ParseGameTracksFromSingleSong(HtmlNode songNode)
        {
            List<Ez2OnGameTrack> songSpecificGameTracks = new List<Ez2OnGameTrack>();
            var ez2djGame = InferGameFromSongAlbum(songNode);

            var song = ParseSongFromHtmlNode(songNode);

            var ez2OnDbSequenceNumber = Convert.ToInt32(songNode.SelectSingleNode(XPathToSequenceNumber).InnerText);
            var thumbnailUrl = songNode.SelectSingleNode(XPathToThumbnail)
                .GetAttributeValue("src", string.Empty);

            var ezMode =
                ParseDifficultyModeFromSongNode(songNode, XPathToEzDifficultyLevel, DifficultyCategory.Easy);
            
            if (ezMode.Category != DifficultyCategory.None)
            {
                songSpecificGameTracks.Add(new Ez2OnGameTrack(song, ez2djGame, ezMode)
                {
                    Ez2OnDbSequenceNumber = ez2OnDbSequenceNumber,
                    ThumbnailUrl = thumbnailUrl
                });
            }
            
            var nmMode =
                ParseDifficultyModeFromSongNode(songNode, XPathToNmDifficultyLevel, DifficultyCategory.Normal);
            
            if (nmMode.Category != DifficultyCategory.None)
            {
                songSpecificGameTracks.Add(new Ez2OnGameTrack(song, ez2djGame, nmMode)
                {
                    Ez2OnDbSequenceNumber = ez2OnDbSequenceNumber,
                    ThumbnailUrl = thumbnailUrl
                });
            }
            
            var hdMode =
                ParseDifficultyModeFromSongNode(songNode, XPathToHdDifficultyLevel, DifficultyCategory.Hard);
            
            if (hdMode.Category != DifficultyCategory.None)
            {
                songSpecificGameTracks.Add(new Ez2OnGameTrack(song, ez2djGame, hdMode)
                {
                    Ez2OnDbSequenceNumber = ez2OnDbSequenceNumber,
                    ThumbnailUrl = thumbnailUrl
                });
            }
            
            var shdMode =
                ParseDifficultyModeFromSongNode(songNode, XPathToShdDifficultyLevel, DifficultyCategory.SuperHard);
            
            if (shdMode.Category != DifficultyCategory.None)
            {
                songSpecificGameTracks.Add(new Ez2OnGameTrack(song, ez2djGame, shdMode)
                {
                    Ez2OnDbSequenceNumber = ez2OnDbSequenceNumber,
                    ThumbnailUrl = thumbnailUrl
                });
            }

            return songSpecificGameTracks;
        }

        public Game InferGameFromSongAlbum(HtmlNode songNode)
        {
            var album = songNode.SelectSingleNode(XPathToAlbum).InnerText.Trim();
            Game ez2djGame;
            switch (album.ToUpper())
            {
                case "1ST TRACKS":
                case "2ND TRAX":
                case "3RD TRAX":
                case "4TH TRAX":
                case "PLATINUM":
                case "6TH TRAX":
                case "7TH TRAX":
                    ez2djGame = new Game()
                    {
                        Title = $"EZ2DJ {album.ToUpper()}",
                        IsDlc = false
                    };
                    break;
                case "S/E":
                    ez2djGame = new Game()
                    {
                        Title = $"EZ2DJ Special Edition",
                        IsDlc = false
                    };
                    break;
                case "2008":
                case "2013":
                case "2021":
                    ez2djGame = new Game()
                    {
                        Title = $"EZ2ON {album.ToUpper()}",
                        IsDlc = false
                    };
                    break;
                default:
                    ez2djGame = new Game()
                    {
                        Title = $"{album.ToUpper()}",
                        IsDlc = true
                    };
                    break;
            }

            return ez2djGame;
        }

        public DifficultyMode ParseDifficultyModeFromSongNode(HtmlNode songNode, string xPathToDifficultyLevel, DifficultyCategory category)
        {
            DifficultyMode difficultyMode = new DifficultyMode();
            var difficultLevelNode = songNode.SelectSingleNode(xPathToDifficultyLevel);
            if (difficultLevelNode != null)
            {
                difficultyMode.Level = int.Parse(difficultLevelNode.InnerText);
                difficultyMode.Category = category;

            }

            if (difficultyMode.Level == 0)
            {
                _logger.LogWarning("Unable to parse DifficultyMode Level. " +
                                   "Consider verifying song difficult level from source");
            }

            return difficultyMode;
        }

        public Song ParseSongFromHtmlNode(HtmlNode songNode)
        {
            var remixNode = songNode.SelectSingleNode(XPathToSongRemixTag);
            string remix = remixNode?.InnerText.Trim();

            var song = new Song()
            {
                Album = songNode.SelectSingleNode(XPathToAlbum).FirstChild.InnerText.Trim(),
                Title = remix == null
                    ? songNode.SelectSingleNode(XPathToSongTitle).FirstChild.InnerText.Trim()
                    : songNode.SelectSingleNode(XPathToSongTitle).FirstChild.InnerText.Trim() + $" {remix}",
                Composer = songNode.SelectSingleNode(XPathToSongComposer).InnerText.Trim(),
                Bpm = songNode.SelectSingleNode(XPathToSongBpm).InnerText.Trim()
            };
            return song;
        }
    }
}