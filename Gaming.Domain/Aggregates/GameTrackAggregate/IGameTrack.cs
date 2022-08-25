using Gaming.Domain.Aggregates.GameAggregate;
using Gaming.Domain.Aggregates.MusicAggregate;

namespace Gaming.Domain.Aggregates.GameTrackAggregate;

public interface IGameTrack
{
    IGame Game { get; set; }
    ISong Song { get; set; }
    IDifficultyMode DifficultyMode { get; set; }
    string ThumbnailUrl { get; set; }
    string VisualizedBy { get; set; }
    string ToString();
}
