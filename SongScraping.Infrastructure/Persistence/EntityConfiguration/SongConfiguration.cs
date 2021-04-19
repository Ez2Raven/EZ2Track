using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicGames.Domain.AggregatesModels.MusicAggregate;

namespace SongScraping.Infrastructure.Persistence.EntityConfiguration
{
    public sealed class SongConfiguration : SeedWorkTptConfiguration<Song>
    {
        public override void Configure(EntityTypeBuilder<Song> builder)
        {
            builder
                .Property(song => song.Title)
                .HasMaxLength(250)
                .IsRequired();

            builder
                .Property(song => song.Album)
                .HasMaxLength(250);

            builder
                .Property(song => song.Bpm)
                .HasMaxLength(20);

            builder
                .Property(song => song.Composer)
                .HasMaxLength(250);

            builder
                .Property(song => song.Genre)
                .HasMaxLength(250);
            
            base.Configure(builder);
        }
    }
}