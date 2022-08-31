namespace Gaming.Domain.AggregateModels.SongChartAggregate.Ez2on;

public class Ez2OnReleaseTitle : ReleaseTitle
{
    public static readonly Ez2OnReleaseTitle FirstTrax = new(1, "EZ2DJ 1ST TRACKS", false);
    public static readonly Ez2OnReleaseTitle SpecialEdition = new(2, "EZ2DJ Special Edition", false);
    public static readonly Ez2OnReleaseTitle SecondTrax = new(3, "EZ2DJ 2ND TRAX", false);
    public static readonly Ez2OnReleaseTitle ThirdTrax = new(4, "EZ2DJ 3RD TRAX", false);
    public static readonly Ez2OnReleaseTitle FourthTrax = new(5, "EZ2DJ 4TH TRAX", false);
    public static readonly Ez2OnReleaseTitle Platinum = new(6, "EZ2DJ PLATINUM", false);
    public static readonly Ez2OnReleaseTitle SixthTrax = new(7, "EZ2DJ 6TH TRAX", false);
    public static readonly Ez2OnReleaseTitle SeventhTrax = new(8, "EZ2DJ 7TH TRAX", false);
    public static readonly Ez2OnReleaseTitle Ez2On2008 = new(9, "EZ2ON Reboot: R - 2008", false);
    public static readonly Ez2OnReleaseTitle Ez2On2013 = new(10, "EZ2ON Reboot: R - 2013", false);
    public static readonly Ez2OnReleaseTitle Ez2On2021 = new(11, "EZ2ON Reboot: R - 2021", false);
    public static readonly Ez2OnReleaseTitle TimeTraveler = new(12, "EZ2ON Reboot: R - Time Traveler", true);
    public static readonly Ez2OnReleaseTitle CodeNameViolet = new(13, "EZ2ON Reboot: R - CodeName Violet", true);
    public static readonly Ez2OnReleaseTitle PrestigePass = new(14, "EZ2ON Reboot: R - Prestige Pass", true);
    public static readonly Ez2OnReleaseTitle O2Jam = new(15, "EZ2ON Reboot: R - O2Jam Collaboration DLC", true);

    private Ez2OnReleaseTitle(int id, string name, bool isDlc) : base(id, name, isDlc)
    {
    }
}
