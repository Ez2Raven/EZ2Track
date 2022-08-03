using System.Collections.Generic;
using Gaming.Domain.Aggregates.GameAggregate;
using Gaming.Domain.Aggregates.GameTrackAggregate;
using Gaming.Domain.Aggregates.MusicAggregate;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace Crawler.SongScraping.Parsers;

public class Ez2DbParser
{
    private readonly ILogger<Ez2DbParser> _logger;

    public Ez2DbParser(ILogger<Ez2DbParser> logger)
    {
        _logger = logger;
    }

    private string XPathToSequenceNumber { get; } = "td[1]";
    private string XPathToAlbum { get; } = "td[2]";
    private string XPathToThumbnail { get; } = "td[3]/img";
    private string XPathToSongTitle { get; } = "td[4]/a[1]";
    private string XPathToSongRemixTag { get; } = "td[4]/a/span[@class='remix']";
    private string XPathToSongComposer { get; } = "td[4]/a/span[@class='songcomposer']";
    private string XPathToEzDifficultyLevel { get; } = "td[5]/a";
    private string XPathToNmDifficultyLevel { get; } = "td[6]/a";
    private string XPathToHdDifficultyLevel { get; } = "td[7]/a";
    private string XPathToShdDifficultyLevel { get; } = "td[8]/a";
    private string XPathToSongBpm { get; } = "td[9]";


    public List<Ez2DbGameTrack> AggregateGameTracksForAllSongs(HtmlNodeCollection songNodes)
    {
        var ez2OnGameTracks = new List<Ez2DbGameTrack>();
        foreach (var songNode in songNodes)
        {
            ez2OnGameTracks.AddRange(ParseGameTracksFromSingleSong(songNode));
        }

        return ez2OnGameTracks;
    }

    private IEnumerable<Ez2DbGameTrack> ParseGameTracksFromSingleSong(HtmlNode songNode)
    {
        var songSpecificGameTracks = new List<Ez2DbGameTrack>();
        var game = InferGameFromSongAlbum(songNode);

        if (game.Id != 0)
        {
            var song = ParseSongFromHtmlNode(songNode);

            var ez2OnDbSequenceNumber =
                int.TryParse(songNode.SelectSingleNode(XPathToSequenceNumber)?.InnerText, out var seqNum)
                    ? seqNum
                    : 0;

            var thumbnailUrl = songNode.SelectSingleNode(XPathToThumbnail)
                .GetAttributeValue("src", string.Empty);

            var ezMode =
                ParseDifficultyModeFromSongNode(songNode, XPathToEzDifficultyLevel, DifficultyCategory.Easy);

            if (ezMode.Category != DifficultyCategory.None)
            {
                songSpecificGameTracks.Add(new Ez2DbGameTrack(song, game, ezMode)
                {
                    Ez2OnDbSequenceNumber = ez2OnDbSequenceNumber, ThumbnailUrl = thumbnailUrl
                });
            }

            var nmMode =
                ParseDifficultyModeFromSongNode(songNode, XPathToNmDifficultyLevel, DifficultyCategory.Normal);

            if (nmMode.Category != DifficultyCategory.None)
            {
                songSpecificGameTracks.Add(new Ez2DbGameTrack(song, game, nmMode)
                {
                    Ez2OnDbSequenceNumber = ez2OnDbSequenceNumber, ThumbnailUrl = thumbnailUrl
                });
            }

            var hdMode =
                ParseDifficultyModeFromSongNode(songNode, XPathToHdDifficultyLevel, DifficultyCategory.Hard);

            if (hdMode.Category != DifficultyCategory.None)
            {
                songSpecificGameTracks.Add(new Ez2DbGameTrack(song, game, hdMode)
                {
                    Ez2OnDbSequenceNumber = ez2OnDbSequenceNumber, ThumbnailUrl = thumbnailUrl
                });
            }

            var shdMode =
                ParseDifficultyModeFromSongNode(songNode, XPathToShdDifficultyLevel, DifficultyCategory.SuperHard);

            if (shdMode.Category != DifficultyCategory.None)
            {
                songSpecificGameTracks.Add(new Ez2DbGameTrack(song, game, shdMode)
                {
                    Ez2OnDbSequenceNumber = ez2OnDbSequenceNumber, ThumbnailUrl = thumbnailUrl
                });
            }
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
                ez2djGame = new Game {Title = $"EZ2DJ {album.ToUpper()}", IsDlc = false};
                break;
            case "S/E":
                ez2djGame = new Game {Title = "EZ2DJ Special Edition", IsDlc = false};
                break;
            case "2008":
            case "2013":
            case "2021":
                ez2djGame = new Game {Title = $"EZ2ON {album.ToUpper()}", IsDlc = false};
                break;
            default:
                ez2djGame = new Game {Title = $"{album.ToUpper()}", IsDlc = true};
                break;
        }

        return ez2djGame;
    }

    public DifficultyMode ParseDifficultyModeFromSongNode(HtmlNode songNode, string xPathToDifficultyLevel,
        DifficultyCategory category)
    {
        var difficultyMode = new DifficultyMode();
        var difficultLevelNode = songNode.SelectSingleNode(xPathToDifficultyLevel);
        difficultyMode.Level = int.TryParse(difficultLevelNode?.InnerText, out var difficulty) ? difficulty : 0;
        difficultyMode.Category = category;

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
        var remix = remixNode?.InnerText.Trim();

        var song = new Song
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
