using Gaming.Domain.AggregateModels.SongChartAggregate.Ez2on;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MusicCatalog.EFCore.Persistence.EntityConfiguration;

public sealed class GameTrackConfiguration : SeedWorkTptConfiguration<Ez2OnSongChart>
{
    public override void Configure(EntityTypeBuilder<Ez2OnSongChart> builder)
    {
        builder
            .HasOne(track => track.Game)
            .WithMany()
            .HasForeignKey(track => track.GameId);

        builder
            .OwnsOne(gameTrack => gameTrack.DifficultyMode,
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
            .HasOne(gameTrack => gameTrack.Song)
            .WithMany()
            .HasForeignKey(gameTrack => gameTrack.SongId);

        builder
            .Property(gameTrack => gameTrack.ThumbnailUrl)
            .HasMaxLength(250);

        builder
            .Property(gameTrack => gameTrack.VisualizedBy)
            .HasMaxLength(250);
    }
}
