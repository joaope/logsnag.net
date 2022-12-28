using System.Net;
using LogSnag.Tests.Unit.Mocking;

namespace LogSnag.Tests.Unit;

public sealed class LogSnagTests
{
    [Fact]
    public async Task OkResponse_CorrectRequest()
    {
        var httpClient = MockHttpClient.Ok;
        var logSnag = new LogSnag(httpClient, "test-api-token");

        await logSnag.Publish(new LogSnagEvent("logsnag-net", "test-channel", "AnEvent")
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
            @"{""project"":""logsnag-net"",""channel"":""test-channel"",""event"":""AnEvent"",""notify"":true,""tags"":{""key-one"":1,""key-two"":""val2"",""bool"":true},""parser"":""text""}",
            await httpClient.LastRequest.Content!.ReadAsStringAsync());
    }

    [Fact]
    public async Task NotOkResponse_ThrowsResponseException()
    {
        var response = new HttpResponseMessage(HttpStatusCode.BadGateway);
        var httpClient = new MockHttpClient(response);
        var logSnag = new LogSnag(httpClient, "test-api-token");

        var exception = await Assert.ThrowsAsync<LogSnagResponseException>(async () =>
        {
            await logSnag.Publish(new LogSnagEvent("logsnag-net", "test-channel", "AnEvent")
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

        Assert.Same(response, exception.Response);
        Assert.Equal("Not successful response while publishing event to LogSnag.", exception.Message);
    }
}