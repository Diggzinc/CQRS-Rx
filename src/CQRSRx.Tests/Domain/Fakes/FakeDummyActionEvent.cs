using CQRSRx.Events;

namespace CQRSRx.Tests.Domain.Fakes
{
        public class FakeDummyActionEvent : IEvent
        {
            public long Version { get; set; }
        }
}
