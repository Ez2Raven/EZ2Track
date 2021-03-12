using System;

namespace CleanCode.Patterns.Validations
{
    public class ValidationNotification
    {
        public string Message { get; }

        public ValidationNotification(string message)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }
    }
}