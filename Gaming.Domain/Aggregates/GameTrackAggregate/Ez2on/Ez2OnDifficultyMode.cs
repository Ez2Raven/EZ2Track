namespace Gaming.Domain.Aggregates.GameTrackAggregate.Ez2on;

public class Ez2OnDifficultyMode : IDifficultyMode
{
    public DifficultyCategory Category { get; set; } = DifficultyCategory.None;
    public KeyModes KeyMode { get; set; } = KeyModes.None;
    public int Level { get; set; }

    public override string ToString()
    {
        return $"{nameof(Category)}: {Category}, {nameof(Level)}: {Level}";
    }
}
