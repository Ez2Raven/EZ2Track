using System;
using System.Collections.Generic;

namespace MusicGames.Domain.Models
{
    public class GameTrackPlaylist:List<IGameTrack>
    {
        public string Name { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime DateTimeModified { get; set; }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(DateTimeCreated)}: {DateTimeCreated}, {nameof(DateTimeModified)}: {DateTimeModified}";
        }
    }
}