

using CQRSRx.Events;

public class FakeRenameEvent : IEvent
{
    public long Version { get; set; }

    public string From { get; }
    
    public string To { get; }

    public FakeRenameEvent(string from, string to)
    {
        From = from;
        To = to;
    }
}
