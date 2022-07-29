using FluentValidation;

namespace MusicGames.Domain.AggregatesModels.GameAggregate
{
    public class GameValidator : AbstractValidator<Game>
    {
        public GameValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Title).Length(1, 250);
        }
    }
}