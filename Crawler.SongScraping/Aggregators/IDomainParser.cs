using System.Collections.Generic;
using HtmlAgilityPack;

namespace Crawler.SongScraping.Aggregators;

/// <summary>
///     Returns domain objects based on the HTML structure of a specific
///     <see cref="IScraper{T}">Scraper</see>
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IDomainParser<T>
{
    /// <summary>
    ///     Returns a collection of domain objects based on the interpretations of HTML elements
    /// </summary>
    /// <param name="nodes"></param>
    /// <returns></returns>
    public IList<T> Parse(HtmlNodeCollection nodes);
}
