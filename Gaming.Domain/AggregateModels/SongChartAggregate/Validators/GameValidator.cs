using FluentValidation;

namespace Gaming.Domain.AggregateModels.SongChartAggregate.Validators;

public class GameValidator : AbstractValidator<ReleaseTitle>
{
    public GameValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Name).Length(1, 250);
    }
}
