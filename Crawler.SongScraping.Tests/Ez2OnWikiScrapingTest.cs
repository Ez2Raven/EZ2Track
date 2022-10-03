using System.Linq;
using Crawler.SongScraping.Aggregators.Ez2OnWiki.Parsers.LevelList;
using Crawler.SongScraping.Aggregators.Ez2OnWiki.Parsers.SongList;
using Crawler.SongScraping.Aggregators.Ez2OnWiki.Scrapers;
using Crawler.SongScraping.Interpreters.Ez2On;
using Crawler.SongScraping.Interpreters.Generic;
using FluentAssertions;
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
        var parser =
            new SongChartCollectionParser(songTitleInterpreter, songAlbumInterpreter, gameTitleInterpreter, chartLevelInterpreter);
        var songChartCollectionScraper = new SongChartCollectionScraper(loggerStub.Object, parser);

        var songCharts = songChartCollectionScraper.Run(ez2onLevelListUrl);

        songCharts.Count.Should().BeGreaterThan(0);
        foreach (var chart in songCharts)
        {
            _output.WriteLine(chart.ToString());
        }
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
            new SongChartCollectionParser(songTitleInterpreter, songAlbumInterpreter, gameTitleInterpreter, chartLevelInterpreter);
        var songChartScraper = new SongChartCollectionScraper(loggerStub.Object, songChartCollectionParser);
        var songListParser = new SongListCollectionParser();
        var songListLinkedScraper = new SongListLinkedScraper(songChartScraper, songListParser);

        var songCharts = songListLinkedScraper.AddSongMetaData(songListUrl, levelListUrl);
        songCharts.Count.Should().BeGreaterThan(0);

        var filteredSongCharts = songCharts.Where(songChart => songChart.Song.Composer.ToUpper() == "M2U").ToList();
        filteredSongCharts.Count.Should().BeGreaterThan(0);
        foreach (var songChart in filteredSongCharts.OrderBy(songChart => songChart.Song.Title)
                     .Distinct(new SongTitleComparer()))
        {
            _output.WriteLine($"{songChart.Song.Title} - {songChart.Song.Genre}");
        }
    }
}
