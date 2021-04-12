using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicGames.Domain.AggregatesModels.GameAggregate;

namespace SongScraping.Infrastructure.Persistence.EntityConfiguration
{
    public sealed class GameConfiguration : TablePerTypeConfiguration<Game>
    {
        public override void Configure(EntityTypeBuilder<Game> builder)
        {
            builder
                .Property(game => game.Title)
                .HasMaxLength(250)
                .IsRequired();
            builder
                .Property(game => game.IsDlc)
                .IsRequired();
            
            base.Configure(builder);
        }
    }
}