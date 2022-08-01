﻿using Gaming.Domain.SeedWork;

namespace Gaming.Domain.Aggregates.GameAggregate
{
    public class Game : Entity
    {
        public string Title { get; set; }
        public bool IsDlc { get; set; }

        public override string ToString()
        {
            return $"{nameof(Title)}: {Title}, {nameof(IsDlc)}: {IsDlc}";
        }
    }
}