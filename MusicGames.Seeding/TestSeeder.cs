using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MusicGames.Domain.AggregatesModels.GameAggregate;
using MusicGames.Domain.AggregatesModels.GameTrackAggregate;
using MusicGames.Domain.AggregatesModels.MusicAggregate;
using SongScraping.Infrastructure.Persistence;

namespace MusicGames.Seeding
{
    public class TestSeeder : ISeeding
    {
        private readonly ILogger<TestSeeder> _logger;
        private readonly GameContext _gameContext;
        private readonly Ez2OnGameTrackContext _ez2OnGameTrackContext;

        public TestSeeder(ILogger<TestSeeder> logger, GameContext gameContext, Ez2OnGameTrackContext ez2OnGameTrackContext)
        {
            _logger = logger;
            _gameContext = gameContext;
            _ez2OnGameTrackContext = ez2OnGameTrackContext;
        }
        public void Seed()
        {
            Game firstGameFromBoundedContext = null;
            
            _logger.LogInformation("Beginning Seed");
            using (_gameContext)
            {
                _gameContext.Database.Migrate();
                firstGameFromBoundedContext = _gameContext.Games.FirstOrDefault(b => b.Title == "First Game");
                if (firstGameFromBoundedContext == null)
                {
                    firstGameFromBoundedContext = new Game
                    {
                        Title = "First Game",
                        IsDlc = false,
                        ExternalId = Guid.NewGuid()
                    };
                    
                    _gameContext.Games.Add(firstGameFromBoundedContext);
                }

                _gameContext.SaveChanges();
            }
            
            using (_ez2OnGameTrackContext)
            {
                _ez2OnGameTrackContext.Database.Migrate();
               
                var firstSong = _ez2OnGameTrackContext.Songs.FirstOrDefault(s => s.Title == "First Song");
                if (firstSong == null)
                {
                    firstSong = new Song()
                    {
                        Title = "First Song",
                        Album = "First Album",
                        Bpm = "90 ~ 120",
                        Composer = "First Composer",
                        Genre = "First Genre",
                        ExternalId = Guid.NewGuid()
                    };
                    _ez2OnGameTrackContext.Songs.Add(firstSong);
                }
                
                var firstGameTrack = _ez2OnGameTrackContext.Ez2OnGameTracks.FirstOrDefault(gt => gt.Id == 1);
                if (firstGameTrack == null)
                {
                    firstGameTrack = new Ez2OnGameTrack(
                        firstSong,
                        firstGameFromBoundedContext.Id,
                        new DifficultyMode() {Category = DifficultyCategory.SuperHard, Level = 20})
                    {
                        Ez2OnDbSequenceNumber = 1,
                        ExternalId = Guid.NewGuid(),
                        SongId = firstSong.Id
                    }; 
                    
                    _ez2OnGameTrackContext.Ez2OnGameTracks.Add(firstGameTrack);
                }

                _ez2OnGameTrackContext.SaveChanges();
                _logger.LogInformation("End of Seed");
            }
        }
    }
}