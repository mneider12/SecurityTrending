using System;
using System.Collections.Generic;
using System.Text;

namespace DataEntry
{
    /// <summary>
    /// exception occurred reading from a file
    /// </summary>
    public class FileLoaderException: Exception
    {
        /// <summary>
        /// default exception constructor
        /// </summary>
        public FileLoaderException() : base() { }
        /// <summary>
        /// pass a message with the exception
        /// </summary>
        /// <param name="message">error message</param>
        public FileLoaderException(string message) : base(message) { }
        /// <summary>
        /// exception caused by another exception
        /// </summary>
        /// <param name="message">error message</param>
        /// <param name="innerException">inner exception</param>
        public FileLoaderException(string message, Exception innerException) : base(message, innerException) { }
    }
}
