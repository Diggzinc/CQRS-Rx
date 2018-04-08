using System;
using CQRSRx.Domain;
using CQRSRx.Events;

namespace CQRSRx.Tests.Domain.Fakes
{
    public class FakeAggregate :
        BaseAggregateRoot,
        IEventHandler<FakeCreateEvent>,
        IEventHandler<FakeDummyActionEvent>,
        IEventHandler<FakeRenameEvent>
    {
        public string Name { get; private set; }

        public void PerformAction()
        {
            ApplyChange(new FakeDummyActionEvent());
        }

        public void Rename(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));
            }
            ApplyChange(new FakeRenameEvent(Name, name));
        }

        private FakeAggregate()
        {
        }

        void IEventHandler<FakeCreateEvent>.Apply(FakeCreateEvent evt)
        {
            _id = evt.Id;
            Name = evt.Name;
        }

        public FakeAggregate(Guid id, string name)
        {
            ApplyChange(new FakeCreateEvent(id, name));
        }

        void IEventHandler<FakeDummyActionEvent>.Apply(FakeDummyActionEvent evt)
        {
        }

        void IEventHandler<FakeRenameEvent>.Apply(FakeRenameEvent evt)
        {
            Name = evt.To;
        }
    }
}
