using System.Collections.Generic;
using Crawler.SongScraping.Parsers.Exceptions;
using Gaming.Domain.AggregateModels.SongChartAggregate;
using HtmlAgilityPack;

namespace Crawler.SongScraping.Parsers.Ez2OnWiki.SongList;

public class SongListDecorator : Decorator<ISongChart>
{
    public SongListDecorator(IAggregator<ISongChart> aggregator, IHtmlCollectionParser<ISong> songListParser) : base(aggregator)
    {
        SongListParser = songListParser;
    }

    private IHtmlCollectionParser<ISong> SongListParser { get; }

    public IList<ISong> ParseSongList(string songListUrl)
    {
        var miniBrowser = new HtmlWeb();
        var loadUrlTask = miniBrowser.LoadFromWebAsync(songListUrl);
        var songListHtmlDocument = loadUrlTask.Result;
        var songListXPath = "//*[@id=\"content\"]/div/h3";

        if (songListHtmlDocument == null)
        {
            throw new ParserException("Unable to load song list url for parsing");
        }

        var albumNodes = songListHtmlDocument.DocumentNode.SelectNodes(songListXPath);
        if (albumNodes == null)
        {
            throw new ParserException("Invalid xPath to start parse song list url");
        }

        return SongListParser.Parse(albumNodes);
    }

    public IList<ISongChart> AddSongMetaData(string songListUrl, string songChartUrl)
    {
        var songList = ParseSongList(songListUrl);
        var songCharts = base.Run(songChartUrl);

        foreach (var song in songList)
        {
            foreach (var songChart in songCharts)
            {
                if (songChart.Song.Title == song.Title)
                {
                    songChart.Song = song;
                }
            }
        }

        return songCharts;
    }
}
