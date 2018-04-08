using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading;
using CQRSRx.Events;
using CQRSRx.Exceptions;
using CQRSRx.Resources;

namespace CQRSRx.Domain
{
    /// <summary>
    /// The base type that a concrete aggregate root can extend.
    /// </summary>
    public abstract class BaseAggregateRoot : IAggregateRoot
    {
        private long _version = -1;

        /// <summary>
        /// The unique identifier of the aggregate root (domain object).
        /// </summary>
        protected Guid _id = Guid.Empty;

        private readonly Queue<IEvent> _uncommittedEvents = new Queue<IEvent>();

        private readonly ISubject<Change> _subject = new ReplaySubject<Change>();

        /// <inheritdoc/>
        Guid IAggregateRoot.Id => _id;

        /// <inheritdoc/>
        long IAggregateRoot.Version => _version;

        /// <inheritdoc/>
        void IObserver<IEvent>.OnCompleted() { }

        /// <inheritdoc/>
        void IObserver<IEvent>.OnError(Exception error) 
            => throw new CorruptedStreamException(StringResources.AggregateSourceStreamIsCorrupted(), error);

        /// <inheritdoc/>
        void IObserver<IEvent>.OnNext(IEvent evt)
        {
            var expectedVersion = _version + 1;

            if (evt.Version != expectedVersion)
            {
                throw new StreamEventOutOfOrderException(StringResources.AggregateSourceStreamProvidedEventOutOfOrder(expectedVersion, evt.Version));
            }

            Apply(evt);
            _version++;
        }

        /// <inheritdoc/>
        void IAggregateRoot.PublishChanges()
        {
            foreach (var evt in _uncommittedEvents)
            {
                _subject.OnNext(new Change(evt));
            }
            _uncommittedEvents.Clear();
        }

        /// <inheritdoc/>
        IDisposable IObservable<Change>.Subscribe(IObserver<Change> observer) => _subject.Subscribe(observer);

        /// <inheritdoc/>
        IEnumerator<UncommittedChange> IEnumerable<UncommittedChange>.GetEnumerator()
            => _uncommittedEvents.Select(e => new UncommittedChange(e)).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => (this as IEnumerable<UncommittedChange>).GetEnumerator();

        /// <summary>
        /// Applies the event to the aggregate.
        /// </summary>
        /// <param name="evt">the event to apply.</param>
        protected void ApplyChange(IEvent evt)
        {
            Apply(evt);
            _version++;
            evt.Version = _version;
            _uncommittedEvents.Enqueue(evt);
        }

        private void Apply<TEvent>(TEvent evt) where TEvent : IEvent
        {
            var handlerType = typeof(IEventHandler<>).MakeGenericType(evt.GetType());
            var method = handlerType.GetMethod(nameof(IEventHandler<TEvent>.Apply));
            method.Invoke(this, new object[] { evt });
        }
    }
}
