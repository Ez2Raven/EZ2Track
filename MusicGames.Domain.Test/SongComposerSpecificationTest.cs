using System;
using System.Linq;
using Bogus;
using CleanCode.Patterns.Specifications;
using CleanCode.Patterns.Validations;
using CleanCode.Patterns.XUnit;
using Microsoft.Extensions.Logging;
using Moq;
using MusicGames.Domain.Specifications;
using MusicGames.Domain.Validations;
using Xunit;
using Xunit.Abstractions;

namespace MusicGames.Domain.Test
{
    public class SongComposerSpecificationTest
    {
        private readonly ITestOutputHelper _output;

        public SongComposerSpecificationTest(ITestOutputHelper output)
        {
            _output = output;
        }
        
        [Theory]
        [InlineData("en")]
        [InlineData("ko")]
        [InlineData("ja")]
        public void Assign_GreaterThan256_Composer_ReturnsFalse(string locale)
        {
            // Arrange
            var lorem = new Bogus.DataSets.Lorem(locale: locale)
            {
                Random = new Randomizer(1080)
            };

            const int stubLengthToTriggerNotification = 257;
            var fakeComposerTitle = lorem.Letter(stubLengthToTriggerNotification);

            Song fakeSong = new Song()
            {
                Composer = fakeComposerTitle
            };
            
            // Act
            SongComposerWithin256CharactersSpec mockSpec = new SongComposerWithin256CharactersSpec();
            var isSatisfied = mockSpec.IsSatisfiedBy(fakeSong);
            
            // Assert
            _output.WriteLine($"Asserting: {fakeSong.Composer}");
            Assert.False(isSatisfied);
        }

        [Theory]
        [ClassData(typeof(NullAndWhitespaceTheoryData))]
        public void Assign_Whitespace_Composer_Returns_False(string songTitle)
        {
            // Arrange
            var fakeComposer = songTitle;

            Song fakeSong = new Song()
            {
                Composer = fakeComposer
            };

            // Act
            SongComposerProvidedSpec mockSpec = new SongComposerProvidedSpec();
            var isSatisfied = mockSpec.IsSatisfiedBy(fakeSong);

            // Assert
            _output.WriteLine($"Asserting: {fakeSong.Composer}");
            Assert.False(isSatisfied);
        }
    }

    public class SongComposerProvidedSpec:Validatable, ISpecification<Song>
    {
        public const string ValidationMessage = "Song composer must not be empty and does not contains only whitespace";
        public bool IsSatisfiedBy(Song entity)
        {
            var isSatisfied = !string.IsNullOrWhiteSpace(entity.Title);
            if (!isSatisfied)
            {
                BroadcastValidationMessage(ValidationMessage);
            }
            
            return isSatisfied;
        }
    }
}