using System;
using System.Collections.Generic;
using CQRSRx.Events;

namespace CQRSRx.Domain
{
    /// <summary>
    /// Defines an aggregate root to be used for the domain objects.
    /// </summary>
    public interface IAggregateRoot : IObserver<IEvent>, IObservable<Change>, IEnumerable<UncommittedChange>
    {
        /// <summary>
        /// The aggregate unique identifier.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// The monotonically increasing version.
        /// </summary>
        long Version { get; }

        /// <summary>
        /// Pushes the uncommitted changes to the observers, and marks them as committed.
        /// </summary>
        void PublishChanges();
    }
}
