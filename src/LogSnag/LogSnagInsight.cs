namespace LogSnag;

public sealed class LogSnagInsight
{
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

    public string Project { get; }
    public string Title { get; }
    public string Value { get; }
    public string? Icon { get; set; }
}