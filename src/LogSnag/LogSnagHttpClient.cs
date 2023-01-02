using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using LogSnag.Internal;

namespace LogSnag;

public sealed class LogSnagHttpClient : ILogSnagHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiToken;

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

    public LogSnagHttpClient(string apiToken) : this(new HttpClient(), apiToken)
    {
    }

    public LogSnagHttpClient(HttpClient httpClient, string apiToken)
    {
        if (string.IsNullOrWhiteSpace(apiToken))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(apiToken));
        }

        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _apiToken = apiToken;

        _httpClient.BaseAddress = new Uri("https://api.logsnag.com/v1/");
        _httpClient.DefaultRequestHeaders.Clear();
    }

    public async Task Publish(LogSnagEvent @event) => await Send(
        HttpMethod.Post,
        @event,
        PublishJsonSerializerOptions,
        "log",
        "Error while publishing an event to LogSnag. See inner exception for details.",
        "Not successful response while publishing event to LogSnag.");

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
            Content = new StringContent(body, null, "application/json"),
            Headers =
            {
                Authorization = new AuthenticationHeaderValue("Bearer", _apiToken)
            }
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
            throw new LogSnagResponseException(responseErrorMessage, response);
        }
    }
}