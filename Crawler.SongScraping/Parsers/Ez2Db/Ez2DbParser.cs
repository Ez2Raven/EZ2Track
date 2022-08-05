using System.Collections.Generic;
using Crawler.SongScraping.Parsers.Exceptions;
using Gaming.Domain.Aggregates.GameAggregate;
using Gaming.Domain.Aggregates.GameTrackAggregate;
using Gaming.Domain.Aggregates.MusicAggregate;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace Crawler.SongScraping.Parsers.Ez2Db;

public class Ez2DbParser : IMusicGameParser
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


    public List<IGameTrack> ParseGameTracks(HtmlNodeCollection rowsOfSongNodes)
    {
        var ez2OnGameTracks = new List<IGameTrack>();
        foreach (var songNode in rowsOfSongNodes)
        {
            ez2OnGameTracks.AddRange(ParseGameTracksFromSingleSong(songNode));
        }

        return ez2OnGameTracks;
    }

    public Game ParseGameInfo(HtmlNode songNode)
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

    public DifficultyMode ParseDifficultyMode(HtmlNode songNode, string xPathToDifficultyLevel,
        DifficultyCategory category)
    {
        var difficultyMode = new DifficultyMode();
        var difficultLevelNode = songNode.SelectSingleNode(xPathToDifficultyLevel);
        difficultyMode.Level = int.TryParse(difficultLevelNode?.InnerText, out var difficulty) ? difficulty : 0;
        difficultyMode.Category = category;

        if (difficultyMode.Level == 0)
        {
            throw new ParserException("Game track difficulty level cannot be zero");
        }

        return difficultyMode;
    }

    public Song ParseSongInfo(HtmlNode songNode)
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

    private IEnumerable<Ez2DbGameTrack> ParseGameTracksFromSingleSong(HtmlNode songNode)
    {
        var songSpecificGameTracks = new List<Ez2DbGameTrack>();
        var game = ParseGameInfo(songNode);

        if (game.Id != 0)
        {
            var song = ParseSongInfo(songNode);

            var ez2OnDbSequenceNumber =
                int.TryParse(songNode.SelectSingleNode(XPathToSequenceNumber)?.InnerText, out var seqNum)
                    ? seqNum
                    : 0;

            var thumbnailUrl = songNode.SelectSingleNode(XPathToThumbnail)
                .GetAttributeValue("src", string.Empty);

            var ezMode =
                ParseDifficultyMode(songNode, XPathToEzDifficultyLevel, DifficultyCategory.Easy);

            songSpecificGameTracks.Add(new Ez2DbGameTrack(song, game, ezMode)
            {
                Ez2OnDbSequenceNumber = ez2OnDbSequenceNumber, ThumbnailUrl = thumbnailUrl
            });

            var nmMode =
                ParseDifficultyMode(songNode, XPathToNmDifficultyLevel, DifficultyCategory.Normal);

            songSpecificGameTracks.Add(new Ez2DbGameTrack(song, game, nmMode)
            {
                Ez2OnDbSequenceNumber = ez2OnDbSequenceNumber, ThumbnailUrl = thumbnailUrl
            });

            var hdMode =
                ParseDifficultyMode(songNode, XPathToHdDifficultyLevel, DifficultyCategory.Hard);

            songSpecificGameTracks.Add(new Ez2DbGameTrack(song, game, hdMode)
            {
                Ez2OnDbSequenceNumber = ez2OnDbSequenceNumber, ThumbnailUrl = thumbnailUrl
            });

            var shdMode =
                ParseDifficultyMode(songNode, XPathToShdDifficultyLevel, DifficultyCategory.SuperHard);

            songSpecificGameTracks.Add(new Ez2DbGameTrack(song, game, shdMode)
            {
                Ez2OnDbSequenceNumber = ez2OnDbSequenceNumber, ThumbnailUrl = thumbnailUrl
            });
        }

        return songSpecificGameTracks;
    }
}
