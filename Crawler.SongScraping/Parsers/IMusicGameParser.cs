// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using Gaming.Domain.Aggregates.GameAggregate;
using Gaming.Domain.Aggregates.GameTrackAggregate;
using Gaming.Domain.Aggregates.MusicAggregate;
using HtmlAgilityPack;

namespace Crawler.SongScraping.Parsers;

public interface IMusicGameParser
{
    List<IGameTrack> Process(HtmlNodeCollection rowsOfSongNodes);
    Game ParseGameInfo(HtmlNode songNode);

    DifficultyMode ParseDifficultyMode(HtmlNode songNode, string xPathToDifficultyLevel,
        DifficultyCategory category);

    Song ParseSongInfo(HtmlNode songNode);
}
