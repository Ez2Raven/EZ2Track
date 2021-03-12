using System;
using System.Collections.Generic;
using CleanCode.Patterns.Specifications;
using CleanCode.Patterns.Validations;
using Microsoft.Extensions.Logging;
using MusicGames.Domain.Specifications;

namespace MusicGames.Domain.Validations
{
    public sealed class SongValidator:IObserver<ValidationNotification>
    {
        private readonly ILogger<SongValidator> _logger;
        private readonly Song _song;
        private readonly List<ValidationNotification> _errors;
        
        private readonly IsSongTitleProvided _isSongTitleProvided;
        private readonly IsSongTitleWithin256Characters _isSongTitleWithin256Characters;

        public SongValidator(ILogger<SongValidator> logger, Song song)
        {
            _logger = logger;
            _song = song ?? throw new ArgumentNullException(nameof(song));
            _errors = new List<ValidationNotification>();
            
            _isSongTitleProvided = new IsSongTitleProvided();
            _isSongTitleWithin256Characters = new IsSongTitleWithin256Characters();
            
        }

        public List<ValidationNotification> Validate()
        {
            _isSongTitleProvided.Subscribe(this);
            _isSongTitleWithin256Characters.Subscribe(this);
            var titleSpecification = _isSongTitleProvided.And(_isSongTitleWithin256Characters);
            titleSpecification.IsSatisfiedBy(_song);

            return _errors;
        }

        public void OnCompleted()
        {
            _logger.LogDebug("Validation Completed, No additional validation message will be recorded");
        }

        public void OnError(Exception error)
        {
            // Note that the OnError method should not handle the passed Exception object as an exception.
            // Nothing to do when publisher has an exception.
        }

        public void OnNext(ValidationNotification validationNotification)
        {
            if (validationNotification?.Message != null)
                _errors.Add(validationNotification);
        }
    }
}