using System;

namespace CQRSRx.Exceptions
{
    /// <summary>
    /// Exception raised when the observed stream by an aggregate throws an error.
    /// </summary>
    public class CorruptedStreamException : Exception
    {
        /// <summary>
        /// Constructs an instance of the exception.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="innerException">The inner exception to nest.</param>
        public CorruptedStreamException(string message, Exception innerException) : base(message, innerException) { }
    }
}
