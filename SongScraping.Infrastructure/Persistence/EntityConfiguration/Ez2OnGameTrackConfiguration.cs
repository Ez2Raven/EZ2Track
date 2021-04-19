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
                .Property(track => track.Ez2OnDbSequenceNumber)
                .IsRequired();
            base.Configure(builder);     
        }
    }
}