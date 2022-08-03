using Bogus;
using Gaming.Domain.Aggregates.GameAggregate;
using Gaming.Domain.Aggregates.GameTrackAggregate;
using Gaming.Domain.Aggregates.MusicAggregate;
using Xunit;
using Xunit.Abstractions;

namespace Gaming.Domain.Test;

public class GameTrackValidatorTest
{
    private readonly ITestOutputHelper _output;
    private readonly Faker _randomFluent;

    public GameTrackValidatorTest(ITestOutputHelper output)
    {
        _output = output;
        _randomFluent = new Faker {Random = new Randomizer(1080)};
    }

    [Theory]
    [InlineData("en")]
    [InlineData("ja")]
    [InlineData("ko")]
    public void GameTrack_MustBeValid_WithAbstractedGameId(string locale)
    {
        _randomFluent.Lorem.Locale = locale;

        var fakeGame = new Game
        {
            Title = _randomFluent.Hacker.Phrase(),
            IsDlc = _randomFluent.Random.Bool(),
            Id = _randomFluent.Random.Int(1)
        };

        var fakeSong = new Song
        {
            Title = _randomFluent.Lorem.Letter(),
            Composer = _randomFluent.Lorem.Letter(),
            Album = _randomFluent.Lorem.Letter()
        };
        var randomFluent = new Faker {Random = new Randomizer(1080)};
        var fakeMode = new DifficultyMode();
        fakeMode.Level = randomFluent.Random.Int(1, 20);
        fakeMode.Category = DifficultyCategory.Easy;

        var fakeGameTrack = new GameTrack(fakeSong, fakeGame, fakeMode);

        var fakeSongValidator = new SongValidator();

        var fakeGameValidator = new GameValidator();

        var mockValidator = new GameTrackValidator(fakeSongValidator, fakeGameValidator);

        var validationResult = mockValidator.Validate(fakeGameTrack);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                _output.WriteLine(error.ToString());
            }
        }

        Assert.True(validationResult.IsValid);
    }
}
