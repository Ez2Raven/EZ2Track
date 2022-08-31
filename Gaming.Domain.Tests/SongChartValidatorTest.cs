using Bogus;
using Gaming.Domain.AggregateModels.SongChartAggregate;
using Gaming.Domain.AggregateModels.SongChartAggregate.Ez2on;
using Gaming.Domain.AggregateModels.SongChartAggregate.Validators;
using Xunit;
using Xunit.Abstractions;

namespace Gaming.Domain.Tests;

public class SongChartValidatorTest
{
    private readonly ITestOutputHelper _output;
    private readonly Faker _randomFluent;

    public SongChartValidatorTest(ITestOutputHelper output)
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

        var fakeGame = Ez2OnReleaseTitle.Platinum;

        var fakeSong = new Song
        {
            Title = _randomFluent.Lorem.Letter(),
            Composer = _randomFluent.Lorem.Letter(),
            Album = _randomFluent.Lorem.Letter()
        };
        var randomFluent = new Faker {Random = new Randomizer(1080)};
        var fakeMode = new Ez2OnDifficultyMode();
        fakeMode.Level = randomFluent.Random.Int(1, 20);
        fakeMode.Category = DifficultyCategory.Easy;

        var fakeGameTrack = new Ez2OnSongChart(fakeSong, fakeGame, fakeMode);

        var fakeSongValidator = new SongValidator();

        var fakeGameValidator = new GameValidator();

        var mockValidator = new SongChartValidator(fakeSongValidator, fakeGameValidator);

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
