using System;
using System.Collections.Generic;

namespace MusicGames.Domain.SeedWork
{
    public class Entity
    {
        public int Id { get; set; }

        public Guid ExternalId { get; set; }

        /// <summary>
        /// Tracking column to implement database optimistic concurrency control.
        /// </summary>
        public int RowVersion { get; set; }

        private sealed class IdExternalIdRowVersionEqualityComparer : IEqualityComparer<Entity>
        {
            public bool Equals(Entity x, Entity y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Id == y.Id && x.ExternalId.Equals(y.ExternalId) && x.RowVersion == y.RowVersion;
            }

            public int GetHashCode(Entity obj)
            {
                return HashCode.Combine(obj.Id, obj.ExternalId, obj.RowVersion);
            }
        }

        public static IEqualityComparer<Entity> IdExternalIdRowVersionComparer { get; } = new IdExternalIdRowVersionEqualityComparer();
    }
}