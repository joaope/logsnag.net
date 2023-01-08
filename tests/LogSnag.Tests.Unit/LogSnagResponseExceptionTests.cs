using System.Net;

namespace LogSnag.Tests.Unit;

public sealed class LogSnagResponseExceptionTests
{
    [Fact]
    public void EmptyBody()
    {
        var exception = new LogSnagResponseException(
            "error message", 
            null, 
            new HttpResponseMessage(HttpStatusCode.BadGateway));

        Assert.Null(exception.Error);
        Assert.Equal("error message", exception.Message);
        Assert.Equal(HttpStatusCode.BadGateway, exception.StatusCode);
    }

    [Fact]
    public void WithBody()
    {
        const string responseContent = @"
{
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
}";
        
        var exception = new LogSnagResponseException(
            "error message",
            responseContent,
            new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(responseContent)
            });

        Assert.Equal("error message", exception.Message);
        Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
        Assert.Equivalent(new LogSnagErrorResponse(
                "Validation Error",
                new LogSnagErrorResponse.Validations(
                    null,
                    new[]
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

    [Fact]
    public void WithBodyAndHeaders()
    {
        const string responseContent = @"
{
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
        ],
        ""headers"": [
            {
                ""path"": ""authorization"",
                ""type"": ""invalid_type"",
                ""message"": ""Authorization header must be a valid Bearer token""
            }
        ]
    }
}";

        var exception = new LogSnagResponseException(
            "error message",
            responseContent,
            new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(responseContent)
            });

        Assert.Equal("error message", exception.Message);
        Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
        Assert.Equivalent(new LogSnagErrorResponse(
                "Validation Error",
                new LogSnagErrorResponse.Validations(
                    new []
                    {
                        new LogSnagErrorResponse.Validations.Item(
                            "authorization",
                            "invalid_type",
                            "Authorization header must be a valid Bearer token")
                    },
                    new[]
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