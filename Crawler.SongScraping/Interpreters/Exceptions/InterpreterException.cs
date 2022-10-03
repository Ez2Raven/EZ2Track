using System;

namespace Crawler.SongScraping.Interpreters.Exceptions;

public class InterpreterException : Exception
{
    public InterpreterException()
    {
    }

    public InterpreterException(string? message) : base(message)
    {
    }

    public InterpreterException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
