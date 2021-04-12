using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicGames.Domain.AggregatesModels.GameTrackAggregate;

namespace SongScraping.Infrastructure.Persistence.EntityConfiguration
{
    public sealed class Ez2OnGameTrackConfiguration : TablePerTypeConfiguration<Ez2OnGameTrack>
    {
        public override void Configure(EntityTypeBuilder<Ez2OnGameTrack> builder)
        {
            builder
                .Property(gameTrack => gameTrack.GameId)
                .IsRequired();

            builder
                .OwnsOne(gt => gt.DifficultyMode,
                    dm =>
                    {
                        dm.Property(dm => dm.Category)
                            .HasMaxLength(50)
                            .IsRequired();

                        dm.Property(dm => dm.Level)
                            .HasMaxLength(5)
                            .IsRequired();
                    });

            builder
                .Property(gameTrack => gameTrack.ThumbnailUrl)
                .HasMaxLength(250);

            builder
                .Property(gameTrack => gameTrack.VisualizedBy)
                .HasMaxLength(250);

            base.Configure(builder);
        }
    }
}