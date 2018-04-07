using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CQRSRx.Domain;
using CQRSRx.Events;
using NSubstitute;
using Xunit;

namespace CQRSRx.Tests.Domain
{
    [Trait("Domain", "AggregateRoot")]
    public class AggregateRootTests
    {
        [Fact]
        public async Task Should_Load_From_Storage_When_Loading_Aggregate()
        {
            IAggregateRoot AggregateFactory() => Substitute.For<IAggregateRoot>();
            var aggregateId = Guid.NewGuid();

            var cancellationToken = default(CancellationToken);

            var storage = Substitute.For<IStorage>();
            var storedEvents = new IEvent[0];

            storage
                .LoadEventsAsync(aggregateId, cancellationToken)
                .Returns(storedEvents);

            IAggregateRootRepository<IAggregateRoot> repository = new AggregateRootRepository<IAggregateRoot>(storage, AggregateFactory);

            await repository.LoadAsync(aggregateId, cancellationToken);

            await storage
                .Received()
                .LoadEventsAsync(aggregateId, cancellationToken);
        }

        [Fact]
        public async Task Should_Save_On_Storage_When_Saving_Aggregate()
        {
            IAggregateRoot AggregateFactory() => Substitute.For<IAggregateRoot>();
            var cancellationToken = default(CancellationToken);

            var storage = Substitute.For<IStorage>();

            storage
                .SaveChangesAsync(Arg.Any<IEnumerable<UncommittedChange>>(), cancellationToken)
                .Returns(Task.CompletedTask);

            var aggregate = Substitute.For<IAggregateRoot>();
            var repository = new AggregateRootRepository<IAggregateRoot>(storage, AggregateFactory);

            await repository.SaveAsync(aggregate, cancellationToken);

            await storage
                .Received()
                .SaveChangesAsync(Arg.Any<IEnumerable<UncommittedChange>>(), cancellationToken);
        }

        [Fact]
        public async Task Should_Publish_Aggregate_Changes_After_Saving_Aggregate()
        {
            IAggregateRoot AggregateFactory() => Substitute.For<IAggregateRoot>();
            var cancellationToken = default(CancellationToken);

            var storage = Substitute.For<IStorage>();
            storage
                .SaveChangesAsync(Arg.Any<IEnumerable<UncommittedChange>>(), cancellationToken)
                .Returns(Task.CompletedTask);

            var aggregate = Substitute.For<IAggregateRoot>();

            var repository = new AggregateRootRepository<IAggregateRoot>(storage, AggregateFactory);

            await repository.SaveAsync(aggregate, cancellationToken);

            Received.InOrder(() =>
            {
                storage.SaveChangesAsync(Arg.Any<IEnumerable<UncommittedChange>>(), cancellationToken);
                aggregate.PublishChanges();
            });
        }
    }
}
