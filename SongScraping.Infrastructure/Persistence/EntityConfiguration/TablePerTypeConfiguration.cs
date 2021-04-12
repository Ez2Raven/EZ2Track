using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicGames.Domain.SeedWork;

namespace SongScraping.Infrastructure.Persistence.EntityConfiguration
{
    /// <summary>
    /// Handles database configurations for Domain Entities.   
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class TablePerTypeConfiguration<T>:IEntityTypeConfiguration<T> where T : Entity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder
                .HasKey(entity => entity.Id);

            builder
                .HasIndex(e => e.ExternalId)
                .IsUnique()
                .HasDatabaseName($"Index_{typeof(T).Name}_WebApiLookupRef");
            
            builder
                .Property(e => e.RowVersion)
                .IsRowVersion();
            
            builder.ToTable(typeof(T).Name);
        }
    }
}