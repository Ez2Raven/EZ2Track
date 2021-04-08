using System;
using System.Collections.Generic;
using MusicGames.Domain.SeedWork;

namespace MusicGames.Domain.AggregatesModels.GameTrackAggregate
{
    public class Playlist<T> : Entity, IAggregateRoot
    {
        public string Name { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime DateTimeModified { get; set; }

        public List<T> Songs { get; set; } = new();
    }
}