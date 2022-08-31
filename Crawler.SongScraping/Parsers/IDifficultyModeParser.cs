using Gaming.Domain.AggregateModels.SongChartAggregate;
using HtmlAgilityPack;

namespace Crawler.SongScraping.Parsers;

public interface IDifficultyModeParser
{
    int ParseLevel(HtmlNode node, string xPath);
    DifficultyCategory ParseDifficultyCategory(HtmlNode node, string xPath);
    KeyModes ParseKeyMode(HtmlNode node, string xPath);
}
