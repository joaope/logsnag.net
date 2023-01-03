using System.Net;
using System.Text.Json;

namespace LogSnag;

public sealed class LogSnagResponseException : LogSnagException
{
    private readonly Lazy<ErrorResponse?> _lazyError;

    public HttpResponseMessage RawResponse { get; }
    public HttpStatusCode StatusCode => RawResponse.StatusCode;
    public ErrorResponse? Error => _lazyError.Value;

    private static readonly JsonSerializerOptions ErrorSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public LogSnagResponseException(string message, string contentString, HttpResponseMessage rawResponse)
        : base(message)
    {
        RawResponse = rawResponse;

        _lazyError = new Lazy<ErrorResponse?>(() =>
        {
            try
            {
                return JsonSerializer.Deserialize<ErrorResponse?>(contentString, ErrorSerializerOptions);
            }
            catch (JsonException)
            {
                return null;
            }
        });
    }

    public sealed class ErrorResponse
    {
        public string Message { get; }
        public ErrorResponseValidation Validation { get; }

        public ErrorResponse(string message, ErrorResponseValidation validation)
        {
            Message = message;
            Validation = validation;
        }

        public sealed class ErrorResponseValidation
        {
            public ErrorResponseValidationBodyItem[] Body { get; }

            public ErrorResponseValidation(ErrorResponseValidationBodyItem[] body)
            {
                Body = body;
            }

            public sealed class ErrorResponseValidationBodyItem
            {
                public string Path { get; }
                public string Type { get; }
                public string Message { get; }

                public ErrorResponseValidationBodyItem(string path, string type, string message)
                {
                    Path = path;
                    Type = type;
                    Message = message;
                }
            }
        }
    }
}