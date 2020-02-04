using System;

namespace Reports
{
    /// <summary>
    /// Exception when no quote can be returned by the quote feed
    /// </summary>
    public class NoQuoteAvailableException: Exception
    {
        public NoQuoteAvailableException() : base() { }

        public NoQuoteAvailableException(string message) : base(message) { }

        public NoQuoteAvailableException(string message, Exception innerException) : base(message, innerException) { }

        protected NoQuoteAvailableException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
