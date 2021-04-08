using System;
using Bogus;
using Bogus.DataSets;
using MusicGames.Domain.AggregatesModels;
using MusicGames.Domain.AggregatesModels.GameAggregate;
using MusicGames.Domain.AggregatesModels.GameTrackAggregate;
using MusicGames.Domain.Validations;
using Xunit;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace MusicGames.Domain.Test
{
    public class GamePlayListTest
    {
        private readonly Faker _randomFluent;

        public GamePlayListTest()
        {
            _randomFluent = new Bogus.Faker()
            {
                Random = new Randomizer(1080)
            };
        }

        [Fact]
        public void Validate_GamePlayList_ReturnsTrue()
        {
            GameTrackPlaylist gameTrackPlaylist = new GameTrackPlaylist();
            for (int count = 0; count < 10; count++)
            {
                Song fakeSong = new Song()
                {
                    Title = _randomFluent.Hacker.Phrase(),
                    Album = _randomFluent.Hacker.Phrase(),
                    Composer = _randomFluent.Person.FullName
                };

                int fakeGameId = _randomFluent.Random.Int(0, 100);

                DifficultyMode fakeMode = new DifficultyMode();

                gameTrackPlaylist.Add(new Ez2OnGameTrack(fakeSong, fakeGameId, fakeMode));
            }

            gameTrackPlaylist.Name = _randomFluent.Hacker.Phrase();
            gameTrackPlaylist.DateTimeCreated = _randomFluent.Date.Past(10, DateTime.Now);
            gameTrackPlaylist.DateTimeModified = _randomFluent.Date.Future(10, DateTime.Now);

            GamePlayListValidator validator = new GamePlayListValidator();
            ValidationResult results = validator.Validate(gameTrackPlaylist);
            
            Assert.True(results.IsValid);
        }
        
        [Fact]
        public void Assign_DefaultDateTimeCreated_To_GamePlayList_ReturnsFalse()
        {
            GameTrackPlaylist gameTrackPlaylist = new GameTrackPlaylist();
            for (int count = 0; count < 10; count++)
            {
                Song fakeSong = new Song()
                {
                    Title = _randomFluent.Hacker.Phrase(),
                    Album = _randomFluent.Hacker.Phrase(),
                    Composer = _randomFluent.Person.FullName
                };

                int fakeGameId = _randomFluent.Random.Int(0, 100);
                
                DifficultyMode fakeMode = new DifficultyMode();

                gameTrackPlaylist.Add(new Ez2OnGameTrack(fakeSong, fakeGameId, fakeMode));
            }

            gameTrackPlaylist.Name = _randomFluent.Hacker.Phrase();
            gameTrackPlaylist.DateTimeModified = DateTime.Now;
            GamePlayListValidator validator = new GamePlayListValidator();
            ValidationResult results = validator.Validate(gameTrackPlaylist);

            Assert.Contains(results.Errors, x => x.ErrorMessage == GamePlayListValidator.DateTimeCreatedErrorMessage);

        }
        
        [Fact]
        public void Assign_DefaultDateTimeModified_To_GamePlayList_ReturnsFalse()
        {
            GameTrackPlaylist gameTrackPlaylist = new GameTrackPlaylist();
            for (int count = 0; count < 10; count++)
            {
                Song fakeSong = new Song()
                {
                    Title = _randomFluent.Hacker.Phrase(),
                    Album = _randomFluent.Hacker.Phrase(),
                    Composer = _randomFluent.Person.FullName
                };

                int fakeGameId = _randomFluent.Random.Int(0, 100);
                
                DifficultyMode fakeMode = new DifficultyMode();

                gameTrackPlaylist.Add(new Ez2OnGameTrack(fakeSong, fakeGameId, fakeMode));
            }

            gameTrackPlaylist.Name = _randomFluent.Hacker.Phrase();
            gameTrackPlaylist.DateTimeCreated = DateTime.Now;
            GamePlayListValidator validator = new GamePlayListValidator();
            ValidationResult results = validator.Validate(gameTrackPlaylist);

            Assert.Contains(results.Errors, x => x.ErrorMessage == GamePlayListValidator.DateTimeModifiedErrorMessage);

        }
    }
}