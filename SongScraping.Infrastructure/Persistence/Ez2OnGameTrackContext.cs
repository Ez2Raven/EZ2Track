using Microsoft.EntityFrameworkCore;
using MusicGames.Domain.AggregatesModels.GameAggregate;
using MusicGames.Domain.AggregatesModels.GameTrackAggregate;
using MusicGames.Domain.AggregatesModels.MusicAggregate;
using SongScraping.Infrastructure.Persistence.EntityConfiguration;

namespace SongScraping.Infrastructure.Persistence
{
    public class Ez2OnGameTrackContext : DbContext
    {
        public Ez2OnGameTrackContext(DbContextOptions<Ez2OnGameTrackContext> options)
        : base(options)
        {
            
        }
        
        public DbSet<Song> Songs { get; set; }
        
        public DbSet<GameTrack> GameTracks { get; set; }
        
        public DbSet<Ez2OnGameTrack> Ez2OnGameTracks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new SongConfiguration().Configure(modelBuilder.Entity<Song>());
            new GameTrackConfiguration().Configure(modelBuilder.Entity<GameTrack>());
            new Ez2OnGameTrackConfiguration().Configure(modelBuilder.Entity<Ez2OnGameTrack>());
        }
    }
}