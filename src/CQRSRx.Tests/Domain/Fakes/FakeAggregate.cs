using CQRSRx.Domain;
using CQRSRx.Events;

namespace CQRSRx.Tests.Domain.Fakes
{
    public class FakeAggregate : BaseAggregateRoot, IEventHandler<FakeActionPerformed>
    {
        public void PerformAction()
        {
            ApplyChange(new FakeActionPerformed());
        }

        public FakeAggregate()
        {
        }

        void IEventHandler<FakeActionPerformed>.Apply(FakeActionPerformed evt)
        {
        }
    }
}
