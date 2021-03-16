using System;
using System.Collections.Generic;
using Bogus;
using CleanCode.Patterns.Validations;
using CleanCode.Patterns.XUnit;
using MusicGames.Domain.Specifications;
using Xunit;

namespace MusicGames.Domain.Test
{
    public class ValidatableUnitTest : IObserver<ValidationNotification>
    {
        private readonly List<ValidationNotification> _listOfValidationNotifications;

        public ValidatableUnitTest()
        {
            _listOfValidationNotifications = new List<ValidationNotification>();
        }
        
        public void OnCompleted()
        {
            // Nothing to do here
        }

        public void OnError(Exception error)
        {
            // Nothing to do here
        }

        public void OnNext(ValidationNotification value)
        {
            _listOfValidationNotifications.Add(value);
        }
        
        [Theory]
        [ClassData(typeof(NullAndWhitespaceTheoryData))]
        public void SameObserver_SubscribingTwice_Returns_OnlyOneMessage(string fakeSongTitle)
        {
            // arrange
            Song fakeSong = new Song()
            {
                Title = fakeSongTitle
            };
            
            //act
            SongTitleProvidedSpec mockSpec = new SongTitleProvidedSpec();
            mockSpec.Subscribe(this);
            mockSpec.Subscribe(this);
            var isNotificationTriggered = !mockSpec.IsSatisfiedBy(fakeSong);

            // assert
            Assert.True(isNotificationTriggered);
            Assert.Single(mockSpec.Observers);
        }

        [Fact]
        public void Unsubscribe_From_Validatable_Removes_Observer()
        {
            //arrange
            var lorem = new Bogus.DataSets.Lorem()
            {
                Random = new Randomizer(1080)
            };
            const int stubLengthToTriggerNotification = 257;
            var fakeTitle = lorem.Letter(stubLengthToTriggerNotification);

            Song fakeSong = new Song()
            {
                Title = fakeTitle
            };
            
            
            SongTitleWithin256CharactersSpec mockSpec = new SongTitleWithin256CharactersSpec();
            var unsubscriber = mockSpec.Subscribe(this);
            var isNotificationTriggered = !mockSpec.IsSatisfiedBy(fakeSong);
            Assert.True(isNotificationTriggered);
            Assert.Single(mockSpec.Observers);
            
            //act
            unsubscriber.Dispose();
            
            // assert
            Assert.Empty(mockSpec.Observers);
        }
    }
}