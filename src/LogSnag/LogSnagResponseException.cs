namespace LogSnag;

public sealed class LogSnagResponseException : LogSnagException
{
    public HttpResponseMessage Response { get; }

    public LogSnagResponseException(string message, HttpResponseMessage response)
        : base(message)
    {
        Response = response;
    }
}