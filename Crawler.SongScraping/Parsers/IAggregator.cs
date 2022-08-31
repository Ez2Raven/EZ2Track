using System.Collections.Generic;
using HtmlAgilityPack;

namespace Crawler.SongScraping.Parsers;

public interface IAggregator<T>
{
    IList<T> Run(string url);
}

public abstract class Aggregator<T> : IAggregator<T>
{
    public abstract IList<T> Run(string url);
}

public abstract class Decorator<T> : Aggregator<T>
{
    private readonly IAggregator<T> _aggregator;

    protected Decorator(IAggregator<T> aggregator)
    {
        _aggregator = aggregator;
    }

    public override IList<T> Run(string url)
    {
        return _aggregator.Run(url);
    }
}

public interface IHtmlCollectionParser<T>
{
    public IList<T> Parse(HtmlNodeCollection nodes);
}
