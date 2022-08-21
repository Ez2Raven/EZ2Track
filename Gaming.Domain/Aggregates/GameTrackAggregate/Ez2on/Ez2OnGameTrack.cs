using Gaming.Domain.Aggregates.GameAggregate;
using Gaming.Domain.Aggregates.MusicAggregate;
using Gaming.Domain.SeedWork;

namespace Gaming.Domain.Aggregates.GameTrackAggregate.Ez2on;

public class Ez2OnGameTrack : Entity, IAggregateRoot, IGameTrack
{
    public Ez2OnGameTrack(Song song, Game game, Ez2OnDifficultyMode difficultyMode)
    {
        Song = song;
        DifficultyMode = difficultyMode;
        Game = game;
        ThumbnailUrl = string.Empty;
        VisualizedBy = string.Empty;
    }

    public Ez2OnGameTrack()
    {
        Song = new Song();
        Game = new Game();
        DifficultyMode = new Ez2OnDifficultyMode();
        ThumbnailUrl = string.Empty;
        VisualizedBy = string.Empty;
    }

    public int GameId { get; set; }
    public int SongId { get; set; }

    public IGame Game { get; set; }
    public ISong Song { get; set; }

    public IDifficultyMode DifficultyMode { get; set; }
    public string ThumbnailUrl { get; set; }
    public string VisualizedBy { get; set; }

    public override string ToString()
    {
        return
            $"{base.ToString()}, {nameof(GameId)}: {GameId}, {nameof(DifficultyMode)}: {DifficultyMode}, {nameof(ThumbnailUrl)}: {ThumbnailUrl}, {nameof(VisualizedBy)}: {VisualizedBy}";
    }
}
