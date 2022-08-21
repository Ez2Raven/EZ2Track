// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using CleanCode.Patterns.DataStructures;

namespace Gaming.Domain.Aggregates.GameTrackAggregate;

public class KeyModes : Enumeration
{
    public static readonly KeyModes None = new(0, "N/A");

    public KeyModes(int id, string name) : base(id, name)
    {
    }
}
