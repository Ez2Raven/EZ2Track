using Gaming.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MusicCatalog.EFCore.Persistence.EntityConfiguration
{
    public abstract class SeedWorkTptConfiguration<T>:TablePerTypeConfiguration<T> where T : Entity
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            builder
                .HasKey(entity => entity.Id);
            
            builder
                .HasIndex(e => e.UniversalId)
                .IsUnique()
                .HasDatabaseName($"Index_{typeof(T).Name}_WebApiLookupRef");
            
            builder
                .Property(e => e.RowVersion)
                .HasDefaultValue(0)
                .IsRowVersion();
            
            base.Configure(builder);
        }
    }
}