// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace Gaming.Domain.Aggregates.GameAggregate;

public interface IGame
{
    string Title { get; set; }
    bool IsDlc { get; set; }
}
