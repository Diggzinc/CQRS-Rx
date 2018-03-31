using System.Threading;
using System.Threading.Tasks;

namespace CQRSRx.Queries
{
    /// <summary>
    /// Asynchronous handler for queries.
    /// </summary>
    public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
    {
        /// <summary>
        /// Handles a query asynchronously.
        /// </summary>
        /// <param name="query">The query to handle.</param>
        /// <returns>The correspondent task for the query handling.</returns>
        Task<TResult> HandleAsync(TQuery query);

        /// <summary>
        /// Handles a query asynchronously with a custom cancellation token.
        /// </summary>
        /// <param name="query">The query to handle.</param>
        /// <param name="cancellationToken">The custom cancellation token.</param>
        /// <returns>The correspondent task for the query handling.</returns>
        Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken);
    }
}