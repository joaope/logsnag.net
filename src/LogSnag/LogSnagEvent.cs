namespace LogSnag;

public sealed class LogSnagEvent
{
    public LogSnagEvent(string project, string channel, string @event)
    {
        if (string.IsNullOrEmpty(project))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(project));
        }

        if (string.IsNullOrEmpty(channel))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(channel));
        }

        if (string.IsNullOrEmpty(@event))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(@event));
        }

        Project = project;
        Channel = channel;
        Event = @event;
    }

    public string Project { get; }
    public string Channel { get; }
    public string Event { get; }
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public bool Notify { get; set; } = true;
    public LogSnagTags Tags { get; } = new();
    public LogSnagParser? Parser { get; set; }
}