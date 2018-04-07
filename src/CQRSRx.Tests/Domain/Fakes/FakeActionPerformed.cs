using CQRSRx.Events;

namespace CQRSRx.Tests.Domain
{
        public class FakeActionPerformed : IEvent
        {
            public long Version { get; }
        }
}
