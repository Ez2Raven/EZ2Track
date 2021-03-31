using System;
using FluentValidation;
using MusicGames.Domain.Models;

namespace MusicGames.Domain.Validations
{
    public class GameTrackValidator:AbstractValidator<IGameTrack>
    {
        private readonly SongValidator _songValidator;
        private readonly GameValidator _gameValidator;

        public GameTrackValidator(SongValidator songValidator, GameValidator gameValidator)
        {
            _songValidator = songValidator ?? throw new ArgumentNullException(nameof(songValidator));
            _gameValidator = gameValidator ?? throw new ArgumentNullException(nameof(gameValidator));
            RuleFor(x => x.Game).NotNull();
            RuleFor(x => x.DifficultyMode.Category).NotEqual(DifficultyCategory.None);
            RuleFor(x => x.DifficultyMode.Level).NotEqual(0);
            RuleFor(x => x).Must(BeAValidSong).WithMessage("Please specify a valid song");
            RuleFor(x => x.Game).Must(BeAValidGame).WithMessage("Please specify a valid game");
        }

        private bool BeAValidSong(ISong song)
        {
            var validateResults = _songValidator.Validate(song);
            return validateResults.IsValid;
        }

        private bool BeAValidGame(IGame game)
        {
            var validateResults = _gameValidator.Validate(game);
            return validateResults.IsValid;
        }
    }
}