using System.Collections.Generic;
using Gaming.Domain.Aggregates.GameAggregate;
using Gaming.Domain.Aggregates.GameTrackAggregate;
using Gaming.Domain.Aggregates.MusicAggregate;
using Gaming.Domain.Ez2on;
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

    public List<IGameTrack> Process()
    {
        var url = "https://ez2on.co.kr/6K/?mode=database&pagelist=218";
        return ProcessGameTracks(url);
    }

    public List<IGameTrack> ProcessGameTracks(string url)
    {
        var web1 = new HtmlWeb();
        var loadUrlTask = web1.LoadFromWebAsync(url);
        var htmlDoc = loadUrlTask.Result;
        var xpath = "/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr";
        var songNodes = htmlDoc.DocumentNode.SelectNodes(xpath);

        var ez2OnGameTracks = new List<IGameTrack>();
        foreach (var songNode in songNodes)
        {
            ez2OnGameTracks.AddRange(ParseGameTracksFromSingleSong(songNode));
        }

        return ez2OnGameTracks;
    }

    public List<ISong> ProcessSongs(string url)
    {
        var web1 = new HtmlWeb();
        var loadUrlTask = web1.LoadFromWebAsync(url);
        var htmlDoc = loadUrlTask.Result;
        var xpath = "/html/body/div[@id='contentmain']/table[@id='EZ2ONContent']/tbody[@id='EZ2DJ_TRACKS']/tr";
        var songNodes = htmlDoc.DocumentNode.SelectNodes(xpath);

        var ez2OnGameTracks = new List<ISong>();
        foreach (var songNode in songNodes)
        {
            ez2OnGameTracks.Add(ParseSongInfo(songNode));
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
                ez2djGame = new Game {Title = Ez2OnReleaseTitle.FirstTrax.Name, IsDlc = false};
                break;
            case "2ND TRAX":
                ez2djGame = new Game {Title = Ez2OnReleaseTitle.SecondTrax.Name, IsDlc = false};
                break;
            case "3RD TRAX":
                ez2djGame = new Game {Title = Ez2OnReleaseTitle.ThirdTrax.Name, IsDlc = false};
                break;
            case "4TH TRAX":
                ez2djGame = new Game {Title = Ez2OnReleaseTitle.FourthTrax.Name, IsDlc = false};
                break;
            case "PLATINUM":
                ez2djGame = new Game {Title = Ez2OnReleaseTitle.Platinum.Name, IsDlc = false};
                break;
            case "6TH TRAX":
                ez2djGame = new Game {Title = Ez2OnReleaseTitle.SixthTrax.Name, IsDlc = false};
                break;
            case "7TH TRAX":
                ez2djGame = new Game {Title = Ez2OnReleaseTitle.SeventhTrax.Name, IsDlc = false};
                break;
            case "S/E":
                ez2djGame = new Game {Title = Ez2OnReleaseTitle.SpecialEdition.Name, IsDlc = false};
                break;
            case "2008":
                ez2djGame = new Game {Title = Ez2OnReleaseTitle.Ez2On2008.Name, IsDlc = false};
                break;
            case "2013":
                ez2djGame = new Game {Title = Ez2OnReleaseTitle.Ez2On2013.Name, IsDlc = false};
                break;
            case "2021":
                ez2djGame = new Game {Title = Ez2OnReleaseTitle.Ez2On2021.Name, IsDlc = false};
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
        return difficultyMode;
    }

    public Song ParseSongInfo(HtmlNode songNode, string gameTitle = "")
    {
        var remixNode = songNode.SelectSingleNode(XPathToSongRemixTag);
        var remix = remixNode?.InnerText.Trim();

        var song = new Song
        {
            Album = string.IsNullOrWhiteSpace(gameTitle)
                ? songNode.SelectSingleNode(XPathToAlbum).FirstChild.InnerText.Trim()
                : gameTitle,
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
        var ganeTracks = new List<Ez2DbGameTrack>();
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

            ganeTracks.Add(new Ez2DbGameTrack(song, game, ezMode)
            {
                Ez2OnDbSequenceNumber = ez2OnDbSequenceNumber, ThumbnailUrl = thumbnailUrl
            });

            var nmMode =
                ParseDifficultyMode(songNode, XPathToNmDifficultyLevel, DifficultyCategory.Normal);

            ganeTracks.Add(new Ez2DbGameTrack(song, game, nmMode)
            {
                Ez2OnDbSequenceNumber = ez2OnDbSequenceNumber, ThumbnailUrl = thumbnailUrl
            });

            var hdMode =
                ParseDifficultyMode(songNode, XPathToHdDifficultyLevel, DifficultyCategory.Hard);

            ganeTracks.Add(new Ez2DbGameTrack(song, game, hdMode)
            {
                Ez2OnDbSequenceNumber = ez2OnDbSequenceNumber, ThumbnailUrl = thumbnailUrl
            });

            var shdMode =
                ParseDifficultyMode(songNode, XPathToShdDifficultyLevel, DifficultyCategory.SuperHard);

            ganeTracks.Add(new Ez2DbGameTrack(song, game, shdMode)
            {
                Ez2OnDbSequenceNumber = ez2OnDbSequenceNumber, ThumbnailUrl = thumbnailUrl
            });
        }

        return ganeTracks;
    }
}
