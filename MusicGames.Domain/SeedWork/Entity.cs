using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicGames.Domain.SeedWork
{
    public class Entity
    {
        private int? _id = null;

        public int? Id
        {
            get => _id;
            set => _id = value;
        }

        public Guid ExternalId { get; set; }

        /// <summary>
        /// Tracking column to implement database optimistic concurrency control.
        /// </summary>
        public byte[] RowVersion { get; set; }

        private sealed class ExternalIdEqualityComparer : IEqualityComparer<Entity>
        {
            public bool Equals(Entity x, Entity y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.ExternalId.Equals(y.ExternalId);
            }

            public int GetHashCode(Entity obj)
            {
                return obj.ExternalId.GetHashCode();
            }
        }

        /// <summary>
        /// Default Equality Comparer: Compare Equality via Guid.
        /// </summary>
        public static IEqualityComparer<Entity> ExternalIdComparer { get; } = new ExternalIdEqualityComparer();
    }
}