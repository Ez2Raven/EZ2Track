using CleanCode.Patterns.DataStructures;
using Gaming.Domain.AggregateModels.SongChartAggregate;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace Crawler.SongScraping.Parsers.Generic;

public class DifficultyModeParser : IDifficultyModeParser
{
    private readonly ILogger<DifficultyModeParser> _logger;

    public DifficultyModeParser(ILogger<DifficultyModeParser> logger)
    {
        _logger = logger;
    }

    public int ParseLevel(HtmlNode node, string xPath)
    {
        var isValidNode = int.TryParse(node.SelectSingleNode(xPath)?.InnerText, out var level);
        if (!isValidNode)
        {
            _logger.LogWarning("Unable to correctly parse level from html");
        }

        return level;
    }

    public DifficultyCategory ParseDifficultyCategory(HtmlNode node, string xPath)
    {
        var nodeToValue = node.SelectSingleNode(xPath);
        if (nodeToValue == null)
        {
            _logger.LogWarning("Unable to correctly parse difficulty category from html");
            return DifficultyCategory.None;
        }

        return Enumeration.FromDisplayName<DifficultyCategory>(nodeToValue.InnerText);
    }

    public KeyModes ParseKeyMode(HtmlNode node, string xPath)
    {
        var nodeToValue = node.SelectSingleNode(xPath);
        if (nodeToValue == null)
        {
            _logger.LogWarning("Unable to correctly parse difficulty category from html");
            return KeyModes.None;
        }

        return Enumeration.FromDisplayName<KeyModes>(nodeToValue.InnerText);
    }
}
