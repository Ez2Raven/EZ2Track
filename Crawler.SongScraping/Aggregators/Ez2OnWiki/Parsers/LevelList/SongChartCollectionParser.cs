using System.Collections.Generic;
using Crawler.SongScraping.Interpreters;
using Gaming.Domain.AggregateModels.SongChartAggregate;
using Gaming.Domain.AggregateModels.SongChartAggregate.Ez2on;
using HtmlAgilityPack;

namespace Crawler.SongScraping.Aggregators.Ez2OnWiki.Parsers.LevelList;

public class SongChartCollectionParser : IDomainParser<ISongChart>
{
    private readonly IDomainInterpreter<int> _chartLevelInterpreter;
    private readonly IDomainInterpreter<ReleaseTitle> _gameReleaseInterpreter;
    private readonly IDomainInterpreter<string> _songAlbumInterpreter;
    private readonly IDomainInterpreter<string> _songTitleInterpreter;

    public SongChartCollectionParser(IDomainInterpreter<string> songTitleInterpreter,
        IDomainInterpreter<string> songAlbumInterpreter, IDomainInterpreter<ReleaseTitle> gameReleaseInterpreter,
        IDomainInterpreter<int> chartLevelInterpreter)
    {
        _songTitleInterpreter = songTitleInterpreter;
        _songAlbumInterpreter = songAlbumInterpreter;
        _gameReleaseInterpreter = gameReleaseInterpreter;
        _chartLevelInterpreter = chartLevelInterpreter;
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

    public IList<ISongChart> Parse(HtmlNodeCollection nodes)
    {
        var gameTracks = new List<ISongChart>();
        foreach (var songNode in nodes)
        {
            var game = _gameReleaseInterpreter.Interpret(songNode, XPathToAlbum);
            var songTitle = _songTitleInterpreter.Interpret(songNode, XPathToSongTitle);
            var songAlbum = _songAlbumInterpreter.Interpret(songNode, XPathToAlbum);
            var song = new Song {Album = songAlbum, Title = songTitle};

            var ez4KeysDifficultyMode = new Ez2OnDifficultyMode
            {
                Category = DifficultyCategory.Easy,
                KeyMode = Ez2OnKeyModes.FourKeys,
                Level = _chartLevelInterpreter.Interpret(songNode, XPathTo4KeysEasyLevel)
            };

            gameTracks.Add(new Ez2OnSongChart(song, game, ez4KeysDifficultyMode));

            var nm4KeysDifficultyMode = new Ez2OnDifficultyMode
            {
                Category = DifficultyCategory.Normal,
                KeyMode = Ez2OnKeyModes.FourKeys,
                Level = _chartLevelInterpreter.Interpret(songNode, XPathTo4KeysNormalLevel)
            };

            gameTracks.Add(new Ez2OnSongChart(song, game, nm4KeysDifficultyMode));

            var hd4KeysDifficultyMode = new Ez2OnDifficultyMode
            {
                Category = DifficultyCategory.Hard,
                KeyMode = Ez2OnKeyModes.FourKeys,
                Level = _chartLevelInterpreter.Interpret(songNode, XPathTo4KeysHardLevel)
            };

            gameTracks.Add(new Ez2OnSongChart(song, game, hd4KeysDifficultyMode));

            var shd4KeysDifficultyMode = new Ez2OnDifficultyMode
            {
                Category = DifficultyCategory.SuperHard,
                KeyMode = Ez2OnKeyModes.FourKeys,
                Level = _chartLevelInterpreter.Interpret(songNode, XPathTo4KeysShdLevel)
            };

            gameTracks.Add(new Ez2OnSongChart(song, game, shd4KeysDifficultyMode));

            var ez5KeysDifficultyMode = new Ez2OnDifficultyMode
            {
                Category = DifficultyCategory.Easy,
                KeyMode = Ez2OnKeyModes.FiveKeys,
                Level = _chartLevelInterpreter.Interpret(songNode, XPathTo5KeysEasyLevel)
            };

            gameTracks.Add(new Ez2OnSongChart(song, game, ez5KeysDifficultyMode));

            var nm5KeysDifficultyMode = new Ez2OnDifficultyMode
            {
                Category = DifficultyCategory.Normal,
                KeyMode = Ez2OnKeyModes.FiveKeys,
                Level = _chartLevelInterpreter.Interpret(songNode, XPathTo5KeysNormalLevel)
            };

            gameTracks.Add(new Ez2OnSongChart(song, game, nm5KeysDifficultyMode));

            var hd5KeysDifficultyMode = new Ez2OnDifficultyMode
            {
                Category = DifficultyCategory.Hard,
                KeyMode = Ez2OnKeyModes.FiveKeys,
                Level = _chartLevelInterpreter.Interpret(songNode, XPathTo5KeysHardLevel)
            };

            gameTracks.Add(new Ez2OnSongChart(song, game, hd5KeysDifficultyMode));

            var shd5KeysDifficultyMode = new Ez2OnDifficultyMode
            {
                Category = DifficultyCategory.SuperHard,
                KeyMode = Ez2OnKeyModes.FiveKeys,
                Level = _chartLevelInterpreter.Interpret(songNode, XPathTo5KeysShdLevel)
            };

            gameTracks.Add(new Ez2OnSongChart(song, game, shd5KeysDifficultyMode));

            var ez6KeysDifficultyMode = new Ez2OnDifficultyMode
            {
                Category = DifficultyCategory.Easy,
                KeyMode = Ez2OnKeyModes.SixKeys,
                Level = _chartLevelInterpreter.Interpret(songNode, XPathTo6KeysEasyLevel)
            };

            gameTracks.Add(new Ez2OnSongChart(song, game, ez6KeysDifficultyMode));

            var nm6KeysDifficultyMode = new Ez2OnDifficultyMode
            {
                Category = DifficultyCategory.Normal,
                KeyMode = Ez2OnKeyModes.SixKeys,
                Level = _chartLevelInterpreter.Interpret(songNode, XPathTo6KeysNormalLevel)
            };

            gameTracks.Add(new Ez2OnSongChart(song, game, nm6KeysDifficultyMode));

            var hd6KeysDifficultyMode = new Ez2OnDifficultyMode
            {
                Category = DifficultyCategory.Hard,
                KeyMode = Ez2OnKeyModes.SixKeys,
                Level = _chartLevelInterpreter.Interpret(songNode, XPathTo6KeysHardLevel)
            };

            gameTracks.Add(new Ez2OnSongChart(song, game, hd6KeysDifficultyMode));

            var shd6KeysDifficultyMode = new Ez2OnDifficultyMode
            {
                Category = DifficultyCategory.SuperHard,
                KeyMode = Ez2OnKeyModes.SixKeys,
                Level = _chartLevelInterpreter.Interpret(songNode, XPathTo6KeysShdLevel)
            };

            gameTracks.Add(new Ez2OnSongChart(song, game, shd6KeysDifficultyMode));

            var ez8KeysDifficultyMode = new Ez2OnDifficultyMode
            {
                Category = DifficultyCategory.Easy,
                KeyMode = Ez2OnKeyModes.EightKeys,
                Level = _chartLevelInterpreter.Interpret(songNode, XPathTo8KeysEasyLevel)
            };

            gameTracks.Add(new Ez2OnSongChart(song, game, ez8KeysDifficultyMode));

            var nm8KeysDifficultyMode = new Ez2OnDifficultyMode
            {
                Category = DifficultyCategory.Normal,
                KeyMode = Ez2OnKeyModes.EightKeys,
                Level = _chartLevelInterpreter.Interpret(songNode, XPathTo8KeysNormalLevel)
            };

            gameTracks.Add(new Ez2OnSongChart(song, game, nm8KeysDifficultyMode));

            var hd8KeysDifficultyMode = new Ez2OnDifficultyMode
            {
                Category = DifficultyCategory.Hard,
                KeyMode = Ez2OnKeyModes.EightKeys,
                Level = _chartLevelInterpreter.Interpret(songNode, XPathTo8KeysHardLevel)
            };

            gameTracks.Add(new Ez2OnSongChart(song, game, hd8KeysDifficultyMode));

            var shd8KeysDifficultyMode = new Ez2OnDifficultyMode
            {
                Category = DifficultyCategory.SuperHard,
                KeyMode = Ez2OnKeyModes.EightKeys,
                Level = _chartLevelInterpreter.Interpret(songNode, XPathTo8KeysShdLevel)
            };

            gameTracks.Add(new Ez2OnSongChart(song, game, shd8KeysDifficultyMode));
        }

        return gameTracks;
    }
}
