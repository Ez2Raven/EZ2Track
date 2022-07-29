using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicGames.Domain.AggregatesModels.GameAggregate;
using MusicGames.Domain.AggregatesModels.GameTrackAggregate;
using MusicGames.Domain.AggregatesModels.MusicAggregate;

namespace SongScraping.Infrastructure.Persistence.EntityConfiguration
{
    public sealed class GameTrackConfiguration : SeedWorkTptConfiguration<GameTrack>
    {
        public override void Configure(EntityTypeBuilder<GameTrack> builder)
        {
            builder
                .Property(gameTrack => gameTrack.GameId)
                .IsRequired();

            builder
                .OwnsOne(gt => gt.DifficultyMode,
                    dm =>
                    {
                        dm.Property(difficultyMode => difficultyMode.Category)
                            .HasMaxLength(50)
                            .IsRequired();

                        dm.Property(difficultyMode => difficultyMode.Level)
                            .HasMaxLength(5)
                            .IsRequired();
                    });

            builder
                .HasOne(gt => gt.Song)
                .WithMany()
                .HasForeignKey(gt=>gt.SongId);

            builder
                .Property(gameTrack => gameTrack.ThumbnailUrl)
                .HasMaxLength(250);

            builder
                .Property(gameTrack => gameTrack.VisualizedBy)
                .HasMaxLength(250);
        }
    }
}