using System;

namespace Crawler.SongScraping.Aggregators.Exceptions;

public class ScraperException : Exception
{
    public ScraperException()
    {
    }

    public ScraperException(string? message) : base(message)
    {
    }

    public ScraperException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
