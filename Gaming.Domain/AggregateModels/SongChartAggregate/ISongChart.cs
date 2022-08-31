namespace Gaming.Domain.AggregateModels.SongChartAggregate;

public interface ISongChart
{
    ReleaseTitle Game { get; set; }
    ISong Song { get; set; }
    IDifficultyMode DifficultyMode { get; set; }
    string ThumbnailUrl { get; set; }
    string VisualizedBy { get; set; }
    string ToString();
}
