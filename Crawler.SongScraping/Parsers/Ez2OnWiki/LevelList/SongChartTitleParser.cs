using System;
using Crawler.SongScraping.Parsers.Exceptions;
using HtmlAgilityPack;

namespace Crawler.SongScraping.Parsers.Ez2OnWiki.LevelList;

public class SongChartTitleParser : ISongParser
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
        throw new NotSupportedException("Song composer is not available in song chart wiki page");
    }

    public string ParseBpm(HtmlNode node, string xPath)
    {
        throw new NotSupportedException("Song bpm is not available in song chart wiki page");
    }

    public string ParseGenre(HtmlNode node, string xPath)
    {
        throw new NotSupportedException("Song genre is not available in song chart wiki page");
    }
}
