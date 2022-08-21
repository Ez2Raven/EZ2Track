// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using Gaming.Domain.Aggregates.GameAggregate;
using Gaming.Domain.Aggregates.MusicAggregate;

namespace Gaming.Domain.Aggregates.GameTrackAggregate;

public interface IGameTrack
{
    IGame Game { get; set; }
    ISong Song { get; set; }
    IDifficultyMode DifficultyMode { get; set; }
    string ThumbnailUrl { get; set; }
    string VisualizedBy { get; set; }
    string ToString();
}
