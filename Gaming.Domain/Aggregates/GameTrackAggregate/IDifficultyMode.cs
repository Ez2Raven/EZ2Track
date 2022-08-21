// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace Gaming.Domain.Aggregates.GameTrackAggregate;

public interface IDifficultyMode
{
    DifficultyCategory Category { get; set; }
    KeyModes KeyMode { get; set; }
    int Level { get; set; }
}
