using Crawler.SongScraping.Interpreters.Exceptions;
using Gaming.Domain.AggregateModels.SongChartAggregate;
using Gaming.Domain.AggregateModels.SongChartAggregate.Ez2on;
using HtmlAgilityPack;

namespace Crawler.SongScraping.Interpreters.Ez2On;

public class Ez2OnReleaseInterpreter : IDomainInterpreter<ReleaseTitle>
{
    public ReleaseTitle Interpret(HtmlNode node, string xPath)
    {
        var album = node.SelectSingleNode(xPath)?.InnerText.Trim();

        return album?.ToUpper() switch
        {
            "1ST" => Ez2OnReleaseTitle.FirstTrax,
            "2ND" => Ez2OnReleaseTitle.SecondTrax,
            "3RD" => Ez2OnReleaseTitle.ThirdTrax,
            "4TH" => Ez2OnReleaseTitle.FourthTrax,
            "PT" => Ez2OnReleaseTitle.Platinum,
            "6TH" => Ez2OnReleaseTitle.SixthTrax,
            "7TH" => Ez2OnReleaseTitle.SeventhTrax,
            "S/E" => Ez2OnReleaseTitle.SpecialEdition,
            "2008" => Ez2OnReleaseTitle.Ez2On2008,
            "2013" => Ez2OnReleaseTitle.Ez2On2013,
            "2021" => Ez2OnReleaseTitle.Ez2On2021,
            "TT" => Ez2OnReleaseTitle.TimeTraveler,
            "CV" => Ez2OnReleaseTitle.CodeNameViolet,
            "PP" => Ez2OnReleaseTitle.PrestigePass,
            "O2" => Ez2OnReleaseTitle.O2Jam,
            _ => throw new InterpreterException("Unrecognized EZ2ON Reboot: R release title.")
        };
    }
}
