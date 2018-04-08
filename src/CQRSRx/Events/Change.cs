using System;

namespace CQRSRx.Events
{
    /// <summary>
    /// Defines a change that was applied.
    /// </summary>
    public sealed class Change
    {
        /// <summary>
        /// The event that was applied by the change.
        /// </summary>
        public IEvent Event { get; }

        /// <summary>
        /// Constructs a new instance of a change.
        /// </summary>
        public Change(IEvent evt)
        {
            Event = evt;
        }

    }
}
