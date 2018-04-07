using System;

namespace CQRSRx.Integration.SimpleInjector.Exceptions
{
    /// <summary>
    /// Exception raised when handlers cannot be resolved by the processor.
    /// </summary>
    public class UnresolvedHandlerException : Exception
    {
        /// <summary>
        /// Constructs an instance of the exception.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <param name="innerException">The inner exception to nest.</param>
        public UnresolvedHandlerException(string message, Exception innerException) : base(message, innerException) { }
    }
}
