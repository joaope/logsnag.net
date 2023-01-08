namespace LogSnag;

public sealed class LogSnagInsight
{
    /// <summary>
    /// Creates a new instance of LogSnagInsight.
    ///
    /// <paramref name="project"/>, <paramref name="title"/> and <paramref name="value"/>
    /// are mandatory fields.
    /// </summary>
    /// <param name="project">Project name to where this event will be published to.</param>
    /// <param name="title">Insight title.</param>
    /// <param name="value">Insight value.</param>
    /// <exception cref="ArgumentException"></exception>
    public LogSnagInsight(string project, string title, string value)
    {
        if (string.IsNullOrEmpty(project))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(project));
        }

        if (string.IsNullOrEmpty(title))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(title));
        }

        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(value));
        }
       
        Project = project;
        Title = title;
        Value = value;
    }

    /// <summary>
    /// Project name. Mandatory field.
    /// </summary>
    public string Project { get; }

    /// <summary>
    /// Insight title. Mandatory field.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Insight value. Mandatory field.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Single Emoji.
    /// </summary>
    public string? Icon { get; set; }
}