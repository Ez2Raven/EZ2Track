using Crawler.SongScraping.Parsers.Exceptions;
using HtmlAgilityPack;

namespace Crawler.SongScraping.Parsers.Generic;

public class SongParser : ISongParser
{
    public string ParseTitle(HtmlNode node, string xPath)
    {
        var targetNode = node.SelectSingleNode(xPath);
        if (targetNode == null)
        {
            throw new ParserException("Unable to correctly parse song title from html");
        }

        return targetNode.InnerText;
    }

    public string ParseAlbum(HtmlNode node, string xPath)
    {
        var targetNode = node.SelectSingleNode(xPath);
        if (targetNode == null)
        {
            throw new ParserException("Unable to correctly parse song album from html");
        }

        return targetNode.InnerText;
    }

    public string ParseComposer(HtmlNode node, string xPath)
    {
        var targetNode = node.SelectSingleNode(xPath);
        if (targetNode == null)
        {
            throw new ParserException("Unable to correctly parse song composer from html");
        }

        return targetNode.InnerText;
    }

    public string ParseBpm(HtmlNode node, string xPath)
    {
        var targetNode = node.SelectSingleNode(xPath);
        if (targetNode == null)
        {
            throw new ParserException("Unable to correctly parse song BPM from html");
        }

        return targetNode.InnerText;
    }

    public string ParseGenre(HtmlNode node, string xPath)
    {
        var targetNode = node.SelectSingleNode(xPath);
        if (targetNode == null)
        {
            throw new ParserException("Unable to correctly parse song genre from html");
        }

        return targetNode.InnerText;
    }
}
