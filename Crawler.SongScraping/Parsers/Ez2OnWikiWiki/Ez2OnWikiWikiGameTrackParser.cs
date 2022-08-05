// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using Crawler.SongScraping.Parsers.Exceptions;
using Gaming.Domain.Aggregates.GameAggregate;
using Gaming.Domain.Aggregates.GameTrackAggregate;
using Gaming.Domain.Aggregates.MusicAggregate;
using Gaming.Domain.Ez2on;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace Crawler.SongScraping.Parsers.Ez2OnWikiWiki;

public class Ez2OnWikiWikiGameTrackParser : IMusicGameParser
{
    private readonly ILogger<Ez2OnWikiWikiGameTrackParser> _logger;

    public Ez2OnWikiWikiGameTrackParser(ILogger<Ez2OnWikiWikiGameTrackParser> logger)
    {
        _logger = logger;
    }

    private string XPathToAlbum { get; } = "td[1]";
    private string XPathToSongTitle { get; } = "td[2]";
    private string XPathToEzDifficultyLevel { get; } = "td[3]";
    private string XPathToNmDifficultyLevel { get; } = "td[4]";
    private string XPathToHdDifficultyLevel { get; } = "td[5]";
    private string XPathToShdDifficultyLevel { get; } = "td[6]";

    public List<IGameTrack> Process(HtmlNodeCollection rowsOfSongNodes)
    {
        var ez2OnGameTracks = new List<IGameTrack>();
        foreach (var songNode in rowsOfSongNodes)
        {
            ez2OnGameTracks.AddRange(ParseGameTracksFromSongNode(songNode));
        }

        return ez2OnGameTracks;
    }

    /// <summary>
    ///     Throws exception if the Game Release cannot be determined
    /// </summary>
    /// <param name="songNode"></param>
    /// <returns></returns>
    /// <exception cref="ParserException"></exception>
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
            case "3RD":
                ez2djGame = new Game {Title = Ez2OnReleaseTitle.ThirdTrax.Name, IsDlc = false};
                break;
            case "4TH":
                ez2djGame = new Game {Title = Ez2OnReleaseTitle.FourthTrax.Name, IsDlc = false};
                break;
            case "PT":
                ez2djGame = new Game {Title = Ez2OnReleaseTitle.Platinum.Name, IsDlc = false};
                break;
            case "6TH":
                ez2djGame = new Game {Title = Ez2OnReleaseTitle.SixthTrax.Name, IsDlc = false};
                break;
            case "7TH":
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
            case "TT":
                ez2djGame = new Game {Title = Ez2OnReleaseTitle.TimeTraveler.Name, IsDlc = true};
                break;
            case "CV":
                ez2djGame = new Game {Title = Ez2OnReleaseTitle.CodeNameViolet.Name, IsDlc = true};
                break;
            case "PP":
                ez2djGame = new Game {Title = Ez2OnReleaseTitle.PrestigePass.Name, IsDlc = true};
                break;
            case "02":
                ez2djGame = new Game {Title = Ez2OnReleaseTitle.O2Jam.Name, IsDlc = true};
                break;
            default:
                throw new ParserException("Unrecognized EZ2ON Reboot: R release title.");
        }

        return ez2djGame;
    }

    public DifficultyMode ParseDifficultyMode(HtmlNode songNode, string xPathToDifficultyLevel,
        DifficultyCategory category)
    {
        throw new NotImplementedException();
    }

    public Song ParseSongInfo(HtmlNode songNode)
    {
        var song = new Song {Title = songNode.SelectSingleNode(XPathToSongTitle).FirstChild.InnerText.Trim()};

        return song;
    }

    private IEnumerable<GameTrack> ParseGameTracksFromSongNode(HtmlNode songNode)
    {
        var ganeTracks = new List<GameTrack>();
        try
        {
            var game = ParseGameInfo(songNode);
            var song = ParseSongInfo(songNode);
            song.Album = game.Title;

            var ezMode =
                ParseDifficultyMode(songNode, XPathToEzDifficultyLevel, DifficultyCategory.Easy);

            ganeTracks.Add(new GameTrack(song, game, ezMode));

            var nmMode =
                ParseDifficultyMode(songNode, XPathToNmDifficultyLevel, DifficultyCategory.Normal);

            ganeTracks.Add(new GameTrack(song, game, nmMode));

            var hdMode =
                ParseDifficultyMode(songNode, XPathToHdDifficultyLevel, DifficultyCategory.Hard);

            ganeTracks.Add(new GameTrack(song, game, hdMode));

            var shdMode =
                ParseDifficultyMode(songNode, XPathToShdDifficultyLevel, DifficultyCategory.SuperHard);

            ganeTracks.Add(new GameTrack(song, game, shdMode));
        }
        catch (ParserException parserException)
        {
            _logger.LogWarning(parserException, "Unable to parse Song Node at XPath: {xpath}", songNode.XPath);
            _logger.LogTrace(parserException, "HTML Element: {html}", songNode.WriteTo());
        }

        return ganeTracks;
    }
}
