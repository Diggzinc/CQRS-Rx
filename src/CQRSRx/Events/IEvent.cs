using System;

namespace CQRSRx.Events
{
    /// <summary>
    /// Defines an Event.
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        /// The version of the event.
        /// </summary>
        long Version { get; set; }
    }
}
