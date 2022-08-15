// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using Gaming.Domain.Aggregates.GameTrackAggregate;
using Gaming.Domain.Aggregates.MusicAggregate;

namespace Crawler.SongScraping.Parsers;

public interface IMusicGameParser
{
    IList<IGameTrack> Process();
    IList<IGameTrack> ProcessGameTracks(string url);
    IList<ISong> ProcessSongs(string url);
}
