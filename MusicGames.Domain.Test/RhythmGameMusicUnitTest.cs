using System;
using System.Linq;
using Bogus;
using Microsoft.Extensions.Logging;
using Moq;
using MusicGames.Domain.Validations;
using Xunit;

namespace MusicGames.Domain.Test
{
    public class RhythmGameMusicUnitTest
    {
        [Theory]
        [InlineData("en")]
        [InlineData("ko")]
        [InlineData("ja")]
        public void Assign_GreaterThan256_Title_Sends_Notification(string locale)
        {
            // Arrange
            var faker = new Faker("en")
            {
                Random = new Randomizer(1080)
            };

            var lorem = new Bogus.DataSets.Lorem(locale: locale)
            {
                Random = new Randomizer(1080)
            };

            const int stubLength = 257;
            var fakeSongTitle = lorem.Letter(stubLength);
            
            Song fakeSong = new Song()
            {
                Title = fakeSongTitle
            };

            var stubLoggerBehavior = new Mock<ILogger<SongValidator>>();
            ILogger<SongValidator> stubLogger = stubLoggerBehavior.Object;
            
            // Act
            SongValidator songValidator = new SongValidator(stubLogger, fakeSong);
            
            var actualErrorMessages = songValidator.Validate();

            // Assert
            Assert.Single(actualErrorMessages);
            const string expectedErrorMessage = "Song title must be within 256 characters";
            Assert.Equal(expectedErrorMessage,actualErrorMessages.First().Message);
        }
    }
}