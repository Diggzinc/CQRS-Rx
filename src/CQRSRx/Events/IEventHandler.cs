namespace CQRSRx.Events
{
    /// <summary>
    /// Handles the event (used in conjugation with aggregate roots).
    /// </summary>
    public interface IEventHandler<TEvent> where TEvent : IEvent
    {
        /// <summary>
        /// Applies the event received on the aggregate root.
        /// </summary>
        /// <param name="evt">The event to apply.</param>
        void Apply(TEvent evt);
    }
}
