using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CQRSRx.Domain;
using CQRSRx.Events;
using CQRSRx.Exceptions;
using CQRSRx.Tests.Domain.Fakes;
using NSubstitute;
using Xunit;

namespace CQRSRx.Tests.Domain
{
    [Trait("Domain", "AggregateRoot")]
    public partial class AggregateTests
    {
        [Fact]
        public void Should_Redirect_Change_Created_By_Action_To_Event_Handler()
        {
            const int expectedUncommittedChanges = 2;
            const int expectedVersion = 1;

            var aggregate = new FakeAggregate(Guid.NewGuid(), nameof(FakeAggregate));

            aggregate.PerformAction();

            var uncommittedChanges = (aggregate as IEnumerable<UncommittedChange>);

            Assert.Equal(expectedVersion, (aggregate as IAggregateRoot).Version);
            Assert.Equal(expectedUncommittedChanges, uncommittedChanges.Count());
            Assert.Collection(
                uncommittedChanges,
                uncommittedChange =>
                {
                    Assert.NotNull(uncommittedChange.Event);
                    Assert.IsType<FakeCreateEvent>(uncommittedChange.Event);
                },
                uncommittedChange =>
                {
                    Assert.NotNull(uncommittedChange.Event);
                    Assert.IsType<FakeDummyActionEvent>(uncommittedChange.Event);
                });
        }

        [Fact]
        public void Should_Throw_Error_When_Loading_An_Event_Stream_With_Events_Out_Of_Order()
        {
            FakeAggregate AggregateFactory() => (FakeAggregate)Activator.CreateInstance(typeof(FakeAggregate), true);
            var aggregateId = Guid.NewGuid();

            var unorderedStream = new IEvent[]
            {
                new FakeCreateEvent(aggregateId, nameof(FakeAggregate)) { Version = 0 },
                new FakeDummyActionEvent() { Version = 1 },
                new FakeDummyActionEvent() { Version = 3 }, // event out of order
                new FakeDummyActionEvent() { Version = 2 }
            }
            .ToObservable();

            var aggregate = AggregateFactory();
            Assert.Throws<StreamEventOutOfOrderException>(() => unorderedStream.Subscribe(aggregate));
        }

        [Fact]
        public void Should_Throw_Error_When_Loading_A_Corrupt_Event_Stream()
        {
            FakeAggregate AggregateFactory() => (FakeAggregate)Activator.CreateInstance(typeof(FakeAggregate), true);
            var aggregateId = Guid.NewGuid();

            var corruptedStream = new IEvent[]
            {
                new FakeCreateEvent(aggregateId, nameof(FakeAggregate)) { Version = 0 },
                new FakeDummyActionEvent() { Version = 1 },
                new FakeDummyActionEvent() { Version = 2 },
                new FakeDummyActionEvent() { Version = 3 }
            }
            .ToObservable()
            .Concat(Observable.Throw<IEvent>(new Exception()));

            var aggregate = AggregateFactory();

            Assert.Throws<CorruptedStreamException>(() => corruptedStream.Subscribe(aggregate));
        }

        [Fact]
        public async Task Should_Load_Aggregate_Correctly_Through_Repository()
        {
            FakeAggregate AggregateFactory() => (FakeAggregate)Activator.CreateInstance(typeof(FakeAggregate), true);

            var aggregateId = Guid.NewGuid();
            var cancellationToken = default(CancellationToken);
            var expectedName = $"{nameof(FakeAggregate)}Renamed";
            const int expectedVersion = 1;

            var storedEvents = new IEvent[]
            {
                new FakeCreateEvent(aggregateId, nameof(FakeAggregate)) { Version = 0 },
                new FakeRenameEvent(nameof(FakeAggregate), expectedName) { Version = 1 }
            };

            var storage = Substitute.For<IStorage>();
            storage
                .LoadEventsAsync(aggregateId, cancellationToken)
                .Returns(storedEvents);

            var repository = new AggregateRootRepository<FakeAggregate>(storage, AggregateFactory);

            var aggregate = await repository.LoadAsync(aggregateId, cancellationToken);

            Assert.Equal(expectedVersion, (aggregate as IAggregateRoot).Version);
            Assert.Equal(aggregateId, (aggregate as IAggregateRoot).Id);
            Assert.Equal(expectedName, aggregate.Name);
        }

        [Fact]
        public async Task Should_Publish_Aggregate_Events_When_Saving_Through_Repository()
        {
            const int expectedChanges = 2;

            var changes = new List<Change>();
            var aggregateId = Guid.NewGuid();
            var cancellationToken = default(CancellationToken);

            var storage = Substitute.For<IStorage>();
            var repository = new AggregateRootRepository<FakeAggregate>(storage, null);

            var aggregate = new FakeAggregate(aggregateId, nameof(FakeAggregate));
            aggregate.Subscribe(changes.Add);

            aggregate.PerformAction();

            Assert.Empty(changes);

            await repository.SaveAsync(aggregate, cancellationToken);

            Assert.Equal(expectedChanges, changes.Count);
            Assert.Collection(
                changes,
                change =>
                {
                    Assert.NotNull(change.Event);
                    Assert.IsType<FakeCreateEvent>(change.Event);
                },
                change =>
                {
                    Assert.NotNull(change.Event);
                    Assert.IsType<FakeDummyActionEvent>(change.Event);
                });
        }

        [Fact]
        public void Should_Enumerate_Aggregate()
        {
            IEnumerable aggregate = new FakeAggregate(Guid.NewGuid(), nameof(FakeAggregate));

            Assert.Single(aggregate);

            foreach (var change in aggregate)
            {
                Assert.IsType<UncommittedChange>(change);
            }
        }
    }
}
