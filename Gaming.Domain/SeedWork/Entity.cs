using System;
using System.Collections.Generic;

namespace Gaming.Domain.SeedWork;

public class Entity
{
    /// <summary>
    ///     Identifier that is used within the local database for Joining tables
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     Universal identifier that is only to be used for integration between systems
    /// </summary>
    public Guid UniversalId { get; set; }

    /// <summary>
    ///     Tracking column to implement database optimistic concurrency control.
    /// </summary>
    public int RowVersion { get; set; }

    public static IEqualityComparer<Entity> IdExternalIdRowVersionComparer { get; } =
        new IdExternalIdRowVersionEqualityComparer();

    private sealed class IdExternalIdRowVersionEqualityComparer : IEqualityComparer<Entity>
    {
        public bool Equals(Entity? x, Entity? y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(x, null))
            {
                return false;
            }

            if (ReferenceEquals(y, null))
            {
                return false;
            }

            if (x.GetType() != y.GetType())
            {
                return false;
            }

            return x.Id == y.Id && x.UniversalId.Equals(y.UniversalId) && x.RowVersion == y.RowVersion;
        }

        public int GetHashCode(Entity? obj)
        {
            return HashCode.Combine(obj?.Id, obj?.UniversalId, obj?.RowVersion);
        }
    }
}
