using Gaming.Domain.Aggregates.MusicAggregate;

namespace Gaming.Domain.Aggregates.GameTrackAggregate
{
    public sealed class Ez2OnGameTrack : GameTrack
    {
        public Ez2OnGameTrack()
        {
        }
        public Ez2OnGameTrack(Song song, int gameId, DifficultyMode difficultyMode) : base(song, gameId,
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