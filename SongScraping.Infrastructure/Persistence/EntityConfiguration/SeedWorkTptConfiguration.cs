using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicGames.Domain.SeedWork;

namespace SongScraping.Infrastructure.Persistence.EntityConfiguration
{
    public abstract class SeedWorkTptConfiguration<T>:TablePerTypeConfiguration<T> where T : Entity
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            builder
                .HasKey(entity => entity.Id);
            
            builder
                .HasIndex(e => e.ExternalId)
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