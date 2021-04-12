using System;
using System.Linq;
using MusicGames.Domain.AggregatesModels.GameAggregate;
using MusicGames.Domain.AggregatesModels.GameTrackAggregate;
using MusicGames.Domain.AggregatesModels.MusicAggregate;
using SongScraping.Infrastructure.Persistence;

namespace MusicGames.Seeding
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new Ez2OnGameTrackContext())
            {
                context.Database.EnsureCreated();

                var firstGame = context.Games.FirstOrDefault(b => b.Title == "First Game");
                if (firstGame == null)
                {
                    context.Games.Add(new Game
                    {
                        Title = "First Game",
                        IsDlc = false,
                        ExternalId = Guid.NewGuid()
                    });
                }

                var firstSong = context.Songs.FirstOrDefault(s => s.Title == "First Song");
                if (firstSong == null)
                {
                    context.Songs.Add(new Song()
                    {
                        Title = "First Song",
                        Album = "First Album",
                        Bpm = "90 ~ 120",
                        Composer = "First Composer",
                        Genre = "First Genre",
                        ExternalId = Guid.NewGuid()
                    });
                }

                var firstGameTrack = context.Ez2OnGameTracks.FirstOrDefault(gt => gt.Id == 1);
                if (firstGameTrack == null)
                {
                    context.Ez2OnGameTracks.Add(
                        new Ez2OnGameTrack(
                            firstSong,
                            1,
                            new DifficultyMode() {Category = DifficultyCategory.SuperHard, Level = 20})
                    {
                        Ez2OnDbSequenceNumber = 1,
                        ExternalId = Guid.NewGuid()
                    });
                }
                context.SaveChanges();
            }
        }
    }
}