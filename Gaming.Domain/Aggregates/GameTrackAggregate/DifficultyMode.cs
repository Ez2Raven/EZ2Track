using Gaming.Domain.Ez2on;

namespace Gaming.Domain.Aggregates.GameTrackAggregate;

public class DifficultyMode
{
    public DifficultyCategory Category { get; set; } = DifficultyCategory.None;
    public Ez2OnKeyModes KeyMode { get; set; } = Ez2OnKeyModes.None;
    public int Level { get; set; }

    public override string ToString()
    {
        return $"{nameof(Category)}: {Category}, {nameof(Level)}: {Level}";
    }
}
