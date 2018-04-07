using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using CQRSRx.Events;
using CQRSRx.Tests.Domain.Fakes;
using NSubstitute;
using Xunit;

namespace CQRSRx.Tests.Domain
{
    [Trait("Domain", "Aggregate")]
    public partial class AggregateTests
    {
        [Fact]
        public void Should_Redirect_Change_Created_By_Action_To_Event_Handler()
        {
            var aggregate = Substitute.For<FakeAggregate>();

            aggregate.PerformAction();

            (aggregate as IEventHandler<FakeActionPerformed>).Received().Apply(Arg.Any<FakeActionPerformed>());
        }
    }
}
