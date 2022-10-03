using System.Linq;
using Crawler.SongScraping.Aggregators.Ez2OnWiki.Parsers.LevelList;
using Crawler.SongScraping.Aggregators.Ez2OnWiki.Parsers.SongList;
using Crawler.SongScraping.Aggregators.Ez2OnWiki.Scrapers;
using Crawler.SongScraping.Interpreters.Ez2On;
using Crawler.SongScraping.Interpreters.Generic;
using FluentAssertions;
using Gaming.Domain.AggregateModels.SongChartAggregate;
using Gaming.Domain.AggregateModels.SongChartAggregate.EqualityComparer;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Crawler.SongScraping.Tests;

public class Ez2OnWikiScrapingTest
{
    private readonly ITestOutputHelper _output;

    public Ez2OnWikiScrapingTest(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void SelectSingleNode_XPath_ReturnsValidHtmlNode()
    {
        var url = "https://wikiwiki.jp/ez2on/LevelList/List";
        var web1 = new HtmlWeb();
        var loadUrlTask = web1.LoadFromWebAsync(url);
        var htmlDoc = loadUrlTask.Result;
        var xpath =
            "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr";

        var ez2OnContentTable = htmlDoc.DocumentNode.SelectSingleNode(xpath);

        ez2OnContentTable.Should().NotBeNull();
    }

    [Fact]
    public void ParseSongCharts_Ez2onLevelListUrl_ReturnsListOfSongCharts()
    {
        var ez2onLevelListUrl = "https://wikiwiki.jp/ez2on/LevelList/List";
        var loggerStub = new Mock<ILogger<SongChartCollectionScraper>>();
        var songTitleInterpreter = new SongTitleInterpreter();
        var songAlbumInterpreter = new SongAlbumInterpreter();
        var gameTitleInterpreter = new Ez2OnReleaseInterpreter();
        var chartLevelInterpreter = new ChartLvInterpreter(new Mock<ILogger<ChartLvInterpreter>>().Object);
        var songChartParser =
            new SongChartCollectionParser(songTitleInterpreter, songAlbumInterpreter, gameTitleInterpreter,
                chartLevelInterpreter);
        var songChartCollectionScraper = new SongChartCollectionScraper(loggerStub.Object, songChartParser);

        var songCharts = songChartCollectionScraper.Run(ez2onLevelListUrl);

        songCharts.Count.Should().BeGreaterThan(0);
        songCharts.Should().NotContain(x => string.IsNullOrWhiteSpace(x.Game.Name));
        songCharts.Should().Contain(x => Equals(x.Game.IsDlc, true));
        songCharts.Should().Contain(x => Equals(x.Game.IsDlc, false));
        songCharts.Should().NotContain(x => Equals(x.DifficultyMode.Category, DifficultyCategory.None));
        songCharts.Should().Contain(x => x.DifficultyMode.Level > 0);
        songCharts.Should().NotContain(x => Equals(x.DifficultyMode.KeyMode, KeyModes.None));
        songCharts.Should().NotContain(x => string.IsNullOrWhiteSpace(x.Song.Title));
    }

    [Fact]
    public void Run_SongListDecorator_ReturnsListOfSongChartsWithSongDetails()
    {
        var songListUrl = "https://wikiwiki.jp/ez2on/SongList";
        var levelListUrl = "https://wikiwiki.jp/ez2on/LevelList/List";
        var loggerStub = new Mock<ILogger<SongChartCollectionScraper>>();
        var songTitleInterpreter = new SongTitleInterpreter();
        var songAlbumInterpreter = new SongAlbumInterpreter();
        var gameTitleInterpreter = new Ez2OnReleaseInterpreter();
        var chartLevelInterpreter = new ChartLvInterpreter(new Mock<ILogger<ChartLvInterpreter>>().Object);
        var songChartCollectionParser =
            new SongChartCollectionParser(songTitleInterpreter, songAlbumInterpreter, gameTitleInterpreter,
                chartLevelInterpreter);
        var songChartScraper = new SongChartCollectionScraper(loggerStub.Object, songChartCollectionParser);
        var songListParser = new SongListCollectionParser();
        var songListLinkedScraper = new SongListLinkedScraper(songChartScraper, songListParser);

        var songCharts = songListLinkedScraper.AddSongMetaData(songListUrl, levelListUrl);
        songCharts.Count.Should().BeGreaterThan(0);
        songCharts.Should().NotContain(x => string.IsNullOrWhiteSpace(x.Game.Name));
        songCharts.Should().Contain(x => Equals(x.Game.IsDlc, true));
        songCharts.Should().Contain(x => Equals(x.Game.IsDlc, false));
        songCharts.Should().NotContain(x => Equals(x.DifficultyMode.Category, DifficultyCategory.None));
        songCharts.Should().Contain(x => x.DifficultyMode.Level > 0);
        songCharts.Should().NotContain(x => Equals(x.DifficultyMode.KeyMode, KeyModes.None));
        songCharts.Should().NotContain(x => string.IsNullOrWhiteSpace(x.Song.Title));
        songCharts.Should().NotContain(x => string.IsNullOrWhiteSpace(x.Song.Album));
        // contains at least 1 non-null record for BPM, Genre and composer
        songCharts.Should().Contain(x => !string.IsNullOrWhiteSpace(x.Song.Bpm));
        songCharts.Should().Contain(x => !string.IsNullOrWhiteSpace(x.Song.Genre));
        songCharts.Should().Contain(x => !string.IsNullOrWhiteSpace(x.Song.Composer));
    }

    [Theory]
    [InlineData("M2U")]
    public void Run_SongListDecorator_ReturnsListOfSongChartsWithSongDetailsOfSpecificComposer(string composer)
    {
        var songListUrl = "https://wikiwiki.jp/ez2on/SongList";
        var levelListUrl = "https://wikiwiki.jp/ez2on/LevelList/List";
        var loggerStub = new Mock<ILogger<SongChartCollectionScraper>>();
        var songTitleInterpreter = new SongTitleInterpreter();
        var songAlbumInterpreter = new SongAlbumInterpreter();
        var gameTitleInterpreter = new Ez2OnReleaseInterpreter();
        var chartLevelInterpreter = new ChartLvInterpreter(new Mock<ILogger<ChartLvInterpreter>>().Object);
        var songChartCollectionParser =
            new SongChartCollectionParser(songTitleInterpreter, songAlbumInterpreter, gameTitleInterpreter,
                chartLevelInterpreter);
        var songChartScraper = new SongChartCollectionScraper(loggerStub.Object, songChartCollectionParser);
        var songListParser = new SongListCollectionParser();
        var songListLinkedScraper = new SongListLinkedScraper(songChartScraper, songListParser);

        var songCharts = songListLinkedScraper.AddSongMetaData(songListUrl, levelListUrl);
        var filteredSongCharts = songCharts.Where(songChart => songChart.Song.Composer.ToUpper() == composer).ToList();

        filteredSongCharts.Count.Should().BeGreaterThan(0);
        filteredSongCharts.Should().NotContain(x => string.IsNullOrWhiteSpace(x.Game.Name));
        filteredSongCharts.Should().Contain(x => Equals(x.Game.IsDlc, true));
        filteredSongCharts.Should().Contain(x => Equals(x.Game.IsDlc, false));
        filteredSongCharts.Should().NotContain(x => Equals(x.DifficultyMode.Category, DifficultyCategory.None));
        filteredSongCharts.Should().Contain(x => x.DifficultyMode.Level > 0);
        filteredSongCharts.Should().NotContain(x => Equals(x.DifficultyMode.KeyMode, KeyModes.None));
        filteredSongCharts.Should().NotContain(x => string.IsNullOrWhiteSpace(x.Song.Title));
        filteredSongCharts.Should().NotContain(x => string.IsNullOrWhiteSpace(x.Song.Album));
        // contains at least 1 non-null record for BPM, Genre and composer
        filteredSongCharts.Should().Contain(x => !string.IsNullOrWhiteSpace(x.Song.Bpm));
        filteredSongCharts.Should().Contain(x => !string.IsNullOrWhiteSpace(x.Song.Genre));
        filteredSongCharts.Should().OnlyContain(x => Equals(x.Song.Composer, composer));
        foreach (var songChart in filteredSongCharts.OrderBy(songChart => songChart.Song.Title)
                     .Distinct(new SongTitleComparer()))
        {
            _output.WriteLine($"{songChart.Song}");
        }
    }
}
