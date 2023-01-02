namespace LogSnag;

public interface ILogSnagHttpClient
{
    /// <summary>
    /// Publish an event to LogSnag.
    /// 
    /// This route is used to publish your events to LogSnag. These events may be designed
    /// in any way that makes sense for your application.
    /// </summary>
    /// <param name="event">Event to be published</param>
    /// <returns></returns>
    Task Publish(LogSnagEvent @event);

    /// <summary>
    /// Publish an insight to LogSnag.
    /// 
    /// Insights are real-time events such as KPI, performance, and other metrics that are
    /// not captured as a regular event. You can publish them periodically or as soon as they
    /// occur and the latest value will be stored in LogSnag.
    /// </summary>
    /// <param name="insight"></param>
    /// <returns></returns>
    Task Insight(LogSnagInsight insight);
}