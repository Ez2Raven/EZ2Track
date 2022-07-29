using System;
using FluentValidation;
using MusicGames.Domain.AggregatesModels.MusicAggregate;

namespace MusicGames.Domain.AggregatesModels.GameTrackAggregate
{
    public class GameTrackValidator : AbstractValidator<GameTrack>
    {
        private readonly SongValidator _songValidator;

        public GameTrackValidator(SongValidator songValidator)
        {
            _songValidator = songValidator ?? throw new ArgumentNullException(nameof(songValidator));
            RuleFor(x => x.GameId).NotEqual(0);
            RuleFor(x => x.DifficultyMode.Category).NotEqual(DifficultyCategory.None);
            RuleFor(x => x.DifficultyMode.Level).NotEqual(0);
            RuleFor(x => x.Song).Must(BeAValidSong).WithMessage("Please specify a valid song");
        }

        private bool BeAValidSong(Song song)
        {
            var validateResults = _songValidator.Validate(song);
            return validateResults.IsValid;
        }
    }
}