namespace Gaming.Domain.AggregateModels.SongChartAggregate;

public interface IDifficultyMode
{
    DifficultyCategory Category { get; set; }
    KeyModes KeyMode { get; set; }
    int Level { get; set; }
}
