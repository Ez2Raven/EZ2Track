// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using CleanCode.Patterns.DataStructures;

namespace Gaming.Domain.Ez2on;

public class Ez2OnKeyModes : Enumeration
{
    public static readonly Ez2OnKeyModes None = new(1, "4K");
    public static readonly Ez2OnKeyModes FourKeys = new(2, "4K");
    public static readonly Ez2OnKeyModes FiveKeys = new(3, "5K");
    public static readonly Ez2OnKeyModes SixKeys = new(4, "6K");
    public static readonly Ez2OnKeyModes EightKeys = new(5, "8K");
    public static readonly Ez2OnKeyModes BasicFourKeys = new(6, "Basic 4K");
    public static readonly Ez2OnKeyModes BasicFiveKeys = new(7, "Basic 5K");
    public static readonly Ez2OnKeyModes BasicSixKeys = new(8, "Basic 6K");
    public static readonly Ez2OnKeyModes BasicEightKeys = new(9, "Basic 8K");

    public Ez2OnKeyModes(int id, string name) : base(id, name)
    {
    }
}
