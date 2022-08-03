using Gaming.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MusicCatalog.EFCore.Persistence.EntityConfiguration;

public class TablePerHierarchyConfiguration<T> : IEntityTypeConfiguration<T> where T : Entity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder
            .HasKey(entity => entity.Id);

        builder
            .HasIndex(e => e.UniversalId)
            .IsUnique()
            .HasDatabaseName($"Index_{typeof(T).Name}_WebApiLookupRef");

        builder
            .Property(e => e.RowVersion)
            .IsRowVersion();
    }
}
