namespace MusicGames.Domain.Models
{
    public class GameTrack
    {
        public Song Song { get; set; }
        public IGame Game { get; set; }
        public DifficultyTier DifficultyTier { get; set; }

        public override string ToString()
        {
            return $"{nameof(Song)}: {Song}, {nameof(Game)}: {Game}, {nameof(DifficultyTier)}: {DifficultyTier}";
        }
    }
}