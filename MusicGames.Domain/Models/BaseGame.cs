namespace MusicGames.Domain.Models
{
    public class BaseGame : IGame
    {
        public string Title { get; set; }
        public bool IsDlc => false;

        public override string ToString()
        {
            return $"{nameof(Title)}: {Title}, {nameof(IsDlc)}: {IsDlc}";
        }
    }
}