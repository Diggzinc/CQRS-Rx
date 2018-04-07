using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CQRSRx.Commands;
using CQRSRx.Integration.SimpleInjector.Exceptions;
using CQRSRx.Integration.SimpleInjector.Resources;
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
        [DebuggerStepThrough]
        public Task ProcessAsync(ICommand command)
        {
            return ProcessAsync(command, default(CancellationToken));
        }

        /// <inheritdoc/>
        [DebuggerStepThrough]
        public Task ProcessAsync(ICommand command, CancellationToken cancellationToken)
        {
            var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());

            try
            {
                var handler = _container.GetInstance(handlerType);
                var method = handlerType.GetMethod(nameof(ICommandHandler<ICommand>.HandleAsync));
                return (Task)method.Invoke(handler, new object[] { command, cancellationToken });
            }
            catch (ActivationException exception)
            {
                throw new UnresolvedHandlerException(
                    StringResources.CannotResolveHandler(handlerType),
                    exception);
            }
            catch (TargetInvocationException exception)
            {
                throw exception.InnerException;
            }
        }

        /// <inheritdoc/>
        [DebuggerStepThrough]
        public Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query)
        {
            return ProcessAsync(query, default(CancellationToken));
        }

        /// <inheritdoc/>
        [DebuggerStepThrough]
        public Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));

            try
            {
                var handler = _container.GetInstance(handlerType);
                var method = handlerType.GetMethod(nameof(IQueryHandler<IQuery<TResult>, TResult>.HandleAsync));
                return (Task<TResult>)method.Invoke(handler, new object[] { query, cancellationToken });
            }
            catch (ActivationException exception)
            {
                throw new UnresolvedHandlerException(
                    StringResources.CannotResolveHandler(handlerType),
                    exception);
            }
            catch (TargetInvocationException exception)
            {
                throw exception.InnerException;
            }
        }
    }
}
