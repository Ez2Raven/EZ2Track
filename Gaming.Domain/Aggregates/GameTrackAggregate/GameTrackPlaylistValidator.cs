using System;
using FluentValidation;

namespace Gaming.Domain.Aggregates.GameTrackAggregate;

public class GameTrackPlaylistValidator : AbstractValidator<Playlist<GameTrack>>
{
    public const string DateTimeCreatedErrorMessage = "Please provide a non-default DateTimeCreated";
    public const string DateTimeModifiedErrorMessage = "Please provide a non-default DateTimeModified";

    public GameTrackPlaylistValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Name).Length(1, 250);
        RuleFor(x => x.DateTimeCreated).NotEqual(default(DateTime))
            .WithMessage(DateTimeCreatedErrorMessage);
        RuleFor(x => x.DateTimeModified).NotEqual(default(DateTime))
            .WithMessage(DateTimeModifiedErrorMessage);
        RuleFor(x => x.Songs).NotNull();
    }
}
