using Gaming.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MusicCatalog.EFCore.Persistence.EntityConfiguration
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