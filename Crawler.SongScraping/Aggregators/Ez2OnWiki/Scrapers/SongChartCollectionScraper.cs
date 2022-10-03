using System.Collections.Generic;
using Crawler.SongScraping.Aggregators.Exceptions;
using Gaming.Domain.AggregateModels.SongChartAggregate;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace Crawler.SongScraping.Aggregators.Ez2OnWiki.Scrapers;

public class SongChartCollectionScraper : Scraper<ISongChart>
{
    private readonly ILogger<SongChartCollectionScraper> _logger;

    public SongChartCollectionScraper(ILogger<SongChartCollectionScraper> logger,
        IDomainParser<ISongChart> songChartParser)
    {
        _logger = logger;
        SongChartParser = songChartParser;
    }

    private IDomainParser<ISongChart> SongChartParser { get; }


    public override IList<ISongChart> Run(string songChartUrl)
    {
        var songCharts = LoadSongChartsFromWeb(songChartUrl);
        return songCharts;
    }

    public IList<ISongChart> LoadSongChartsFromWeb(string songChartUrl)
    {
        var miniBrowser = new HtmlWeb();
        var loadUrlTask = miniBrowser.LoadFromWebAsync(songChartUrl);
        var htmlDoc = loadUrlTask.Result;
        var xPathToSongCharts =
            "//*[@id=\"content\"]/h2[contains(text(), '難易度表 (STANDARD)')]/following-sibling::div/div/table/tbody/tr";
        if (htmlDoc == null)
        {
            throw new ScraperException("Unable to load level list url for parsing");
        }

        var songChartNodes = htmlDoc.DocumentNode.SelectNodes(xPathToSongCharts);
        if (songChartNodes == null)
        {
            throw new ScraperException("Invalid xPath to start parse level list url");
        }

        return SongChartParser.Parse(songChartNodes);
    }
}
