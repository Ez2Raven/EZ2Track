using Bogus;
using CleanCode.Patterns.XUnit;
using Gaming.Domain.AggregateModels.SongChartAggregate;
using Gaming.Domain.AggregateModels.SongChartAggregate.Validators;
using Xunit;
using Xunit.Abstractions;

namespace Gaming.Domain.Tests;

public class SongValidatorTest
{
    private readonly ITestOutputHelper _output;
    private readonly Faker _randomFluent;

    public SongValidatorTest(ITestOutputHelper output)
    {
        _output = output;
        _randomFluent = new Faker {Random = new Randomizer(1080)};
    }

    [Theory]
    [InlineData("en")]
    [InlineData("ko")]
    [InlineData("ja")]
    public void Assign_MoreThan256Characters_to_SongTitleAlbumComposer_ReturnsError(string locale)
    {
        _randomFluent.Lorem.Locale = locale;
        var fakeSong = new Song
        {
            Title = _randomFluent.Lorem.Letter(257),
            Album = _randomFluent.Lorem.Letter(257),
            Composer = _randomFluent.Lorem.Letter(257)
        };

        var validator = new SongValidator();
        var results = validator.Validate(fakeSong);

        var isValid = results.IsValid;
        var failures = results.Errors;

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
        var fakeSong = new Song {Title = whitespace, Album = whitespace, Composer = whitespace};

        var validator = new SongValidator();
        var results = validator.Validate(fakeSong);

        var isValid = results.IsValid;
        var failures = results.Errors;

        Assert.False(isValid);
        foreach (var failure in failures)
        {
            _output.WriteLine(failure.ErrorMessage);
        }
    }
}
