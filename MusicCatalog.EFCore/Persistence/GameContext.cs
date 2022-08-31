using Gaming.Domain.AggregateModels.SongChartAggregate;
using Gaming.Domain.AggregateModels.SongChartAggregate.Ez2on;
using Microsoft.EntityFrameworkCore;
using MusicCatalog.EFCore.Persistence.EntityConfiguration;

namespace MusicCatalog.EFCore.Persistence;

public class GameContext : DbContext
{
    public GameContext(DbContextOptions<GameContext> options) : base(options)
    {
    }

    public DbSet<ReleaseTitle> Games => Set<ReleaseTitle>();
    public DbSet<Song> Songs => Set<Song>();
    public DbSet<Ez2OnSongChart> GameTracks => Set<Ez2OnSongChart>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new GameConfiguration().Configure(modelBuilder.Entity<ReleaseTitle>());
        new SongConfiguration().Configure(modelBuilder.Entity<Song>());
        new GameTrackConfiguration().Configure(modelBuilder.Entity<Ez2OnSongChart>());
    }
}
