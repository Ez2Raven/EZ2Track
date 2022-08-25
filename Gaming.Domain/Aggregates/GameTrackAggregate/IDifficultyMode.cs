namespace Gaming.Domain.Aggregates.GameTrackAggregate;

public interface IDifficultyMode
{
    DifficultyCategory Category { get; set; }
    KeyModes KeyMode { get; set; }
    int Level { get; set; }
}
