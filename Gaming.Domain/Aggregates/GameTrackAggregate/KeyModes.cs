using CleanCode.Patterns.DataStructures;

namespace Gaming.Domain.Aggregates.GameTrackAggregate;

public class KeyModes : Enumeration
{
    public static readonly KeyModes None = new(0, "N/A");

    public KeyModes(int id, string name) : base(id, name)
    {
    }
}
