using HtmlAgilityPack;

namespace Crawler.SongScraping.Interpreters;

/// <summary>
///     Reads a HTML Node at a predetermined xPath and returns the interpreted domain entity
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IDomainInterpreter<out T>
{
    T Interpret(HtmlNode node, string xPath);
}
