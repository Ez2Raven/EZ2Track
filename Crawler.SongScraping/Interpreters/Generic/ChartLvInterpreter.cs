using Crawler.SongScraping.Interpreters.Exceptions;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace Crawler.SongScraping.Interpreters.Generic;

public class ChartLvInterpreter : IDomainInterpreter<int>
{
    private readonly ILogger<ChartLvInterpreter> _logger;

    public ChartLvInterpreter(ILogger<ChartLvInterpreter> logger)
    {
        _logger = logger;
    }

    public int Interpret(HtmlNode node, string xPath)
    {
        var targetNode = node.SelectSingleNode(xPath);
        if (targetNode == null)
        {
            throw new InterpreterException("Unable to correctly parse song title from html");
        }

        var isValidInteger = int.TryParse(node.SelectSingleNode(xPath)?.InnerText, out var level);
        if (!isValidInteger)
        {
            _logger.LogWarning("No integer value found at {xPath} of html node {url}. Returning 0 for Chart Level",
                xPath, node.XPath);
        }

        return level;
    }
}
