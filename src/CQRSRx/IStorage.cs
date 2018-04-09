using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CQRSRx.Events;

namespace CQRSRx
{
    /// <summary>
    /// Generic Event storage.
    /// </summary>
    public interface IStorage : IObservable<IEvent>
    {
        /// <summary>
        /// Loads the given stream of events from the storage.
        /// </summary>
        /// <param name="id">The events stream identifier.</param>
        /// <param name="cancellationToken">The custom cancellation token.</param>
        /// <returns>The correspondent task that will yield the events stored for the given stream.</returns>
        Task<IEnumerable<IEvent>> LoadEventsAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Stores the changes as events on the storage.
        /// </summary>
        /// <param name="changes">The changes to be stored.</param>
        /// <param name="cancellationToken">The custom cancellation token.</param>
        /// <returns>The correspondent task for the save operation.</returns>
        Task SaveChangesAsync(IEnumerable<UncommittedChange> changes, CancellationToken cancellationToken);
    }
}
