using System.Net;
using System.Text.Json;

namespace LogSnag;

public sealed class LogSnagResponseException : LogSnagException
{
    private static readonly JsonSerializerOptions ErrorSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };
    private readonly Lazy<LogSnagErrorResponse?> _lazyError;

    public HttpResponseMessage RawResponse { get; }
    public HttpStatusCode StatusCode => RawResponse.StatusCode;
    public LogSnagErrorResponse? Error => _lazyError.Value;

    internal LogSnagResponseException(string message, string? contentString, HttpResponseMessage rawResponse)
        : base(message)
    {
        RawResponse = rawResponse;

        _lazyError = new Lazy<LogSnagErrorResponse?>(() =>
        {
            if (string.IsNullOrWhiteSpace(contentString))
            {
                return null;
            }

            try
            {
                return JsonSerializer.Deserialize<LogSnagErrorResponse?>(contentString!, ErrorSerializerOptions);
            }
            catch (JsonException)
            {
                return null;
            }
        });
    }
}