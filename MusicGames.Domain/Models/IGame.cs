namespace MusicGames.Domain.Models
{
    public interface IGame
    {
        public string Title { get; set; }
        public bool IsDlc { get; }
    }
}