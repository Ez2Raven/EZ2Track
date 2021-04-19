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
            builder.ToTable(typeof(T).Name);
        }
    }
}