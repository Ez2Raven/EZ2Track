using CleanCode.Patterns.DataStructures;

namespace Gaming.Domain.Aggregates.GameTrackAggregate;

public class DifficultyCategory : Enumeration
{
    public static readonly DifficultyCategory None = new(1, nameof(None));
    public static readonly DifficultyCategory Easy = new(2, nameof(Easy));
    public static readonly DifficultyCategory Normal = new(3, nameof(Normal));
    public static readonly DifficultyCategory Hard = new(4, nameof(Hard));
    public static readonly DifficultyCategory SuperHard = new(5, "SHD");

    public DifficultyCategory(int id, string name) : base(id, name)
    {
    }
}
