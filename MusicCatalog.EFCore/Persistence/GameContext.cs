using Gaming.Domain.Aggregates.GameAggregate;
using Gaming.Domain.Aggregates.GameTrackAggregate.Ez2on;
using Gaming.Domain.Aggregates.MusicAggregate;
using Microsoft.EntityFrameworkCore;
using MusicCatalog.EFCore.Persistence.EntityConfiguration;

namespace MusicCatalog.EFCore.Persistence;

public class GameContext : DbContext
{
    public GameContext(DbContextOptions<GameContext> options) : base(options)
    {
    }

    public DbSet<Game> Games => Set<Game>();
    public DbSet<Song> Songs => Set<Song>();
    public DbSet<Ez2OnGameTrack> GameTracks => Set<Ez2OnGameTrack>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new GameConfiguration().Configure(modelBuilder.Entity<Game>());
        new SongConfiguration().Configure(modelBuilder.Entity<Song>());
        new GameTrackConfiguration().Configure(modelBuilder.Entity<Ez2OnGameTrack>());
    }
}
