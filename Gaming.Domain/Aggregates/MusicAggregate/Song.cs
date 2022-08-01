using Gaming.Domain.SeedWork;

namespace Gaming.Domain.Aggregates.MusicAggregate
{
    public class Song : Entity
    {
        public Song()
        {
        }

        protected Song(Song songDetails)
        {
            Title = songDetails.Title;
            Album = songDetails.Album;
            Composer = songDetails.Composer;
            Genre = songDetails.Genre;
            Bpm = songDetails.Bpm;
        }

        public string Title { get; set; }
        public string Composer { get; set; }
        public string Album { get; set; }

        public string Genre { get; set; }
        public string Bpm { get; set; }
        
        public override string ToString()
        {
            return
                $"{nameof(Title)}: {Title}, {nameof(Composer)}: {Composer}, {nameof(Album)}: {Album}, {nameof(Genre)}: {Genre}, {nameof(Bpm)}: {Bpm}";
        }
    }
}