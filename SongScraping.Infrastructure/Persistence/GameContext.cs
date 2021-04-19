using Microsoft.EntityFrameworkCore;
using MusicGames.Domain.AggregatesModels.GameAggregate;
using SongScraping.Infrastructure.Persistence.EntityConfiguration;

namespace SongScraping.Infrastructure.Persistence
{
    public class GameContext:DbContext
    {
        public GameContext(DbContextOptions<GameContext> options):base(options)
        {
            
        }
        
        public DbSet<Game> Games { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new GameConfiguration().Configure(modelBuilder.Entity<Game>());
        }
    }
}