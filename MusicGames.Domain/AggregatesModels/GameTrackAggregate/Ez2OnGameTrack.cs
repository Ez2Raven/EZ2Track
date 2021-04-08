using MusicGames.Domain.AggregatesModels.MusicAggregate;

namespace MusicGames.Domain.AggregatesModels.GameTrackAggregate
{
    public class Ez2OnGameTrack : GameTrack
    {
        public Ez2OnGameTrack(Song songDetails, int gameId, DifficultyMode difficultyMode) : base(songDetails, gameId,
            difficultyMode)
        {
        }

        public int Ez2OnDbSequenceNumber { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(Ez2OnDbSequenceNumber)}: {Ez2OnDbSequenceNumber}";
        }
    }
}