namespace Gaming.Domain.AggregateModels.SongChartAggregate.Ez2on;

public class Ez2OnKeyModes : KeyModes
{
    public static readonly Ez2OnKeyModes FourKeys = new(1, "4K");
    public static readonly Ez2OnKeyModes FiveKeys = new(2, "5K");
    public static readonly Ez2OnKeyModes SixKeys = new(3, "6K");
    public static readonly Ez2OnKeyModes EightKeys = new(4, "8K");

    public Ez2OnKeyModes(int id, string name) : base(id, name)
    {
    }
}
