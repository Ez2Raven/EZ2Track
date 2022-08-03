using Gaming.Domain.Aggregates.GameAggregate;
using Gaming.Domain.Aggregates.GameTrackAggregate;
using Gaming.Domain.Aggregates.MusicAggregate;

namespace Crawler.SongScraping.Parsers;

/// <summary>
///     Game Tracks from Ez2DB has a DB Sequence number that will be ignored
///     when persisting to database.
/// </summary>
public sealed class Ez2DbGameTrack : GameTrack
{
    public Ez2DbGameTrack()
    {
    }

    public Ez2DbGameTrack(Song song, Game game, DifficultyMode difficultyMode) : base(song, game,
        difficultyMode)
    {
    }

    public int Ez2OnDbSequenceNumber { get; set; }

    public override string ToString()
    {
        return $"{base.ToString()}, {nameof(Ez2OnDbSequenceNumber)}: {Ez2OnDbSequenceNumber}";
    }
}
