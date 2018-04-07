using CQRSRx.Events;

namespace CQRSRx.Tests.Domain.Fakes
{
        public class FakeActionPerformed : IEvent
        {
            public long Version { get; }
        }
}
