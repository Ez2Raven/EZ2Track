// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using Crawler.SongScraping.Parsers.Exceptions;
using Gaming.Domain.Aggregates.GameAggregate;
using Gaming.Domain.Aggregates.GameAggregate.Ez2on;
using Gaming.Domain.Aggregates.GameTrackAggregate;
using Gaming.Domain.Aggregates.GameTrackAggregate.Ez2on;
using Gaming.Domain.Aggregates.MusicAggregate;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace Crawler.SongScraping.Parsers.Ez2OnWikiWiki;

public class Ez2OnWikiWikiGameTrackParser : IRhythmGameParser
{
    private readonly ILogger<Ez2OnWikiWikiGameTrackParser> _logger;

    public Ez2OnWikiWikiGameTrackParser(ILogger<Ez2OnWikiWikiGameTrackParser> logger)
    {
        _logger = logger;
    }

    private string XPathToAlbum { get; } = "td[1]";
    private string XPathToSongTitle { get; } = "td[2]";
    private string XPathTo4KeysEasyLevel { get; } = "td[3]";
    private string XPathTo4KeysNormalLevel { get; } = "td[4]";
    private string XPathTo4KeysHardLevel { get; } = "td[5]";
    private string XPathTo4KeysShdLevel { get; } = "td[6]";
    private string XPathTo5KeysEasyLevel { get; } = "td[7]";
    private string XPathTo5KeysNormalLevel { get; } = "td[8]";
    private string XPathTo5KeysHardLevel { get; } = "td[9]";
    private string XPathTo5KeysShdLevel { get; } = "td[10]";
    private string XPathTo6KeysEasyLevel { get; } = "td[11]";
    private string XPathTo6KeysNormalLevel { get; } = "td[12]";
    private string XPathTo6KeysHardLevel { get; } = "td[13]";
    private string XPathTo6KeysShdLevel { get; } = "td[14]";
    private string XPathTo8KeysEasyLevel { get; } = "td[15]";
    private string XPathTo8KeysNormalLevel { get; } = "td[16]";
    private string XPathTo8KeysHardLevel { get; } = "td[17]";
    private string XPathTo8KeysShdLevel { get; } = "td[18]";

    public string XPathToSongTitleV2 { get; set; } = "td[1]";
    public string XPathToComposer { get; set; } = "td[2]";

    public string XPathToBpm { get; set; } = "td[3]";

    public string XPathToGenre { get; set; } = "td[5]";

    public IList<IGameTrack> Parse()
    {
        throw new NotImplementedException();
    }

    public IGameTrackParser GameTrackParser()
    {
        throw new NotImplementedException();
    }

    public ISongParser SongParser()
    {
        throw new NotImplementedException();
    }

    public IGameParser GameParser()
    {
        throw new NotImplementedException();
    }

    public IList<IGameTrack> Process()
    {
        var url = "https://wikiwiki.jp/ez2on/LevelList/List";
        return ProcessGameTracks(url);
    }

    public IList<IGameTrack> ProcessGameTracks(string url)
    {
        var web1 = new HtmlWeb();
        var loadUrlTask = web1.LoadFromWebAsync(url);
        var htmlDoc = loadUrlTask.Result;
        var xpath =
            "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr";
        var songNodes = htmlDoc.DocumentNode.SelectNodes(xpath);

        var ez2OnGameTracks = new List<IGameTrack>();
        foreach (var songNode in songNodes)
        {
            ez2OnGameTracks.AddRange(ParseGameTracksFromSongNode(songNode));
        }

        return ez2OnGameTracks;
    }

    public IList<ISong> ProcessSongs(string url)
    {
        var web1 = new HtmlWeb();
        var loadUrlTask = web1.LoadFromWebAsync(url);
        var htmlDoc = loadUrlTask.Result;
        var xpath =
            "//*[@id=\"content\"]/div/h3";
        var albumNodes = htmlDoc.DocumentNode.SelectNodes(xpath);
        var songList = ParseSongInfoFromSongListUrl(albumNodes);
        return songList;
    }

    private IList<ISong> ParseSongInfoFromSongListUrl(HtmlNodeCollection albumNodes)
    {
        var songList = new List<ISong>();
        foreach (var albumNode in albumNodes)
        {
            var album = albumNode.InnerText.Trim();
            var songNodes = albumNode.SelectNodes("following-sibling::div[1]/table/tbody/tr");
            if (songNodes == null)
            {
                throw new ParserException("unable to recognise song nodes from song list page");
            }

            foreach (var songNode in songNodes)
            {
                var title = songNode.SelectSingleNode(XPathToSongTitleV2)?.InnerText.Trim() ?? string.Empty;
                var composer = songNode.SelectSingleNode(XPathToComposer)?.InnerText.Trim() ?? string.Empty;
                var genre = songNode.SelectSingleNode(XPathToGenre)?.InnerText.Trim() ?? string.Empty;
                var bpm = songNode.SelectSingleNode(XPathToBpm)?.InnerText.Trim() ?? string.Empty;
                songList.Add(new Song(title, composer, album, genre, bpm));
            }
        }

        return songList;
    }


    /// <summary>
    ///     Throws exception if the Game Release cannot be determined
    /// </summary>
    /// <param name="songNode"></param>
    /// <returns></returns>
    /// <exception cref="ParserException"></exception>
    public Game ParseGameInfo(HtmlNode songNode)
    {
        var album = songNode.SelectSingleNode(XPathToAlbum)?.InnerText.Trim();
        Game ez2djGame;
        switch (album?.ToUpper())
        {
            case "1ST":
                ez2djGame = new Game {Title = Ez2OnReleaseTitle.FirstTrax.Name, IsDlc = false};
                break;
            case "2ND":
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
            case "O2":
                ez2djGame = new Game {Title = Ez2OnReleaseTitle.O2Jam.Name, IsDlc = true};
                break;
            default:
                throw new ParserException("Unrecognized EZ2ON Reboot: R release title.");
        }

        return ez2djGame;
    }

    public Ez2OnDifficultyMode ParseDifficultyMode(HtmlNode songNode, string xPathToDifficultyLevel,
        DifficultyCategory category, Ez2OnKeyModes keyMode)
    {
        var isValidNode = int.TryParse(songNode.SelectSingleNode(xPathToDifficultyLevel)?.InnerText, out var level);
        return isValidNode
            ? new Ez2OnDifficultyMode {Level = level, Category = category, KeyMode = keyMode}
            : throw new ParserException();
    }

    public Song ParseSongInfoFromGameTrack(HtmlNode songNode, string gameTitle = "")
    {
        var title = songNode.SelectSingleNode(XPathToSongTitle) == null
            ? string.Empty
            : songNode.SelectSingleNode(XPathToSongTitle).FirstChild.InnerText.Trim();

        var album = string.IsNullOrWhiteSpace(gameTitle)
            ? songNode.SelectSingleNode(XPathToAlbum) == null
                ? string.Empty
                : songNode.SelectSingleNode(XPathToAlbum).FirstChild.InnerText.Trim()
            : gameTitle;


        return new Song {Title = title, Album = album};
    }

    private IEnumerable<Ez2OnGameTrack> ParseGameTracksFromSongNode(HtmlNode songNode)
    {
        var gameTracks = new List<Ez2OnGameTrack>();
        try
        {
            var game = ParseGameInfo(songNode);
            var song = ParseSongInfoFromGameTrack(songNode);

            gameTracks.Add(new Ez2OnGameTrack(song, game,
                ParseDifficultyMode(songNode, XPathTo4KeysEasyLevel, DifficultyCategory.Easy, Ez2OnKeyModes.FourKeys)));
            gameTracks.Add(new Ez2OnGameTrack(song, game,
                ParseDifficultyMode(songNode, XPathTo4KeysNormalLevel, DifficultyCategory.Normal,
                    Ez2OnKeyModes.FourKeys)));
            gameTracks.Add(new Ez2OnGameTrack(song, game,
                ParseDifficultyMode(songNode, XPathTo4KeysHardLevel, DifficultyCategory.Hard, Ez2OnKeyModes.FourKeys)));
            gameTracks.Add(new Ez2OnGameTrack(song, game,
                ParseDifficultyMode(songNode, XPathTo4KeysShdLevel, DifficultyCategory.SuperHard,
                    Ez2OnKeyModes.FourKeys)));
            gameTracks.Add(new Ez2OnGameTrack(song, game,
                ParseDifficultyMode(songNode, XPathTo5KeysEasyLevel, DifficultyCategory.Easy, Ez2OnKeyModes.FiveKeys)));
            gameTracks.Add(new Ez2OnGameTrack(song, game,
                ParseDifficultyMode(songNode, XPathTo5KeysNormalLevel, DifficultyCategory.Normal,
                    Ez2OnKeyModes.FiveKeys)));
            gameTracks.Add(new Ez2OnGameTrack(song, game,
                ParseDifficultyMode(songNode, XPathTo5KeysHardLevel, DifficultyCategory.Hard, Ez2OnKeyModes.SixKeys)));
            gameTracks.Add(new Ez2OnGameTrack(song, game,
                ParseDifficultyMode(songNode, XPathTo5KeysShdLevel, DifficultyCategory.SuperHard,
                    Ez2OnKeyModes.FiveKeys)));
            gameTracks.Add(new Ez2OnGameTrack(song, game,
                ParseDifficultyMode(songNode, XPathTo6KeysEasyLevel, DifficultyCategory.Easy, Ez2OnKeyModes.SixKeys)));
            gameTracks.Add(new Ez2OnGameTrack(song, game,
                ParseDifficultyMode(songNode, XPathTo6KeysNormalLevel, DifficultyCategory.Normal,
                    Ez2OnKeyModes.FiveKeys)));
            gameTracks.Add(new Ez2OnGameTrack(song, game,
                ParseDifficultyMode(songNode, XPathTo6KeysHardLevel, DifficultyCategory.Hard,
                    Ez2OnKeyModes.EightKeys)));
            gameTracks.Add(new Ez2OnGameTrack(song, game,
                ParseDifficultyMode(songNode, XPathTo6KeysShdLevel, DifficultyCategory.SuperHard,
                    Ez2OnKeyModes.SixKeys)));
            gameTracks.Add(new Ez2OnGameTrack(song, game,
                ParseDifficultyMode(songNode, XPathTo8KeysEasyLevel, DifficultyCategory.Easy,
                    Ez2OnKeyModes.EightKeys)));
            gameTracks.Add(new Ez2OnGameTrack(song, game,
                ParseDifficultyMode(songNode, XPathTo8KeysNormalLevel, DifficultyCategory.Normal,
                    Ez2OnKeyModes.FiveKeys)));
            gameTracks.Add(new Ez2OnGameTrack(song, game,
                ParseDifficultyMode(songNode, XPathTo8KeysHardLevel, DifficultyCategory.Hard,
                    Ez2OnKeyModes.EightKeys)));
            gameTracks.Add(new Ez2OnGameTrack(song, game,
                ParseDifficultyMode(songNode, XPathTo8KeysShdLevel, DifficultyCategory.SuperHard,
                    Ez2OnKeyModes.EightKeys)));
        }
        catch (ParserException parserException)
        {
            _logger.LogWarning(parserException, "Unable to parse Song Node at XPath: {xpath}", songNode.XPath);
            _logger.LogTrace(parserException, "HTML Element: {html}", songNode.WriteTo());
        }

        return gameTracks;
    }
}
