using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using LogSnag.Internal;

namespace LogSnag;

public sealed class LogSnag : ILogSnag
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

    public LogSnag(string apiToken) : this(new HttpClient(), apiToken)
    {
    }

    public LogSnag(HttpClient httpClient, string apiToken)
    {
        if (string.IsNullOrWhiteSpace(apiToken))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(apiToken));
        }

        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _apiToken = apiToken;

        _httpClient.BaseAddress = new Uri("https://api.logsnag.com");
        _httpClient.DefaultRequestHeaders.Clear();
    }

    public async Task Publish(LogSnagEvent @event)
    {
        var body = JsonSerializer.Serialize(@event, PublishJsonSerializerOptions);

        var logRequest = new HttpRequestMessage(HttpMethod.Post, "/v1/log")
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
            throw new LogSnagException("Error while publishing event to LogSnag. See inner exception for details.", e);
        }

        if (!response.IsSuccessStatusCode)
        {
            throw new LogSnagResponseException("Not successful response while publishing event to LogSnag.", response);
        }
    }

    public Task Insight(LogSnagInsight insight)
    {
        throw new NotImplementedException();
    }
}