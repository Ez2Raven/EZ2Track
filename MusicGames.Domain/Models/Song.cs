namespace MusicGames.Domain.Models
{
 
    public class Song
    {
        public string Title { get; set; }
        public string Composer { get; set; }
        public string Album { get; set; }

        public override string ToString()
        {
            return $"{nameof(Title)}: {Title}, {nameof(Composer)}: {Composer}, {nameof(Album)}: {Album}";
        }
    }
}