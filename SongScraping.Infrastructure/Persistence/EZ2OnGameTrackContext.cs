using Microsoft.EntityFrameworkCore;

namespace MusicGames.SongScraping.Persistence
{
    public class Ez2OnGameTrackContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=C:\streamer-site.db");
        }
    }
}