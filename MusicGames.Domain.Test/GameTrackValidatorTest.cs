using Bogus;
using MusicGames.Domain.AggregatesModels.GameTrackAggregate;
using MusicGames.Domain.AggregatesModels.MusicAggregate;
using Xunit;
using Xunit.Abstractions;

namespace MusicGames.Domain.Test
{
    public class GameTrackValidatorTest
    {
        private readonly ITestOutputHelper _output;
        private readonly Faker _randomFluent;

        public GameTrackValidatorTest(ITestOutputHelper output)
        {
            _output = output;
            _randomFluent = new Faker
            {
                Random = new Randomizer(1080)
            };
        }

        [Theory]
        [InlineData("en")]
        [InlineData("ja")]
        [InlineData("ko")]
        public void GameTrack_MustBeValid_WithAbstractedGameId(string locale)
        {
            var fakeGameId = _randomFluent.Random.Int(0, 100);

            var fakeSong = new Song
            {
                Title = _randomFluent.Lorem.Letter(),
                Composer = _randomFluent.Lorem.Letter(),
                Album = _randomFluent.Lorem.Letter()
            };
            var randomFluent = new Faker
            {
                Random = new Randomizer(1080)
            };
            var fakeMode = new DifficultyMode();
            fakeMode.Level = randomFluent.Random.Int(1, 20);
            fakeMode.Category = DifficultyCategory.Easy;

            var fakeGameTrack = new Ez2OnGameTrack(fakeSong, fakeGameId, fakeMode);

            var fakeSongValidator = new SongValidator();

            var mockValidator = new GameTrackValidator(fakeSongValidator);

            var validationResult = mockValidator.Validate(fakeGameTrack);
            if (!validationResult.IsValid)
                foreach (var error in validationResult.Errors)
                    _output.WriteLine(error.ToString());
            Assert.True(validationResult.IsValid);
        }
    }
}