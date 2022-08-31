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

            _gameContext.SaveChanges();
            _logger.LogInformation("Seed Successful");
        }

        _logger.LogInformation("End of Seed");
    }
}
