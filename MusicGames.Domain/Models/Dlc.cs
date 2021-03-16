namespace MusicGames.Domain.Models
{
    public class Dlc : IGame
    {
        public string Title { get; set; }

        public bool IsDlc => true;

        public override string ToString()
        {
            return $"{nameof(Title)}: {Title}, {nameof(IsDlc)}: {IsDlc}";
        }
    }
}