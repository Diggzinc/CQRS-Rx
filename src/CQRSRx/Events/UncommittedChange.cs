namespace CQRSRx.Events
{

    /// <summary>
    /// The change yet to commit.
    /// </summary>
    public sealed class UncommittedChange
    {
        /// <summary>
        /// The event that is uncommitted by the change.
        /// </summary>
        public IEvent Event { get; }

        /// <summary>
        /// /// Constructs a new instance of an uncommitted change.
        /// </summary>
        public UncommittedChange(IEvent evt)
        {
            Event = evt;
        }
    }
}
