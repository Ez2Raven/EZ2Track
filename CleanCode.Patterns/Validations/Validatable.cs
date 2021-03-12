using System;
using System.Collections.Generic;

namespace CleanCode.Patterns.Validations
{
    public abstract class Validatable:IObservable<ValidationNotification>
    {
        private List<IObserver<ValidationNotification>> Observers { get; }

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

    }
}