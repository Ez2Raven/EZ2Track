using FluentValidation;
using MusicGames.Domain.Models;

namespace MusicGames.Domain.Validations
{
    public class GameValidator:AbstractValidator<Game>
    {
        public GameValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Title).Length(1, 250);
        }
    }
}