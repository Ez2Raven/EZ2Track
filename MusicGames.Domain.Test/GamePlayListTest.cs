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
            

            GamePlayList gamePlayList = new GamePlayList();
            for (int count = 0; count < 10; count++)
            {
                gamePlayList.Add(new GameTrack()
                {
                    Song = new Song()
                    {
                        Title = randomPhrase.Phrase(),
                        Album = randomPhrase.Phrase(),
                        Composer = fakePerson.FullName
                    },
                    DifficultyTier = DifficultyTier.None,
                    Game = new BaseGame()
                    {
                        Title = randomPhrase.Phrase()
                    }
                });
            }

            gamePlayList.Name = randomPhrase.Phrase();
            gamePlayList.DateTimeCreated = randomFluent.Date.Past(10, DateTime.Now);
            gamePlayList.DateTimeModified = randomFluent.Date.Future(10, DateTime.Now);

            GamePlayListValidator validator = new GamePlayListValidator();
            ValidationResult results = validator.Validate(gamePlayList);
            
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

            var randomFluent = new Bogus.Faker()
            {
                Random = new Randomizer(1080)
            };
            

            GamePlayList gamePlayList = new GamePlayList();
            for (int count = 0; count < 10; count++)
            {
                gamePlayList.Add(new GameTrack()
                {
                    Song = new Song()
                    {
                        Title = randomPhrase.Phrase(),
                        Album = randomPhrase.Phrase(),
                        Composer = fakePerson.FullName
                    },
                    DifficultyTier = DifficultyTier.None,
                    Game = new BaseGame()
                    {
                        Title = randomPhrase.Phrase()
                    }
                });
            }

            gamePlayList.Name = randomPhrase.Phrase();
            gamePlayList.DateTimeModified = DateTime.Now;
            GamePlayListValidator validator = new GamePlayListValidator();
            ValidationResult results = validator.Validate(gamePlayList);

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
            

            GamePlayList gamePlayList = new GamePlayList();
            for (int count = 0; count < 10; count++)
            {
                gamePlayList.Add(new GameTrack()
                {
                    Song = new Song()
                    {
                        Title = randomPhrase.Phrase(),
                        Album = randomPhrase.Phrase(),
                        Composer = fakePerson.FullName
                    },
                    DifficultyTier = DifficultyTier.None,
                    Game = new BaseGame()
                    {
                        Title = randomPhrase.Phrase()
                    }
                });
            }

            gamePlayList.Name = randomPhrase.Phrase();
            gamePlayList.DateTimeCreated = DateTime.Now;
            GamePlayListValidator validator = new GamePlayListValidator();
            ValidationResult results = validator.Validate(gamePlayList);

            Assert.Contains(results.Errors, x => x.ErrorMessage == GamePlayListValidator.DateTimeModifiedErrorMessage);

        }
    }
}