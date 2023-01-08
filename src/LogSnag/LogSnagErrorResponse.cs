namespace LogSnag;

public sealed class LogSnagErrorResponse
{
    public string Message { get; }
    public Validations Validation { get; }

    public LogSnagErrorResponse(string message, Validations validation)
    {
        Message = message;
        Validation = validation;
    }

    public sealed class Validations
    {
        public Item[]? Headers { get; }
        public Item[]? Body { get; }

        public Validations(
            Item[]? headers,
            Item[]? body)
        {
            Headers = headers;
            Body = body;
        }

        public sealed class Item
        {
            public string Path { get; }
            public string Type { get; }
            public string Message { get; }

            public Item(string path, string type, string message)
            {
                Path = path;
                Type = type;
                Message = message;
            }
        }
    }
}