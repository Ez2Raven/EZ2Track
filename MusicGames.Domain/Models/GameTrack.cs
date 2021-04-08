namespace MusicGames.Domain.Models
{
    public abstract class GameTrack: Song
    {
        protected GameTrack(Song songDetails, Game game, DifficultyMode difficultyMode):base(songDetails)
        {
            Game = game;
            DifficultyMode = difficultyMode;
        }
        public Game Game { get; set; }
        public DifficultyMode DifficultyMode { get; }
        public string ThumbnailUrl { get; set; }
        public string VisualizedBy { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(Game)}: {Game}, {nameof(DifficultyMode)}: {DifficultyMode}, {nameof(ThumbnailUrl)}: {ThumbnailUrl}, {nameof(VisualizedBy)}: {VisualizedBy}";
        }
    }
}