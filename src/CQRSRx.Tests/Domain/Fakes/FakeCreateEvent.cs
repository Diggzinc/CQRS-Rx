using System;
using CQRSRx.Events;

namespace CQRSRx.Tests.Domain.Fakes
{
    public class FakeCreateEvent : IEvent
    {
        public long Version { get; set; }

        public Guid Id { get; }

        public string Name { get; }

        public FakeCreateEvent(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
