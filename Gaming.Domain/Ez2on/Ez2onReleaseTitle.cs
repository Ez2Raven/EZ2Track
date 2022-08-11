// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using CleanCode.Patterns.DataStructures;

namespace Gaming.Domain.Ez2on;

public class Ez2OnReleaseTitle : Enumeration
{
    public static readonly Ez2OnReleaseTitle FirstTrax = new(1, "EZ2DJ 1ST TRACKS");
    public static readonly Ez2OnReleaseTitle SpecialEdition = new(2, "EZ2DJ Special Edition");
    public static readonly Ez2OnReleaseTitle SecondTrax = new(3, "EZ2DJ 2ND TRAX");
    public static readonly Ez2OnReleaseTitle ThirdTrax = new(4, "EZ2DJ 3RD TRAX");
    public static readonly Ez2OnReleaseTitle FourthTrax = new(5, "EZ2DJ 4TH TRAX");
    public static readonly Ez2OnReleaseTitle Platinum = new(6, "EZ2DJ PLATINUM");
    public static readonly Ez2OnReleaseTitle SixthTrax = new(7, "EZ2DJ 6TH TRAX");
    public static readonly Ez2OnReleaseTitle SeventhTrax = new(8, "EZ2DJ 7TH TRAX");
    public static readonly Ez2OnReleaseTitle Ez2On2008 = new(9, "EZ2ON Reboot: R - 2008");
    public static readonly Ez2OnReleaseTitle Ez2On2013 = new(10, "EZ2ON Reboot: R - 2013");
    public static readonly Ez2OnReleaseTitle Ez2On2021 = new(11, "EZ2ON Reboot: R - 2021");
    public static readonly Ez2OnReleaseTitle TimeTraveler = new(12, "EZ2ON Reboot: R - Time Traveler");
    public static readonly Ez2OnReleaseTitle CodeNameViolet = new(13, "EZ2ON Reboot: R - CodeName Violet");
    public static readonly Ez2OnReleaseTitle PrestigePass = new(14, "EZ2ON Reboot: R - Prestige Pass");
    public static readonly Ez2OnReleaseTitle O2Jam = new(15, "EZ2ON Reboot: R - O2Jam Collaboration DLC");

    public Ez2OnReleaseTitle(int id, string name) : base(id, name)
    {
    }
}
