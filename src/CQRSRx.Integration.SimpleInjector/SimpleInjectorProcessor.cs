using System;
using System.Threading;
using System.Threading.Tasks;
using CQRSRx.Commands;
using CQRSRx.Queries;
using SimpleInjector;

namespace CQRSRx.Integration.SimpleInjector
{
    /// <summary>
    /// SimpleInjector implementation of the CQS processor/router.
    /// </summary>
    public class SimpleInjectorProcessor : IProcessor
    {
        private readonly Container _container;

        /// <summary>
        /// Creates a processor for commands and queries.
        /// </summary>
        /// <param name="container">The container with the registered handlers to be resolved based on the given commands/queries.</param>
        public SimpleInjectorProcessor(Container container)
        {
            _container = container;
        }

        /// <inheritdoc/>
        public Task ProcessAsync(ICommand command)
        {
            return ProcessAsync(command, default(CancellationToken));
        }

        /// <inheritdoc/>
        public Task ProcessAsync(ICommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query)
        {
            return ProcessAsync(query, default(CancellationToken));
        }

        /// <inheritdoc/>
        public Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
