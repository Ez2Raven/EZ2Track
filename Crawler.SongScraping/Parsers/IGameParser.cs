using Gaming.Domain.AggregateModels.SongChartAggregate;
using HtmlAgilityPack;

namespace Crawler.SongScraping.Parsers;

public interface IGameParser
{
    ReleaseTitle ParseReleaseTitle(HtmlNode node, string xPath);
}
