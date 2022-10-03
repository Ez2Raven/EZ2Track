using Crawler.SongScraping.Interpreters.Exceptions;
using HtmlAgilityPack;

namespace Crawler.SongScraping.Interpreters.Generic;

public class SongAlbumInterpreter : IDomainInterpreter<string>
{
    public string Interpret(HtmlNode node, string xPath)
    {
        var targetNode = node.SelectSingleNode(xPath);
        if (targetNode == null)
        {
            throw new InterpreterException("Unable to correctly parse song title from html");
        }

        return targetNode.InnerText;
    }
}
