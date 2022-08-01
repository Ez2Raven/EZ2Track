using System;
using System.Collections.Generic;
using Gaming.Domain.SeedWork;

namespace Gaming.Domain.Aggregates.GameTrackAggregate
{
    public class Playlist<T> : Entity, IAggregateRoot
    {
        public string Name { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime DateTimeModified { get; set; }

        public List<T> Songs { get; set; } = new();
    }
}