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

            BaseGame fakeBaseGame = new BaseGame()
            {
                Title = lorem.Letter(1)
            };

            Dlc fakeDlc = new Dlc()
            {
                Title = lorem.Letter(1)
            };

            Song fakeSong = new Song()
            {
                Title = lorem.Letter(1),
                Composer = lorem.Letter((1)),
                Album = lorem.Letter(1)
            };

            GameTrack fakeGameTrack = new GameTrack()
            {
                Song = fakeSong,
                Game = fakeBaseGame,
                DifficultyTier = DifficultyTier.Level1
            };

            GameTrack fakeDlcTrack = new GameTrack()
            {
                Song = fakeSong,
                Game = fakeDlc,
                DifficultyTier = DifficultyTier.Level2
            };


            SongValidator fakeSongValidator = new SongValidator();
            GameValidator fakeGameValidator = new GameValidator();

            GameMusicValidator mockValidator = new GameMusicValidator(fakeSongValidator, fakeGameValidator);

            var gameMusicResults = mockValidator.Validate(fakeGameTrack);
            var dlcMusicResults = mockValidator.Validate(fakeDlcTrack);

            Assert.True(gameMusicResults.IsValid);
            Assert.True(dlcMusicResults.IsValid);
        }
    }
}