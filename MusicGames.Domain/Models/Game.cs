using System.Collections.Generic;

namespace MusicGames.Domain.Models
{
    public class Game
    {
        public string Title { get; set; }
        public bool IsDlc { get; set; }
        public List<GameTrack> GameTracks { get; } = new();

        public override string ToString()
        {
            return $"{nameof(Title)}: {Title}, {nameof(IsDlc)}: {IsDlc}";
        }
    }
}