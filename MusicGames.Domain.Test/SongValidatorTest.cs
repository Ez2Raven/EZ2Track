using System.Collections.Generic;
using Bogus;
using CleanCode.Patterns.XUnit;
using FluentValidation.Results;
using MusicGames.Domain.AggregatesModels;
using MusicGames.Domain.AggregatesModels.GameTrackAggregate;
using MusicGames.Domain.Validations;
using Xunit;
using Xunit.Abstractions;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace MusicGames.Domain.Test
{
    public class SongValidatorTest
    {
        private readonly ITestOutputHelper _output;
        private readonly Faker _randomFluent;
        public SongValidatorTest(ITestOutputHelper output)
        {
            _output = output;
            _randomFluent = new Bogus.Faker()
            {
                Random = new Randomizer(1080)
            };
        }
        
        [Theory]
        [InlineData("en")]
        [InlineData("ko")]
        [InlineData("ja")]
        public void Assign_MoreThan256Characters_to_SongTitleAlbumComposer_ReturnsError(string locale)
        {
            Song fakeSong = new Song()
            {
                Title = _randomFluent.Lorem.Letter(257),
                Album = _randomFluent.Lorem.Letter(257),
                Composer =_randomFluent.Lorem.Letter(257)
            };
            
            SongValidator validator = new SongValidator();
            ValidationResult results = validator.Validate(fakeSong);

            bool isValid = results.IsValid;
            IList<ValidationFailure> failures = results.Errors;
            
            Assert.False(isValid);
            foreach (var failure in failures)
            {
                _output.WriteLine(failure.ErrorMessage);
            }
        }

        [Theory]
        [ClassData(typeof(NullAndWhitespaceTheoryData))]
        public void Assign_NullOrWhitespace_to_SongTitleAlbumComposer_ReturnsError(string whitespace)
        {
            Song fakeSong = new Song()
            {
                Title = whitespace,
                Album = whitespace,
                Composer =whitespace
            };
            
            SongValidator validator = new SongValidator();
            ValidationResult results = validator.Validate(fakeSong);

            bool isValid = results.IsValid;
            IList<ValidationFailure> failures = results.Errors;
            
            Assert.False(isValid);
            foreach (var failure in failures)
            {
                _output.WriteLine(failure.ErrorMessage);
            }
        }
    }
}