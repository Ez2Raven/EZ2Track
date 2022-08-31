using System;
using FluentValidation;
using Gaming.Domain.AggregateModels.SongChartAggregate.Ez2on;

namespace Gaming.Domain.AggregateModels.SongChartAggregate.Validators;

public class SongChartValidator : AbstractValidator<Ez2OnSongChart>
{
    private readonly GameValidator _gameValidator;
    private readonly SongValidator _songValidator;

    public SongChartValidator(SongValidator songValidator, GameValidator gameValidator)
    {
        _songValidator = songValidator ?? throw new ArgumentNullException(nameof(songValidator));
        _gameValidator = gameValidator ?? throw new ArgumentNullException(nameof(gameValidator));
        RuleFor(x => x.Song).Must(BeAValidSong).WithMessage("Please specify a valid song");
        RuleFor(x => x.Game).Must(BeAValidGame).WithMessage("Please specify a valid game");
        RuleFor(x => x.DifficultyMode.Category).NotEqual(DifficultyCategory.None);
        RuleFor(x => x.DifficultyMode.Level).NotEqual(0);
    }

    private bool BeAValidSong(ISong song)
    {
        var validateResults = _songValidator.Validate(song);
        return validateResults.IsValid;
    }

    private bool BeAValidGame(ReleaseTitle game)
    {
        var validateResults = _gameValidator.Validate(game);
        return validateResults.IsValid;
    }
}
