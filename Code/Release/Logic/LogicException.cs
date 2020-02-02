using System;

namespace Logic
{
    /// <summary>
    /// attempted operation violates a business logic rule
    /// </summary>
    public class LogicException : Exception
    {
        /// <summary>
        /// default exception constructor
        /// </summary>
        public LogicException() : base() { }
        /// <summary>
        /// pass a message with the exception
        /// </summary>
        /// <param name="message">error message</param>
        public LogicException(string message) : base(message) { }
        /// <summary>
        /// exception caused by another exception
        /// </summary>
        /// <param name="message">error message</param>
        /// <param name="innerException">inner exception</param>
        public LogicException(string message, Exception innerException) : base(message, innerException) { }
    }
}
