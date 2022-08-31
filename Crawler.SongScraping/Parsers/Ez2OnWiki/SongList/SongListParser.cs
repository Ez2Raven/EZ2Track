using System.Collections.Generic;
using Crawler.SongScraping.Parsers.Exceptions;
using Gaming.Domain.AggregateModels.SongChartAggregate;
using HtmlAgilityPack;

namespace Crawler.SongScraping.Parsers.Ez2OnWiki.SongList;

public class SongListParser : IHtmlCollectionParser<ISong>
{
    public string XPathToSongTitleV2 { get; set; } = "td[1]";
    public string XPathToComposer { get; set; } = "td[2]";

    public string XPathToBpm { get; set; } = "td[3]";

    public string XPathToGenre { get; set; } = "td[5]";

    public IList<ISong> Parse(HtmlNodeCollection nodes)
    {
        var songList = new List<ISong>();
        foreach (var albumNode in nodes)
        {
            var album = albumNode.InnerText.Trim();
            var songNodes = albumNode.SelectNodes("following-sibling::div[1]/table/tbody/tr");
            if (songNodes == null)
            {
                throw new ParserException("unable to recognise song nodes from song list page");
            }

            foreach (var songNode in songNodes)
            {
                var title = songNode.SelectSingleNode(XPathToSongTitleV2)?.InnerText.Trim() ?? string.Empty;
                var composer = songNode.SelectSingleNode(XPathToComposer)?.InnerText.Trim() ?? string.Empty;
                var genre = songNode.SelectSingleNode(XPathToGenre)?.InnerText.Trim() ?? string.Empty;
                var bpm = songNode.SelectSingleNode(XPathToBpm)?.InnerText.Trim() ?? string.Empty;
                songList.Add(new Song(title, composer, album, genre, bpm));
            }
        }

        return songList;
    }
}
