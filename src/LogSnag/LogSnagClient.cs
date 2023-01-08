using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using LogSnag.Internal;

namespace LogSnag;

public sealed class LogSnagClient : ILogSnagClient
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions PublishJsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters =
        {
            new LogSnagTagJsonConverter(),
            new LogSnagTagsJsonConverter(),
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
        }
    };

    private static readonly JsonSerializerOptions InsightJsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
        }
    };

    /// <summary>
    /// Creates a new instance of LogSnagClient.
    ///
    /// This will create an new internal HttpClient instance which will
    /// be used throughout the lifetime of this instance
    /// </summary>
    /// <param name="apiToken">API token required for publishing your events to LogSnag</param>
    public LogSnagClient(string apiToken) : this(new HttpClient(), apiToken)
    {
    }

    /// <summary>
    /// Creates a new instance of LogSnagClient.
    /// </summary>
    /// <param name="httpClient">HTTP client which will be used to access LogSnag API.</param>
    /// <param name="apiToken">API token required for publishing your events to LogSnag</param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public LogSnagClient(HttpClient httpClient, string apiToken)
    {
        if (string.IsNullOrWhiteSpace(apiToken))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(apiToken));
        }

        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

        _httpClient.BaseAddress = new Uri("https://api.logsnag.com/v1/");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiToken);
    }

    /// <inheritdoc cref="ILogSnagClient.Publish"/>
    public async Task Publish(LogSnagEvent @event) => await Send(
        HttpMethod.Post,
        @event,
        PublishJsonSerializerOptions,
        "log",
        "Error while publishing an event to LogSnag. See inner exception for details.",
        "Not successful response while publishing event to LogSnag.");

    /// <inheritdoc cref="ILogSnagClient.Insight"/>
    public async Task Insight(LogSnagInsight insight) => await Send(
        HttpMethod.Post,
        insight,
        InsightJsonSerializerOptions,
        "insight",
        "Error while publishing an insight to LogSnag. See inner exception for details.",
        "Not successful response while publishing insight to LogSnag.");

    private async Task Send<TValue>(
        HttpMethod httpMethod,
        TValue value, 
        JsonSerializerOptions jsonSerializerOptions,
        string requestUri,
        string clientErrorMessage,
        string responseErrorMessage)
    {
        var body = JsonSerializer.Serialize(value, jsonSerializerOptions);

        var logRequest = new HttpRequestMessage(httpMethod, requestUri)
        {
            Content = new StringContent(body, null, "application/json")
        };

        HttpResponseMessage response;

        try
        {
            response = await _httpClient.SendAsync(logRequest);
        }
        catch (Exception e)
        {
            throw new LogSnagException(clientErrorMessage, e);
        }

        if (!response.IsSuccessStatusCode)
        {
            throw new LogSnagResponseException(
                responseErrorMessage,
                await response.Content.ReadAsStringAsync(),
                response);
        }
    }
}