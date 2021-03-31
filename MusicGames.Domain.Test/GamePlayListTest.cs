using System;
using Bogus;
using MusicGames.Domain.Models;
using MusicGames.Domain.Validations;
using Xunit;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace MusicGames.Domain.Test
{
    public class GamePlayListTest
    {
        [Fact]
        public void Validate_GamePlayList_ReturnsTrue()
        {
            var fakePerson = new Bogus.Person()
            {
                Random = new Randomizer(1080)
            };

            var randomPhrase = new Bogus.DataSets.Hacker()
            {
                Random = new Randomizer(1080)
            };

            var randomFluent = new Bogus.Faker()
            {
                Random = new Randomizer(1080)
            };
            

            GameTrackPlaylist gameTrackPlaylist = new GameTrackPlaylist();
            for (int count = 0; count < 10; count++)
            {
                Song fakeSong = new Song()
                {
                    Title = randomPhrase.Phrase(),
                    Album = randomPhrase.Phrase(),
                    Composer = fakePerson.FullName
                };

                Game  fakeGame = new Game()
                {
                    Title = randomPhrase.Phrase()
                };

                DifficultyMode fakeMode = new DifficultyMode();

                gameTrackPlaylist.Add(new Ez2OnGameTrack(fakeSong, fakeGame, fakeMode));
            }

            gameTrackPlaylist.Name = randomPhrase.Phrase();
            gameTrackPlaylist.DateTimeCreated = randomFluent.Date.Past(10, DateTime.Now);
            gameTrackPlaylist.DateTimeModified = randomFluent.Date.Future(10, DateTime.Now);

            GamePlayListValidator validator = new GamePlayListValidator();
            ValidationResult results = validator.Validate(gameTrackPlaylist);
            
            Assert.True(results.IsValid);
        }
        
        [Fact]
        public void Assign_DefaultDateTimeCreated_To_GamePlayList_ReturnsFalse()
        {
            var fakePerson = new Bogus.Person()
            {
                Random = new Randomizer(1080)
            };

            var randomPhrase = new Bogus.DataSets.Hacker()
            {
                Random = new Randomizer(1080)
            };

            GameTrackPlaylist gameTrackPlaylist = new GameTrackPlaylist();
            for (int count = 0; count < 10; count++)
            {
                Song fakeSong = new Song()
                {
                    Title = randomPhrase.Phrase(),
                    Album = randomPhrase.Phrase(),
                    Composer = fakePerson.FullName
                };

                Game  fakeGame = new Game()
                {
                    Title = randomPhrase.Phrase()
                };
                
                DifficultyMode fakeMode = new DifficultyMode();

                gameTrackPlaylist.Add(new Ez2OnGameTrack(fakeSong, fakeGame, fakeMode));
            }

            gameTrackPlaylist.Name = randomPhrase.Phrase();
            gameTrackPlaylist.DateTimeModified = DateTime.Now;
            GamePlayListValidator validator = new GamePlayListValidator();
            ValidationResult results = validator.Validate(gameTrackPlaylist);

            Assert.Contains(results.Errors, x => x.ErrorMessage == GamePlayListValidator.DateTimeCreatedErrorMessage);

        }
        
        [Fact]
        public void Assign_DefaultDateTimeModified_To_GamePlayList_ReturnsFalse()
        {
            var fakePerson = new Bogus.Person()
            {
                Random = new Randomizer(1080)
            };

            var randomPhrase = new Bogus.DataSets.Hacker()
            {
                Random = new Randomizer(1080)
            };

            var randomFluent = new Bogus.Faker()
            {
                Random = new Randomizer(1080)
            };
            

            GameTrackPlaylist gameTrackPlaylist = new GameTrackPlaylist();
            for (int count = 0; count < 10; count++)
            {
                Song fakeSong = new Song()
                {
                    Title = randomPhrase.Phrase(),
                    Album = randomPhrase.Phrase(),
                    Composer = fakePerson.FullName
                };

                Game  fakeGame = new Game()
                {
                    Title = randomPhrase.Phrase()
                };
                
                DifficultyMode fakeMode = new DifficultyMode();

                gameTrackPlaylist.Add(new Ez2OnGameTrack(fakeSong, fakeGame, fakeMode));
            }

            gameTrackPlaylist.Name = randomPhrase.Phrase();
            gameTrackPlaylist.DateTimeCreated = DateTime.Now;
            GamePlayListValidator validator = new GamePlayListValidator();
            ValidationResult results = validator.Validate(gameTrackPlaylist);

            Assert.Contains(results.Errors, x => x.ErrorMessage == GamePlayListValidator.DateTimeModifiedErrorMessage);

        }
    }
}