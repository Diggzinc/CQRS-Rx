using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSRx.Domain
{
    /// <inheritdoc />
    public class AggregateRootRepository<TAggregate> : IAggregateRootRepository<TAggregate> where TAggregate : IAggregateRoot
    {
        private readonly IStorage _storage;
        private readonly Func<TAggregate> _factory;

        /// <summary>
        /// Constructs an aggregate root repository.
        /// </summary>
        /// <param name="storage">The event storage for the aggregate root events.</param>
        /// <param name="factory">Factory method to instantiate new aggregates.</param>
        public AggregateRootRepository(IStorage storage, Func<TAggregate> factory)
        {
            _storage = storage;
            _factory = factory;
        }

        /// <inheritdoc/>
        public async Task<TAggregate> LoadAsync(Guid id, CancellationToken cancellationToken)
        {
            //TODO: Read from most up to date snapshot instead and reload only subsequent events.
            var aggregate = _factory();
            var events = await _storage.LoadEventsAsync(id, cancellationToken);
            events.ToObservable().Subscribe(aggregate);
            return aggregate;
        }

        /// <inheritdoc/>
        public async Task SaveAsync(TAggregate aggregate, CancellationToken cancellationToken)
        {
            await _storage.SaveChangesAsync(aggregate, cancellationToken);
            aggregate.PublishChanges();
        }
    }
}
