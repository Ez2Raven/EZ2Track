using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicGames.Domain.SeedWork;

namespace SongScraping.Infrastructure.Persistence.EntityConfiguration
{
    public class TablePerHierarchyConfiguration<T> :IEntityTypeConfiguration<T> where T : Entity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder
                .HasKey(entity => entity.Id);

            builder
                .HasIndex(e => e.ExternalId)
                .IsUnique()
                .HasDatabaseName("Index_WebApiLookupRef");
            
            builder
                .Property(e => e.RowVersion)
                .IsRowVersion();
        }
    }
    
}