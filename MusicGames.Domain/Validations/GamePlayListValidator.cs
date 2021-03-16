using System;
using FluentValidation;
using MusicGames.Domain.Models;

namespace MusicGames.Domain.Validations
{
    public class GamePlayListValidator:AbstractValidator<GamePlayList>
    {
        public const string DateTimeCreatedErrorMessage = "Please provide a non-default DateTimeCreated";
        public const string DateTimeModifiedErrorMessage = "Please provide a non-default DateTimeModified";
        public GamePlayListValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Name).Length(1, 250);
            RuleFor(x => x.DateTimeCreated).NotEqual(default(DateTime))
                .WithMessage(DateTimeCreatedErrorMessage);
            RuleFor(x => x.DateTimeModified).NotEqual(default(DateTime))
                .WithMessage(DateTimeModifiedErrorMessage);
        }
    }
}