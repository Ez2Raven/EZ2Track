using System.Reflection;
using Bogus;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using MusicGames.Domain.Models;
using MusicGames.Domain.Validations;
using Xunit;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace MusicGames.Domain.Test
{
    public class GameSongValidatorTest
    {
        [Theory]
        [InlineData("en")]
        [InlineData("ja")]
        [InlineData("ko")]
        public void GameSong_MustBeAssociatedWith_EitherGameOrDlc(string locale)
        {
            var lorem = new Bogus.DataSets.Lorem(locale)
            {
                Random = new Randomizer(1080)
            };

            Game fakeGame = new Game()
            {
                Title = lorem.Letter(1)
            };

            Song fakeSong = new Song()
            {
                Title = lorem.Letter(1),
                Composer = lorem.Letter((1)),
                Album = lorem.Letter(1)
            };
            var randomFluent = new Bogus.Faker()
            {
                Random = new Randomizer(1080)
            };
            DifficultyMode fakeMode = new DifficultyMode();
            fakeMode.Level = randomFluent.Random.Int(1, 20);
            fakeMode.Category = DifficultyCategory.Easy;

            Ez2OnGameTrack fakeGameTrack = new Ez2OnGameTrack(fakeSong, fakeGame, fakeMode);

            SongValidator fakeSongValidator = new SongValidator();
            GameValidator fakeGameValidator = new GameValidator();

            GameTrackValidator mockValidator = new GameTrackValidator(fakeSongValidator, fakeGameValidator);

            var gameMusicResults = mockValidator.Validate(fakeGameTrack);

            Assert.True(gameMusicResults.IsValid);
        }
    }
}