namespace MusicGames.Domain.Models
{
    public class Ez2OnGameTrack : Song, IGameTrack
    {
        /// <summary>
        /// Instantiate a new instance of Ez2On Game Track based on a Game Song's metadata 
        /// </summary>
        /// <param name="songDetails"></param>
        /// <param name="game"></param>
        public Ez2OnGameTrack(Song songDetails, IGame game, DifficultyMode difficultyMode): base(songDetails)
        {
            Game = game;
            DifficultyMode = difficultyMode;
        }
        
        public int Ez2OnDbSequenceNumber { get; set; }
        
        public IGame Game { get; set; }

        public DifficultyMode DifficultyMode { get; }

        public string ThumbnailUrl { get; set; }

        public string VisualizedBy { get; set; }
        
        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(Ez2OnDbSequenceNumber)}: {Ez2OnDbSequenceNumber}, {nameof(Game)}: {Game}, {nameof(DifficultyMode)}: {DifficultyMode}, {nameof(ThumbnailUrl)}: {ThumbnailUrl}, {nameof(VisualizedBy)}: {VisualizedBy}";
        }
    }
}