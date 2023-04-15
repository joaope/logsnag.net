using System.Text.Json.Serialization;
using LogSnag.Internal;

namespace LogSnag;

public sealed class LogSnagHistoricalEvent
{
    /// <summary>
    /// Creates a new instance of a LogSnagHistoricalEvent.
    /// 
    /// <paramref name="project"/>, <paramref name="channel"/> and
    /// <paramref name="event"/> are mandatory fields that cannot be null or empty.
    /// </summary>
    /// <param name="timestamp">Event's date and time</param>
    /// <param name="project">Project name to where this event will be published to.</param>
    /// <param name="channel">Channel name to where this event will be published to.</param>
    /// <param name="event">Event name.</param>
    /// <exception cref="ArgumentException"></exception>
    public LogSnagHistoricalEvent(DateTimeOffset timestamp, string project, string channel, string @event)
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

        Timestamp = timestamp;
        Project = project;
        Channel = channel;
        Event = @event;
    }

    /// <summary>
    /// Event's date and time. Mandatory field.
    /// </summary>
    [JsonConverter(typeof(DateTimeOffsetToUnixTimestampJsonConverter))]
    public DateTimeOffset Timestamp { get; }

    /// <summary>
    /// Project name. Mandatory field.
    /// </summary>
    public string Project { get; }
    
    /// <summary>
    /// Channel name. Mandatory field.
    /// </summary>
    public string Channel { get; }

    /// <summary>
    /// Event name. Mandatory field.
    /// </summary>
    public string Event { get; }

    /// <summary>
    /// Event description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Single Emoji.
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// <value>true</value> to send push notifications; otherwise <value>false</value>.
    ///
    /// This is <value>true</value> be default.
    /// </summary>
    public bool Notify { get; set; } = true;

    /// <summary>
    /// Event tags
    /// </summary>
    public LogSnagTags Tags { get; } = new();

    /// <summary>
    /// Parser type. Markdown or Text.
    /// </summary>
    public LogSnagParser? Parser { get; set; }
}