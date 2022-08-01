using Gaming.Domain.Aggregates.GameAggregate;
using Gaming.Domain.Aggregates.GameTrackAggregate;
using Gaming.Domain.Aggregates.MusicAggregate;
using Microsoft.EntityFrameworkCore;
using MusicCatalog.EFCore.Persistence.EntityConfiguration;

namespace MusicCatalog.EFCore.Persistence
{
    public class GameContext:DbContext
    {
        public GameContext(DbContextOptions<GameContext> options):base(options)
        {
            
        }
        
        public DbSet<Game> Games { get; set; }
        public DbSet<Song> Songs { get; set; }
        
        public DbSet<GameTrack> GameTracks { get; set; }
        
        public DbSet<Ez2OnGameTrack> Ez2OnGameTracks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new GameConfiguration().Configure(modelBuilder.Entity<Game>());
            new SongConfiguration().Configure(modelBuilder.Entity<Song>());
            new GameTrackConfiguration().Configure(modelBuilder.Entity<GameTrack>());
            new Ez2OnGameTrackConfiguration().Configure(modelBuilder.Entity<Ez2OnGameTrack>());
        }
    }
}