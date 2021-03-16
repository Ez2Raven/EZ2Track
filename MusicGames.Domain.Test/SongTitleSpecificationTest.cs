using System;
using System.Linq;
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
    public class SongTitleSpecificationTest
    {
        private readonly ITestOutputHelper _output;

        public SongTitleSpecificationTest(ITestOutputHelper output)
        {
            _output = output;
        }
        
        [Theory]
        [InlineData("en")]
        [InlineData("ko")]
        [InlineData("ja")]
        public void Assign_GreaterThan256_Title_ReturnsFalse(string locale)
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
            
            // Act
            SongTitleWithin256CharactersSpec mockSpec = new SongTitleWithin256CharactersSpec();
            var isSatisfied = mockSpec.IsSatisfiedBy(fakeSong);
            
            // Assert
            _output.WriteLine($"Asserting: {fakeSong.Title}");
            Assert.False(isSatisfied);
        }

        [Theory]
        [ClassData(typeof(NullAndWhitespaceTheoryData))]
        public void Assign_Whitespace_Title_Returns_False(string songTitle)
        {
            // Arrange
            var fakeSongTitle = songTitle;

            Song fakeSong = new Song()
            {
                Title = fakeSongTitle
            };

            // Act
            SongTitleProvidedSpec mockSpec = new SongTitleProvidedSpec();
            var isSatisfied = mockSpec.IsSatisfiedBy(fakeSong);

            // Assert
            _output.WriteLine($"Asserting: {fakeSong.Title}");
            Assert.False(isSatisfied);
        }
    }
}