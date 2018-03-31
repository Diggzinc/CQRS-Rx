using System;
using System.Threading;
using System.Threading.Tasks;
using CQRSRx.Commands;
using CQRSRx.Integration.SimpleInjector;
using CQRSRx.Integration.SimpleInjector.Exceptions;
using CQRSRx.Queries;
using CQRSRx.Tests.SimpleInjector.Stubs;
using Moq;
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
            var command = Mock.Of<ICommand>();

            using (var container = new Container())
            {
                IProcessor processor = new SimpleInjectorProcessor(container);
                await Assert.ThrowsAsync<UnresolvedHandlerException>(async () => await processor.ProcessAsync(command));
            }
        }

        [Fact]
        public async Task Should_Throw_Exception_On_Unresolved_QueryHandler()
        {
            var query = Mock.Of<IQuery<object>>();

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
            var handlerMock = new Mock<ICommandHandler<StubCommand>>();
            handlerMock
                .Setup(h => h.HandleAsync(It.IsAny<StubCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException());

            using (var container = new Container())
            {
                container.Register<ICommandHandler<StubCommand>>(() => handlerMock.Object);
                container.Verify();

                IProcessor processor = new SimpleInjectorProcessor(container);
                await Assert.ThrowsAsync<ArgumentException>(async () => await processor.ProcessAsync(command));
            }
        }

        [Fact]
        public async Task Should_Not_Wrap_Exception_When_QueryHandler_Throws_Exception()
        {
            var query = new StubQuery<object>();
            var handlerMock = new Mock<IQueryHandler<StubQuery<object>, object>>();
            handlerMock
                .Setup(h => h.HandleAsync(It.IsAny<StubQuery<object>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException());

            using (var container = new Container())
            {
                container.Register<IQueryHandler<StubQuery<object>, object>>(() => handlerMock.Object);
                container.Verify();

                IProcessor processor = new SimpleInjectorProcessor(container);
                await Assert.ThrowsAsync<ArgumentException>(async () => await processor.ProcessAsync(query));
            }
        }

        [Fact]
        public async Task Should_Process_Command()
        {
            var command = new StubCommand();
            var handlerMock = new Mock<ICommandHandler<StubCommand>>();
            handlerMock
                .Setup(h => h.HandleAsync(It.IsAny<StubCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            using (var container = new Container())
            {
                container.Register<ICommandHandler<StubCommand>>(() => handlerMock.Object);
                container.Verify();

                IProcessor processor = new SimpleInjectorProcessor(container);
                await processor.ProcessAsync(command);

                handlerMock.Verify();
            }
        }

        [Fact]
        public async Task Should_Process_Query()
        {
            const string expectedResult = "value";
            var query = new StubQuery<string>();
            var handlerMock = new Mock<IQueryHandler<StubQuery<string>, string>>();
            handlerMock
                .Setup(h => h.HandleAsync(It.IsAny<StubQuery<string>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult)
                .Verifiable();

            using (var container = new Container())
            {
                container.Register<IQueryHandler<StubQuery<string>, string>>(() => handlerMock.Object);
                container.Verify();

                IProcessor processor = new SimpleInjectorProcessor(container);
                var actualResult = await processor.ProcessAsync(query);

                handlerMock.Verify();
                Assert.Equal(expectedResult, actualResult);
            }
        }
    }
}
