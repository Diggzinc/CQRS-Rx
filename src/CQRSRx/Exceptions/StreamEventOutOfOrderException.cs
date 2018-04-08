using System;

namespace CQRSRx.Exceptions
{
    /// <summary>
    /// Exception raised when the observed stream publishes an event that is out of the expected order.
    /// </summary>
    public class StreamEventOutOfOrderException : Exception
    {
        /// <summary>
        /// Constructs an instance of the exception.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public StreamEventOutOfOrderException(string message) : base(message) { }
    }
}
