// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using CleanCode.Patterns.DataStructures;

namespace Gaming.Domain.Ez2on;

public class Ez2OnReleaseTitle : Enumeration
{
    public static readonly Ez2OnReleaseTitle FirstTrax = new(1, "EZ2DJ 1ST TRACKS");
    public static readonly Ez2OnReleaseTitle SpecialEdition = new(2, "EZ2DJ Special Edition");
    public static readonly Ez2OnReleaseTitle SecondTrax = new(1, "EZ2DJ 2ND TRAX");
    public static readonly Ez2OnReleaseTitle ThirdTrax = new(1, "EZ2DJ 3RD TRAX");
    public static readonly Ez2OnReleaseTitle FourthTrax = new(1, "EZ2DJ 4TH TRAX");
    public static readonly Ez2OnReleaseTitle Platinum = new(1, "PLATINUM");
    public static readonly Ez2OnReleaseTitle SixthTrax = new(1, "6TH TRAX");
    public static readonly Ez2OnReleaseTitle SeventhTrax = new(1, "7TH TRAX");
    public static readonly Ez2OnReleaseTitle Ez2On2008 = new(1, "EZ2ON 2008");
    public static readonly Ez2OnReleaseTitle Ez2On2013 = new(1, "EZ2ON 2013");
    public static readonly Ez2OnReleaseTitle Ez2On2021 = new(1, "EZ2ON 2021");

    public Ez2OnReleaseTitle(int id, string name) : base(id, name)
    {
    }
}
