using System.Collections.Generic;

namespace Crawler.SongScraping.Aggregators;

/// <summary>
///     Parses domain objects from HTML Document downloaded from a given URL
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IScraper<T>
{
    IList<T> Run(string url);
}

/// <inheritdoc />
public abstract class Scraper<T> : IScraper<T>
{
    public abstract IList<T> Run(string url);
}

/// <summary>
///     <para>
///         A LinkedScraper parses a specific url and returns aggregated domain objects from the results of other
///         scrapers.
///     </para>
///     <para>Refer to Decorator Pattern</para>
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class LinkedScraper<T> : Scraper<T>
{
    private readonly IScraper<T> _scraper;

    protected LinkedScraper(IScraper<T> scraper)
    {
        _scraper = scraper;
    }

    public override IList<T> Run(string url)
    {
        return _scraper.Run(url);
    }
}
