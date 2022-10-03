using Gaming.Domain.SeedWork;

namespace Gaming.Domain.AggregateModels.SongChartAggregate.Ez2on;

public class Ez2OnSongChart : IAggregateRoot, ISongChart
{
    public Ez2OnSongChart(ISong song, ReleaseTitle game, IDifficultyMode difficultyMode)
    {
        Song = song;
        DifficultyMode = difficultyMode;
        Game = game;
        ThumbnailUrl = string.Empty;
        VisualizedBy = string.Empty;
    }

    public ReleaseTitle Game { get; set; }
    public ISong Song { get; set; }

    public IDifficultyMode DifficultyMode { get; set; }
    public string ThumbnailUrl { get; set; }
    public string VisualizedBy { get; set; }

    public override string ToString()
    {
        return
            $"{nameof(Game)}: {Game},{nameof(Song)}:{Song} {nameof(DifficultyMode)}: {DifficultyMode}, {nameof(ThumbnailUrl)}: {ThumbnailUrl}, {nameof(VisualizedBy)}: {VisualizedBy}";
    }
}
