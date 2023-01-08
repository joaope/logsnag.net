namespace LogSnag;

public class LogSnagException : Exception
{
    internal LogSnagException(string message, Exception? innerException) : base(message, innerException)
    {
    }

    internal LogSnagException(string message) : this(message, null)
    {
    }
}