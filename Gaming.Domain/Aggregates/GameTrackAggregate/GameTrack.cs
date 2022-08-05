using Gaming.Domain.Aggregates.GameAggregate;
using Gaming.Domain.Aggregates.MusicAggregate;
using Gaming.Domain.SeedWork;

namespace Gaming.Domain.Aggregates.GameTrackAggregate;

public class GameTrack : Entity, IAggregateRoot, IGameTrack
{
    public GameTrack(Song song, Game game, DifficultyMode difficultyMode)
    {
        Song = song;
        DifficultyMode = difficultyMode;
        Game = game;
        ThumbnailUrl = string.Empty;
        VisualizedBy = string.Empty;
    }

    public GameTrack()
    {
        Song = new Song();
        Game = new Game();
        DifficultyMode = new DifficultyMode();
        ThumbnailUrl = string.Empty;
        VisualizedBy = string.Empty;
    }

    public int GameId { get; set; }
    public int SongId { get; set; }

    public Game Game { get; set; }
    public Song Song { get; set; }

    public DifficultyMode DifficultyMode { get; set; }
    public string ThumbnailUrl { get; set; }
    public string VisualizedBy { get; set; }

    public override string ToString()
    {
        return
            $"{base.ToString()}, {nameof(GameId)}: {GameId}, {nameof(DifficultyMode)}: {DifficultyMode}, {nameof(ThumbnailUrl)}: {ThumbnailUrl}, {nameof(VisualizedBy)}: {VisualizedBy}";
    }
}
