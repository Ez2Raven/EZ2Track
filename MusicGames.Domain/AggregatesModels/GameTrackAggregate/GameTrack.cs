using MusicGames.Domain.AggregatesModels.MusicAggregate;
using MusicGames.Domain.SeedWork;

namespace MusicGames.Domain.AggregatesModels.GameTrackAggregate
{
    public abstract class GameTrack : Song, IAggregateRoot
    {
        protected GameTrack(Song songDetails, int gameId, DifficultyMode difficultyMode) : base(songDetails)
        {
            DifficultyMode = difficultyMode;
            GameId = gameId;
        }

        public int GameId { get; }
        public DifficultyMode DifficultyMode { get; }
        public string ThumbnailUrl { get; set; }
        public string VisualizedBy { get; set; }

        public override string ToString()
        {
            return
                $"{base.ToString()}, {nameof(GameId)}: {GameId}, {nameof(DifficultyMode)}: {DifficultyMode}, {nameof(ThumbnailUrl)}: {ThumbnailUrl}, {nameof(VisualizedBy)}: {VisualizedBy}";
        }
    }
}