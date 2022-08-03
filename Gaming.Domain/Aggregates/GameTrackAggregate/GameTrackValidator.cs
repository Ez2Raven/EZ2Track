using System;
using FluentValidation;
using Gaming.Domain.Aggregates.GameAggregate;
using Gaming.Domain.Aggregates.MusicAggregate;

namespace Gaming.Domain.Aggregates.GameTrackAggregate;

public class GameTrackValidator : AbstractValidator<GameTrack>
{
    private readonly GameValidator _gameValidator;
    private readonly SongValidator _songValidator;

    public GameTrackValidator(SongValidator songValidator, GameValidator gameValidator)
    {
        _songValidator = songValidator ?? throw new ArgumentNullException(nameof(songValidator));
        _gameValidator = gameValidator ?? throw new ArgumentNullException(nameof(gameValidator));
        RuleFor(x => x.Song).Must(BeAValidSong).WithMessage("Please specify a valid song");
        RuleFor(x => x.Game).Must(BeAValidGame).WithMessage("Please specify a valid game");
        RuleFor(x => x.DifficultyMode.Category).NotEqual(DifficultyCategory.None);
        RuleFor(x => x.DifficultyMode.Level).NotEqual(0);
    }

    private bool BeAValidSong(Song song)
    {
        var validateResults = _songValidator.Validate(song);
        return validateResults.IsValid;
    }

    private bool BeAValidGame(Game game)
    {
        var validateResults = _gameValidator.Validate(game);
        return validateResults.IsValid;
    }
}
