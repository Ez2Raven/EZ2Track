using System;
using System.Linq;
using Crawler.SongScraping.Parsers.Ez2Db;
using Gaming.Domain.Aggregates.GameAggregate;
using Gaming.Domain.Aggregates.GameTrackAggregate;
using Gaming.Domain.Aggregates.MusicAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MusicCatalog.EFCore.Persistence;

namespace MusicCatalog.Migrations;

public class TestSeeder : ISeeding
{
    private readonly GameContext _gameContext;
    private readonly ILogger<TestSeeder> _logger;

    public TestSeeder(ILogger<TestSeeder> logger, GameContext gameContext)
    {
        _logger = logger;
        _gameContext = gameContext;
    }

    public void Seed()
    {
        _logger.LogInformation("Beginning Seed");
        using (_gameContext)
        {
            // apply migration during startup for easy development
            _gameContext.Database.Migrate();

            var firstGame = _gameContext.Games.FirstOrDefault(b => b.Title == "First Game");
            if (firstGame == null)
            {
                firstGame = new Game {Title = "First Game", IsDlc = false, UniversalId = Guid.NewGuid()};

                _gameContext.Games.Add(firstGame);
            }

            var firstSong = _gameContext.Songs.FirstOrDefault(s => s.Title == "First Song");
            if (firstSong == null)
            {
                firstSong = new Song
                {
                    Title = "First Song",
                    Album = "First Album",
                    Bpm = "90 ~ 120",
                    Composer = "First Composer",
                    Genre = "First Genre",
                    UniversalId = Guid.NewGuid()
                };
                _gameContext.Songs.Add(firstSong);
            }

            var firstGameTrack = _gameContext.GameTracks.FirstOrDefault(gt => gt.Id == 1);
            if (firstGameTrack == null)
            {
                firstGameTrack = new Ez2DbGameTrack(
                    firstSong,
                    firstGame,
                    new DifficultyMode {Category = DifficultyCategory.SuperHard, Level = 20})
                {
                    Ez2OnDbSequenceNumber = 1, UniversalId = Guid.NewGuid(), SongId = firstSong.Id
                };

                _gameContext.GameTracks.Add(firstGameTrack);
            }

            _gameContext.SaveChanges();
            _logger.LogInformation("Seed Successful");
        }

        _logger.LogInformation("End of Seed");
    }
}
