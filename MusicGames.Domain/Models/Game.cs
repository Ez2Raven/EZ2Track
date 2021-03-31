namespace MusicGames.Domain.Models
{
    public class Game : IGame
    {
        public string Title { get; set; }
        public bool IsDlc { get; set; }
    
        public override string ToString()
        {
            return $"{nameof(Title)}: {Title}, {nameof(IsDlc)}: {IsDlc}";
        }
    }
}