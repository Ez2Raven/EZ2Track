namespace MusicGames.Domain.Models
{
    public class Ez2OnGameTrack : GameTrack
    {
        public Ez2OnGameTrack(Song songDetails, Game game, DifficultyMode difficultyMode): base(songDetails, game, difficultyMode)
        {
        }
        
        public int Ez2OnDbSequenceNumber { get; set; }
        
        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(Ez2OnDbSequenceNumber)}: {Ez2OnDbSequenceNumber}, {nameof(Game)}: {Game}, {nameof(DifficultyMode)}: {DifficultyMode}, {nameof(ThumbnailUrl)}: {ThumbnailUrl}, {nameof(VisualizedBy)}: {VisualizedBy}";
        }
    }
}