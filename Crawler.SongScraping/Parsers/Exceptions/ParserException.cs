// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System;

namespace Crawler.SongScraping.Parsers.Exceptions;

public class ParserException : Exception
{
    public ParserException()
    {
    }

    public ParserException(string? message) : base(message)
    {
    }

    public ParserException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
