namespace MusicGames.Domain.Models
{
    public class GameTrack
    {
        public GameTrack(ISong song, IGame game)
        {
            Song = song;
            Game = game;
        }
        
        public ISong Song { get; set; }
        public IGame Game { get; set; }
        public DifficultyTier DifficultyTier { get; set; }
        public string ThumbnailUrl { get; set; }
        
        public string Bpm { get; set; }
        
        public string VisualizedBy { get; set; }
        
        public override string ToString()
        {
            return $"{nameof(Song)}:[{Song}], {nameof(Game)}:[{Game}], {nameof(DifficultyTier)}: {DifficultyTier}, {nameof(Bpm)}: {Bpm}, {nameof(VisualizedBy)}: {VisualizedBy}";
        }
    }
}