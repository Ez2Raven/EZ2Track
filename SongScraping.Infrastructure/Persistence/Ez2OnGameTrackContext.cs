using Microsoft.EntityFrameworkCore;
using MusicGames.Domain.AggregatesModels.GameAggregate;
using MusicGames.Domain.AggregatesModels.GameTrackAggregate;
using MusicGames.Domain.AggregatesModels.MusicAggregate;
using SongScraping.Infrastructure.Persistence.EntityConfiguration;

namespace SongScraping.Infrastructure.Persistence
{
    public class Ez2OnGameTrackContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Ez2OnGameTrack> Ez2OnGameTracks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=streamer-site.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new GameConfiguration().Configure(modelBuilder.Entity<Game>());
            new SongConfiguration().Configure(modelBuilder.Entity<Song>());
            new Ez2OnGameTrackConfiguration().Configure(modelBuilder.Entity<Ez2OnGameTrack>());
        }
    }
}