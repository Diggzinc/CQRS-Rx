using System;
using System.Threading;
using System.Threading.Tasks;
using CQRSRx.Commands;
using CQRSRx.Integration.SimpleInjector;
using CQRSRx.Integration.SimpleInjector.Exceptions;
using CQRSRx.Queries;
using CQRSRx.Tests.SimpleInjector.Stubs;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using SimpleInjector;
using Xunit;

namespace CQRSRx.Tests.SimpleInjector
{
    [Trait("Integration", "SimpleInjector")]
    public class SimpleInjectorProcessorTests
    {
        [Fact]
        public async Task Should_Throw_Exception_On_Unresolved_CommandHandler()
        {
            var command = Substitute.For<ICommand>();

            using (var container = new Container())
            {
                IProcessor processor = new SimpleInjectorProcessor(container);
                await Assert.ThrowsAsync<UnresolvedHandlerException>(async () => await processor.ProcessAsync(command));
            }
        }

        [Fact]
        public async Task Should_Throw_Exception_On_Unresolved_QueryHandler()
        {
            var query = Substitute.For<IQuery<object>>();

            using (var container = new Container())
            {
                IProcessor processor = new SimpleInjectorProcessor(container);
                await Assert.ThrowsAsync<UnresolvedHandlerException>(async () => await processor.ProcessAsync(query));
            }
        }

        [Fact]
        public async Task Should_Not_Wrap_Exception_When_CommandHandler_Throws_Exception()
        {
            var command = new StubCommand();
            var handler = Substitute.For<ICommandHandler<StubCommand>>();
            handler
                .HandleAsync(Arg.Any<StubCommand>(), Arg.Any<CancellationToken>())
                .Throws(new ArgumentException());

            using (var container = new Container())
            {
                container.Register(() => handler);
                container.Verify();

                IProcessor processor = new SimpleInjectorProcessor(container);
                await Assert.ThrowsAsync<ArgumentException>(async () => await processor.ProcessAsync(command));
            }
        }

        [Fact]
        public async Task Should_Not_Wrap_Exception_When_QueryHandler_Throws_Exception()
        {
            var query = new StubQuery<object>();
            var handler = Substitute.For<IQueryHandler<StubQuery<object>, object>>();
            handler
                .HandleAsync(Arg.Any<StubQuery<object>>(), Arg.Any<CancellationToken>())
                .Throws(new ArgumentException());

            using (var container = new Container())
            {
                container.Register(() => handler);
                container.Verify();

                IProcessor processor = new SimpleInjectorProcessor(container);
                await Assert.ThrowsAsync<ArgumentException>(async () => await processor.ProcessAsync(query));
            }
        }

        [Fact]
        public async Task Should_Process_Command()
        {
            var command = new StubCommand();
            var handler = Substitute.For<ICommandHandler<StubCommand>>();
            handler
                .HandleAsync(Arg.Any<StubCommand>(), Arg.Any<CancellationToken>())
                .Returns(Task.CompletedTask);

            using (var container = new Container())
            {
                container.Register(() => handler);
                container.Verify();

                IProcessor processor = new SimpleInjectorProcessor(container);
                await processor.ProcessAsync(command);

                await handler
                    .Received()
                    .HandleAsync(Arg.Any<StubCommand>(), Arg.Any<CancellationToken>());
            }
        }

        [Fact]
        public async Task Should_Process_Query()
        {
            const string expectedResult = "value";
            var query = new StubQuery<string>();
            var handler = Substitute.For<IQueryHandler<StubQuery<string>, string>>();
            handler
                .HandleAsync(Arg.Any<StubQuery<string>>(), Arg.Any<CancellationToken>())
                .Returns(expectedResult);

            using (var container = new Container())
            {
                container.Register(() => handler);
                container.Verify();

                IProcessor processor = new SimpleInjectorProcessor(container);
                var actualResult = await processor.ProcessAsync(query);

                await handler
                    .Received()
                    .HandleAsync(Arg.Any<StubQuery<string>>(), Arg.Any<CancellationToken>());

                Assert.Equal(expectedResult, actualResult);
            }
        }
    }
}
