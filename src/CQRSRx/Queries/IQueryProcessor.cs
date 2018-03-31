
using System.Threading;
using System.Threading.Tasks;

namespace CQRSRx.Queries
{
    /// <summary>
    /// Defines the processor for queries.
    /// The QueryProcessor is responsible to route the queries to their respective handler.
    /// </summary>
    public interface IQueryProcessor
    {
        /// <summary>
        /// Routes the query to their respective handler asynchronously.
        /// </summary>
        /// <param name="query">The query to route.</param>
        /// <returns>The correspondent task for the query handling.</returns>
        Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query);

        /// <summary>
        /// Routes the query to their respective handler asynchronously with a custom cancellation token.
        /// </summary>
        /// <param name="query">The query to route.</param>
        /// <param name="cancellationToken">The custom cancellation token.</param>
        /// <returns>The correspondent task for the query handling.</returns>
        Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken);
    }
}