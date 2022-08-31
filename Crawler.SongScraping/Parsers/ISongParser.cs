using HtmlAgilityPack;

namespace Crawler.SongScraping.Parsers;

public interface ISongParser
{
    string ParseTitle(HtmlNode node, string xPath);
    string ParseAlbum(HtmlNode node, string xPath);
    string ParseComposer(HtmlNode node, string xPath);
    string ParseBpm(HtmlNode node, string xPath);
    string ParseGenre(HtmlNode node, string xPath);
}
