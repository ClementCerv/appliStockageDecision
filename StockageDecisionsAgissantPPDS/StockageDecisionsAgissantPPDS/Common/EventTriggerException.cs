using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace StockageDecisionsAgissantPPDS.Common
{
    public class EventTriggerException : Exception
    {
        public static event EventHandler<Exception> NewInstance;

        public EventTriggerException()
        {
            NewInstance?.Invoke(null, this);
        }

        public EventTriggerException(string message) : base(message)
        {
            NewInstance?.Invoke(null, this);
        }

        public EventTriggerException(string message, Exception innerException) : base(message, innerException)
        {
            NewInstance?.Invoke(null, this);
        }

        protected EventTriggerException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
            NewInstance?.Invoke(null, this);
        }
    }
}
