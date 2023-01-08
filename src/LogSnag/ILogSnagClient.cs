namespace LogSnag;

public interface ILogSnagClient
{
    /// <summary>
    /// Use this method to publish your events to LogSnag.
    ///
    /// These events may be designed in any way that makes sense for your application.
    /// </summary>
    /// <param name="event">Event to be published</param>
    Task Publish(LogSnagEvent @event);

    /// <summary>
    /// Publish an insight to LogSnag.
    /// 
    /// Insights are real-time events such as KPI, performance, and other metrics that are
    /// not captured as a regular event. You can publish them periodically or as soon as they
    /// occur and the latest value will be stored in LogSnag.
    /// </summary>
    /// <param name="insight">Insight to be published</param>
    Task Insight(LogSnagInsight insight);
}