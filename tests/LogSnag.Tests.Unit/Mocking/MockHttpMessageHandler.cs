using System.Net;

namespace LogSnag.Tests.Unit.Mocking
{
    public sealed class MockHttpMessageHandler : HttpMessageHandler
    {
        public HttpRequestMessage LastRequest => Requests.Last();
        public List<HttpRequestMessage> Requests { get; } = new();

        private readonly HttpResponseMessage _response;

        public MockHttpMessageHandler(HttpStatusCode responseStatusCode, string content = "content", string mediaType = "application/json")
        {
            _response = new HttpResponseMessage(responseStatusCode)
            {
                Content = new StringContent(content, null, mediaType)
            };
        }

        public MockHttpMessageHandler(HttpResponseMessage response)
        {
            _response = response;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Requests.Add(request);

            return Task.FromResult(_response);
        }

        public static MockHttpMessageHandler BadRequest => new(HttpStatusCode.BadRequest);

        public static MockHttpMessageHandler Ok => new(HttpStatusCode.OK);

        public static MockHttpMessageHandler NotFound => new(HttpStatusCode.NotFound);
    }
}
