namespace LogSnag;

public class LogSnagException : Exception
{
    public LogSnagException(string message, Exception? innerException) : base(message, innerException)
    {
    }

    public LogSnagException(string message) : this(message, null)
    {
    }
}