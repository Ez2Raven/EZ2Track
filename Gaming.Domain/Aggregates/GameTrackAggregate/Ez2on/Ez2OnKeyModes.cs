// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace Gaming.Domain.Aggregates.GameTrackAggregate.Ez2on;

public class Ez2OnKeyModes : KeyModes
{
    public static readonly Ez2OnKeyModes FourKeys = new(1, "4K");
    public static readonly Ez2OnKeyModes FiveKeys = new(2, "5K");
    public static readonly Ez2OnKeyModes SixKeys = new(3, "6K");
    public static readonly Ez2OnKeyModes EightKeys = new(4, "8K");
    public static readonly Ez2OnKeyModes BasicFourKeys = new(5, "Basic 4K");
    public static readonly Ez2OnKeyModes BasicFiveKeys = new(6, "Basic 5K");
    public static readonly Ez2OnKeyModes BasicSixKeys = new(7, "Basic 6K");
    public static readonly Ez2OnKeyModes BasicEightKeys = new(8, "Basic 8K");

    public Ez2OnKeyModes(int id, string name) : base(id, name)
    {
    }
}
