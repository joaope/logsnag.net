using System.Net;
using LogSnag.Tests.Unit.Mocking;

namespace LogSnag.Tests.Unit;

public sealed partial class LogSnagHttpClientTests
{
    [Fact]
    public async Task Insight_OkResponse_CorrectRequest()
    {
        var httpClient = MockHttpClient.Ok;
        var logSnag = new LogSnagHttpClient(httpClient, "test-api-token");

        await logSnag.Insight(new LogSnagInsight("logsnag-net", "some title", "some value")
        {
            Icon = "icon"
        });

        Assert.Single(httpClient.Requests);
        Assert.Equal(HttpMethod.Post, httpClient.LastRequest.Method);
        Assert.Single(httpClient.LastRequest.Headers);
        Assert.Equal("Bearer", httpClient.LastRequest.Headers.Authorization!.Scheme);
        Assert.Equal("test-api-token", httpClient.LastRequest.Headers.Authorization!.Parameter);
        Assert.Single(httpClient.LastRequest.Content!.Headers);
        Assert.Equal("application/json", httpClient.LastRequest.Content!.Headers.ContentType!.MediaType);
        Assert.Equal("utf-8", httpClient.LastRequest.Content!.Headers.ContentType!.CharSet);
        Assert.Equal("https://api.logsnag.com/v1/insight", httpClient.LastRequest.RequestUri!.ToString());
        Assert.Equal(
            @"{""project"":""logsnag-net"",""title"":""some title"",""value"":""some value"",""icon"":""icon""}",
            await httpClient.LastRequest.Content!.ReadAsStringAsync());
    }

    [Fact]
    public async Task Insight_NotOkResponse_ThrowsResponseException()
    {
        var response = new HttpResponseMessage(HttpStatusCode.BadGateway);
        var httpClient = new MockHttpClient(response);
        var logSnag = new LogSnagHttpClient(httpClient, "test-api-token");

        var exception = await Assert.ThrowsAsync<LogSnagResponseException>(async () =>
        {
            await logSnag.Insight(new LogSnagInsight("logsnag-net", "some title", "and value"));
        });

        Assert.Same(response, exception.Response);
        Assert.Equal("Not successful response while publishing insight to LogSnag.", exception.Message);
    }
}