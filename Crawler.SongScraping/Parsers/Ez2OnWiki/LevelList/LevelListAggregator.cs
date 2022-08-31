using System.Collections.Generic;
using Crawler.SongScraping.Parsers.Exceptions;
using Gaming.Domain.AggregateModels.SongChartAggregate;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace Crawler.SongScraping.Parsers.Ez2OnWiki.LevelList;

public class LevelListAggregator : Aggregator<ISongChart>
{
    private readonly ILogger<LevelListAggregator> _logger;

    public LevelListAggregator(ILogger<LevelListAggregator> logger, IHtmlCollectionParser<ISongChart> songChartParser)
    {
        _logger = logger;
        LevelListParser = songChartParser;
    }

    private IHtmlCollectionParser<ISongChart> LevelListParser { get; }


    public override IList<ISongChart> Run(string songChartUrl)
    {
        var songCharts = ParseSongCharts(songChartUrl);
        return songCharts;
    }

    public IList<ISongChart> ParseSongCharts(string songChartUrl)
    {
        var miniBrowser = new HtmlWeb();
        var loadUrlTask = miniBrowser.LoadFromWebAsync(songChartUrl);
        var levelListHtmlDoc = loadUrlTask.Result;
        var levelListXPath =
            "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr";
        if (levelListHtmlDoc == null)
        {
            throw new ParserException("Unable to load level list url for parsing");
        }

        var songNodes = levelListHtmlDoc.DocumentNode.SelectNodes(levelListXPath);
        if (songNodes == null)
        {
            throw new ParserException("Invalid xPath to start parse level list url");
        }

        return LevelListParser.Parse(songNodes);
    }
}
