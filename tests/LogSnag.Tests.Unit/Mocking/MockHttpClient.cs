namespace LogSnag.Tests.Unit.Mocking
{
    public sealed class MockHttpClient : HttpClient
    {
        public MockHttpMessageHandler Handler { get; }
        public HttpRequestMessage LastRequest => Handler.LastRequest;
        public List<HttpRequestMessage> Requests => Handler.Requests;

        public MockHttpClient(MockHttpMessageHandler handler, Uri baseAddress)
            : base(handler)
        {
            Handler = handler;
            BaseAddress = baseAddress;
        }

        public MockHttpClient(MockHttpMessageHandler handler)
            : this(handler, new Uri("https://api.logsnag.com/"))
        {
        }

        public MockHttpClient(HttpResponseMessage response)
            : this(new MockHttpMessageHandler(response))
        {
        }

        public static MockHttpClient BadRequest => new(MockHttpMessageHandler.BadRequest);

        public static MockHttpClient Ok => new(MockHttpMessageHandler.Ok);

        public static MockHttpClient NotFound => new(MockHttpMessageHandler.NotFound);
    }
}
