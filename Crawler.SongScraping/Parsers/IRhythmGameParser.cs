// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using Gaming.Domain.Aggregates.GameTrackAggregate;

namespace Crawler.SongScraping.Parsers;

public interface IRhythmGameParser
{
    IList<IGameTrack> Parse();

    IGameTrackParser GameTrackParser();
    ISongParser SongParser();
    IGameParser GameParser();
}

public interface IGameParser
{
}

public interface ISongParser
{
}

public interface IGameTrackParser
{
}
