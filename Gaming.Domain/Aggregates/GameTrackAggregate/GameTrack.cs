using Gaming.Domain.Aggregates.MusicAggregate;
using Gaming.Domain.SeedWork;

namespace Gaming.Domain.Aggregates.GameTrackAggregate
{
    public abstract class GameTrack : Entity, IAggregateRoot
    {
        protected GameTrack()
        {
            
        }
        
        protected GameTrack(Song song, int gameId, DifficultyMode difficultyMode)
        {
            Song = song;
            DifficultyMode = difficultyMode;
            GameId = gameId;
        }

        public int GameId { get; set; }
        
        public Song Song { get; set; }
        
        public DifficultyMode DifficultyMode { get; set; }
        public string ThumbnailUrl { get; set; }
        public string VisualizedBy { get; set; }
        public int SongId { get; set; }

        public override string ToString()
        {
            return
                $"{base.ToString()}, {nameof(GameId)}: {GameId}, {nameof(DifficultyMode)}: {DifficultyMode}, {nameof(ThumbnailUrl)}: {ThumbnailUrl}, {nameof(VisualizedBy)}: {VisualizedBy}";
        }
    }
}