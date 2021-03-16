using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CleanCode.Patterns.Notifications;

// Enable InternalsVisibleTo Testing Libraries, but limit its use in DEBUG builds only.
#if(DEBUG)
[assembly: InternalsVisibleTo("MusicGames.Domain.Test")]
#endif
namespace CleanCode.Patterns.Validations
{
    public abstract class Validatable:IObservable<ValidationNotification>
    {
        internal List<IObserver<ValidationNotification>> Observers { get; }

        protected Validatable()
        {
            Observers = new List<IObserver<ValidationNotification>>();
        }

        public IDisposable Subscribe(IObserver<ValidationNotification> observer)
        {
            if (!Observers.Contains(observer))
            {
                Observers.Add(observer);
            }
            return new Unsubscriber<ValidationNotification>(Observers, observer);
        }

        protected void BroadcastValidationMessage(string message)
        {
            foreach (var observer in Observers)
            {
                ValidationNotification notification =
                    new ValidationNotification(message);
                observer.OnNext(notification);
            }
        }

        protected void BroadcastValidationError(Exception ex)
        {
            foreach (var observer in Observers)
            {
                observer.OnError(ex);
            }
        }

        protected void BroadcastValidationCompleted()
        {
            foreach (var observer in Observers)
            {
                observer.OnCompleted();
            }
        }

    }
}