using System.Linq;
using Crawler.SongScraping.Parsers;
using Crawler.SongScraping.Parsers.Ez2OnWiki.LevelList;
using Crawler.SongScraping.Parsers.Ez2OnWiki.SongList;
using Crawler.SongScraping.Parsers.Generic;
using FluentAssertions;
using Gaming.Domain.AggregateModels.SongChartAggregate;
using Gaming.Domain.AggregateModels.SongChartAggregate.EqualityComparer;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Crawler.SongScraping.Tests;

public class Ez2OnWikiAggregatorTest
{
    private readonly ITestOutputHelper _output;

    public Ez2OnWikiAggregatorTest(ITestOutputHelper output)
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
        var url = "https://wikiwiki.jp/ez2on/LevelList/List";
        var loggerStub = new Mock<ILogger<LevelListAggregator>>();
        var songParser = new SongChartTitleParser();
        var gameParser = new Ez2OnGameParser();
        var difficultyParser = new DifficultyModeParser(new Mock<ILogger<DifficultyModeParser>>().Object);
        var parser = new LevelListParser(songParser, gameParser, difficultyParser);
        var aggregator = new LevelListAggregator(loggerStub.Object, parser);

        var songCharts = aggregator.ParseSongCharts(url);
        songCharts.Count.Should().BeGreaterThan(0);
        foreach (var chart in songCharts)
        {
            _output.WriteLine(chart.ToString());
        }
    }

    [Fact]
    public void ParseSongList_Ez2onSongListUrl_ReturnsListOfSongs()
    {
        var url = "https://wikiwiki.jp/ez2on/SongList";
        var songListProcessor = new SongListParser();
        var levelListAggregatorStub = new Mock<IAggregator<ISongChart>>();
        var songListDecorator = new SongListDecorator(levelListAggregatorStub.Object, songListProcessor);
        var songList = songListDecorator.ParseSongList(url);
        songList.Count.Should().BeGreaterThan(0);
        foreach (var song in songList)
        {
            _output.WriteLine(song.ToString());
        }
    }

    [Fact]
    public void Run_SongListDecorator_ReturnsListOfSongChartsWithSongDetails()
    {
        var songListUrl = "https://wikiwiki.jp/ez2on/SongList";
        var levelListUrl = "https://wikiwiki.jp/ez2on/LevelList/List";
        var loggerStub = new Mock<ILogger<LevelListAggregator>>();
        var songParser = new SongChartTitleParser();
        var gameParser = new Ez2OnGameParser();
        var difficultyParser = new DifficultyModeParser(new Mock<ILogger<DifficultyModeParser>>().Object);
        var parser = new LevelListParser(songParser, gameParser, difficultyParser);
        var aggregator = new LevelListAggregator(loggerStub.Object, parser);
        var songListParser = new SongListParser();
        var songListDecorator = new SongListDecorator(aggregator, songListParser);

        var songCharts = songListDecorator.AddSongMetaData(songListUrl, levelListUrl);
        songCharts.Count.Should().BeGreaterThan(0);

        var filteredSongCharts = songCharts.Where(songChart => songChart.Song.Composer.ToUpper() == "M2U").ToList();
        filteredSongCharts.Count.Should().BeGreaterThan(0);
        foreach (var songChart in filteredSongCharts.OrderBy(songChart => songChart.Song.Title).Distinct(new SongTitleComparer()))
        {
            _output.WriteLine(songChart.Song.Title);
        }
    }
}
