using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Bogus;
using CleanCode.Patterns.XUnit;
using Microsoft.Extensions.Logging;
using Moq;
using MusicGames.Domain.Specifications;
using MusicGames.Domain.Validations;
using Xunit;
using Xunit.Abstractions;

namespace MusicGames.Domain.Test
{
    public class SongValidatorUnitTest
    {
        private readonly ITestOutputHelper _output;

        public SongValidatorUnitTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData("en")]
        [InlineData("ko")]
        [InlineData("ja")]
        public void Assign_GreaterThan256_Title_Sends_Notification(string locale)
        {
            // Arrange
            var lorem = new Bogus.DataSets.Lorem(locale: locale)
            {
                Random = new Randomizer(1080)
            };

            const int stubLengthToTriggerNotification = 257;
            var fakeSongTitle = lorem.Letter(stubLengthToTriggerNotification);

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
            _output.WriteLine($"Asserting song title: {Environment.NewLine}{fakeSong.Title}");
            Assert.Single(actualErrorMessages);
            const string expectedErrorMessage = SongTitleWithin256CharactersSpec.ValidationMessage;
            Assert.Equal(expectedErrorMessage, actualErrorMessages.First().Message);
        }

        [Theory]
        [ClassData(typeof(NullAndWhitespaceTheoryData))]
        public void Assign_Whitespace_Title_Sends_Notification(string songTitle)
        {
            // Arrange
            var fakeSongTitle = songTitle;

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
            _output.WriteLine($"Asserting song title: {Environment.NewLine}{fakeSong.Title}");
            const string expectedErrorMessage = SongTitleProvidedSpec.ValidationMessage;
            Assert.Equal(expectedErrorMessage, actualErrorMessages.First().Message);
        }

        [Theory]
        [InlineData("en")]
        [InlineData("ko")]
        [InlineData("ja")]
        public void Assign_GreaterThan256_Composer_Sends_Notification(string locale)
        {
            // Arrange
            var lorem = new Bogus.DataSets.Lorem(locale: locale)
            {
                Random = new Randomizer(1080)
            };

            const int stubLengthToTriggerNotification = 257;
            var fakeSongComposer = lorem.Letter(stubLengthToTriggerNotification);

            Song fakeSong = new Song()
            {
                Composer = fakeSongComposer
            };

            var stubLoggerBehavior = new Mock<ILogger<SongValidator>>();
            ILogger<SongValidator> stubLogger = stubLoggerBehavior.Object;

            // Act
            SongValidator songValidator = new SongValidator(stubLogger, fakeSong);

            var actualErrorMessages = songValidator.Validate();

            // Assert
            _output.WriteLine($"Asserting song composer: {Environment.NewLine}{fakeSong.Composer}");
            const string expectedErrorMessage = SongComposerWithin256CharactersSpec.ValidationMessage;
            Assert.Equal(expectedErrorMessage, actualErrorMessages.First().Message);
        }
    }
}