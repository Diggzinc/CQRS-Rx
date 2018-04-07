using System;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSRx.Domain
{
    /// <summary>
    /// Asynchronous aggregate root repository.
    /// </summary>
    public interface IAggregateRootRepository<TAggregateRoot> where TAggregateRoot : IAggregateRoot
    {
        /// <summary>
        /// Retrieves the aggregate root asynchronously.
        /// </summary>
        /// <param name="id">The Id of the aggregate root object.</param>
        /// <param name="cancellationToken">The custom cancellation token.</param>
        /// <returns>The correspondent task that will yield the aggregate root object.</returns>
        Task<TAggregateRoot> LoadAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Atomically commits the pending changes of the aggregate root asynchronously.
        /// </summary>
        /// <param name="aggregate">The aggregate to commit.</param>
        /// <param name="cancellationToken">The custom cancellation token.</param>
        /// <returns>The correspondent task for the save operation.</returns>
        Task SaveAsync(TAggregateRoot aggregate, CancellationToken cancellationToken);
    }
}
