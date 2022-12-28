using LogSnag.Tests.Unit.Mocking;

namespace LogSnag.Tests.Unit;

public sealed class LogSnagTests
{
    [Fact]
    public async Task OkResponse_CorrectRequest()
    {
        var mockHttpClient = MockHttpClient.Ok;

        var logSnag = new LogSnag(mockHttpClient, "test-api-token");

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

        Assert.Equal(HttpMethod.Post, mockHttpClient.LastRequest.Method);
        Assert.Equal("Bearer", mockHttpClient.LastRequest.Headers.Authorization!.Scheme);
        Assert.Equal("test-api-token", mockHttpClient.LastRequest.Headers.Authorization!.Parameter);
    }
}