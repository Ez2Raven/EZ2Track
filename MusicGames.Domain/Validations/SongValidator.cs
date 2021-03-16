using System;
using System.Collections.Generic;
using System.ComponentModel;
using CleanCode.Patterns.Specifications;
using CleanCode.Patterns.Validations;
using Microsoft.Extensions.Logging;
using MusicGames.Domain.Specifications;

namespace MusicGames.Domain.Validations
{
    public sealed class SongValidator : IObserver<ValidationNotification>
    {
        private readonly ILogger<SongValidator> _logger;
        private readonly Song _song;
        private readonly List<ValidationNotification> _errors;

        private SongTitleProvidedSpec SongTitleProvidedSpecSpecification { get; }
        private SongTitleWithin256CharactersSpec SongTitleWithin256CharactersSpec { get; }

        public SongValidator(ILogger<SongValidator> logger, Song song)
        {
            _logger = logger;
            _song = song ?? throw new ArgumentNullException(nameof(song));
            _errors = new List<ValidationNotification>();

            SongTitleProvidedSpecSpecification = new SongTitleProvidedSpec();
            SongTitleWithin256CharactersSpec = new SongTitleWithin256CharactersSpec();
        }

        public List<ValidationNotification> Validate()
        {
            SongTitleProvidedSpecSpecification.Subscribe(this);
            SongTitleWithin256CharactersSpec.Subscribe(this);
            var titleSpecification = SongTitleProvidedSpecSpecification.And(SongTitleWithin256CharactersSpec);
            titleSpecification.IsSatisfiedBy(_song);

            return _errors;
        }

        public void OnCompleted()
        {
            _logger.LogWarning("One of the validatable specification has completed the validation, " +
                               "but {Classname} will only unsubscribe upon disposed", TypeDescriptor.GetClassName(this));
        }

        /// <summary>
        /// The OnError method should be seen as informational, and the provider should not expect the observer to provide error handling
        /// </summary>
        /// <param name="error"></param>
        public void OnError(Exception error)
        {
            // Note that the OnError method should not handle the passed Exception object as an exception.
            // Nothing to do when publisher has an exception.
            _logger.LogWarning(error,"An exception has occured in one of the validatable specification");
        }

        public void OnNext(ValidationNotification validationNotification)
        {
            if (validationNotification?.Message != null)
                _errors.Add(validationNotification);
        }
    }
}