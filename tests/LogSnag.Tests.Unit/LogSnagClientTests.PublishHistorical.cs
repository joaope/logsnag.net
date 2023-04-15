using System.Net;
using LogSnag.Tests.Unit.Mocking;

namespace LogSnag.Tests.Unit;

public sealed partial class LogSnagClientTests
{
    [Fact]
    public async Task PublishHistorical_OkResponse_CorrectRequest()
    {
        var httpClient = MockHttpClient.Ok;
        var logSnag = new LogSnagClient(httpClient, "test-api-token");

        await logSnag.PublishHistorical(new LogSnagHistoricalEvent(
            new DateTimeOffset(2010, 3, 12, 21, 0, 0, 0, TimeSpan.Zero),
            "logsnag-net",
            "test-channel",
            "AnEvent")
        {
            Tags =
            {
                new LogSnagTag("key-one", 1),
                new LogSnagTag("key-two", "val2"),
                new LogSnagTag("bool", true)
            },
            Notify = true,
            Parser = LogSnagParser.Text
        });

        Assert.Single(httpClient.Requests);
        Assert.Equal(HttpMethod.Post, httpClient.LastRequest.Method);
        Assert.Single(httpClient.LastRequest.Headers);
        Assert.Equal("Bearer", httpClient.LastRequest.Headers.Authorization!.Scheme);
        Assert.Equal("test-api-token", httpClient.LastRequest.Headers.Authorization!.Parameter);
        Assert.Single(httpClient.LastRequest.Content!.Headers);
        Assert.Equal("application/json", httpClient.LastRequest.Content!.Headers.ContentType!.MediaType);
        Assert.Equal("utf-8", httpClient.LastRequest.Content!.Headers.ContentType!.CharSet);
        Assert.Equal("https://api.logsnag.com/v1/log", httpClient.LastRequest.RequestUri!.ToString());
        Assert.Equal(
            @"{""timestamp"":""1268427600000"",""project"":""logsnag-net"",""channel"":""test-channel"",""event"":""AnEvent"",""notify"":true,""tags"":{""key-one"":1,""key-two"":""val2"",""bool"":true},""parser"":""text""}",
            await httpClient.LastRequest.Content!.ReadAsStringAsync());
    }

    [Fact]
    public async Task PublishHistorical_NotOkResponse_ThrowsResponseException()
    {
        var response = new HttpResponseMessage(HttpStatusCode.BadGateway);
        var httpClient = new MockHttpClient(response);
        var logSnag = new LogSnagClient(httpClient, "test-api-token");

        var exception = await Assert.ThrowsAsync<LogSnagResponseException>(async () =>
        {
            await logSnag.PublishHistorical(new LogSnagHistoricalEvent(
                new DateTimeOffset(2010, 3, 12, 21, 0, 0, 0, TimeSpan.Zero),
                "logsnag-net",
                "test-channel",
                "AnEvent")
            {
                Tags =
                {
                    new LogSnagTag("key-one", 1),
                    new LogSnagTag("key-two", "val2"),
                    new LogSnagTag("bool", true)
                },
                Notify = true,
                Parser = LogSnagParser.Text
            });
        });

        Assert.Same(response, exception.RawResponse);
        Assert.Equal(HttpStatusCode.BadGateway, exception.StatusCode);
        Assert.Equal("Not successful response while publishing historical event to LogSnag.", exception.Message);
    }

    [Fact]
    public async Task PublishHistorical_NotOkResponseWithErrorBody_ThrowsResponseException()
    {
        var httpClient = new MockHttpClient(new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent(@"{
    ""message"": ""Validation Error"",
    ""validation"": {
        ""body"": [
            {
                ""path"": ""event"",
                ""type"": ""too_small"",
                ""message"": ""Event name may not be an empty string""
            },
            {
                ""path"": ""tags.tag-1"",
                ""type"": ""custom"",
                ""message"": ""Tag keys must be lowercase characters, or dashes""
            }
        ]
    }
}")
        });
        var logSnag = new LogSnagClient(httpClient, "test-api-token");

        var exception = await Assert.ThrowsAsync<LogSnagResponseException>(async () =>
        {
            await logSnag.PublishHistorical(new LogSnagHistoricalEvent(DateTimeOffset.MinValue, "logsnag-net", "test-channel", "AnEvent"));
        });
        
        Assert.Equivalent(new LogSnagErrorResponse(
            "Validation Error",
            new LogSnagErrorResponse.Validations(
                null,
                new []
                {
                    new LogSnagErrorResponse.Validations.Item(
                        "event",
                        "too_small",
                        "Event name may not be an empty string"),
                    new LogSnagErrorResponse.Validations.Item(
                        "tags.tag-1",
                        "custom",
                        "Tag keys must be lowercase characters, or dashes")
                })),
            exception.Error);
    }
}