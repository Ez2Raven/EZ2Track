using System.Collections.Generic;
using Crawler.SongScraping.Interpreters.Exceptions;
using Gaming.Domain.AggregateModels.SongChartAggregate;
using HtmlAgilityPack;

namespace Crawler.SongScraping.Aggregators.Ez2OnWiki.Scrapers;

public class SongListLinkedScraper : LinkedScraper<ISongChart>
{
    public SongListLinkedScraper(IScraper<ISongChart> scraper, IDomainParser<ISong> songListParser) :
        base(scraper)
    {
        SongListParser = songListParser;
    }

    private IDomainParser<ISong> SongListParser { get; }

    public IList<ISong> ParseSongListFromWeb(string songListUrl)
    {
        var miniBrowser = new HtmlWeb();
        var loadUrlTask = miniBrowser.LoadFromWebAsync(songListUrl);
        var songListHtmlDocument = loadUrlTask.Result;
        var songListXPath = "//*[@id=\"content\"]/div/h3";

        if (songListHtmlDocument == null)
        {
            throw new InterpreterException("Unable to load song list url for parsing");
        }

        var albumNodes = songListHtmlDocument.DocumentNode.SelectNodes(songListXPath);
        if (albumNodes == null)
        {
            throw new InterpreterException("Invalid xPath to start parse song list url");
        }

        return SongListParser.Parse(albumNodes);
    }

    public IList<ISongChart> AddSongMetaData(string songListUrl, string songChartUrl)
    {
        var songList = ParseSongListFromWeb(songListUrl);
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
