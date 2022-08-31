using FluentValidation;

namespace Gaming.Domain.AggregateModels.SongChartAggregate.Validators;

public class SongValidator : AbstractValidator<ISong>
{
    public SongValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Title).Length(1, 250);
        RuleFor(x => x.Composer).NotEmpty();
        RuleFor(x => x.Composer).Length(1, 250);
        RuleFor(x => x.Album).NotEmpty();
        RuleFor(x => x.Album).Length(1, 250);
    }
}
