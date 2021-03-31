namespace MusicGames.Domain.Models
{
    public interface IGameTrack: ISong
    {
        IGame Game { get; set; }
        DifficultyMode DifficultyMode { get; }
        string ThumbnailUrl { get; set; }
        string VisualizedBy { get; set; }
    }
}