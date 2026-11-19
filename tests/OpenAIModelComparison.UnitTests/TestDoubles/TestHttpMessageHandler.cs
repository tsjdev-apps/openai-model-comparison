using System.Net;

namespace OpenAIModelComparison.UnitTests.TestDoubles;

/// <summary>
/// Supplies deterministic HTTP responses for service tests.
/// </summary>
/// <param name="responseFactory">The factory used to create responses.</param>
internal sealed class TestHttpMessageHandler(
    Func<HttpRequestMessage, HttpResponseMessage> responseFactory) : HttpMessageHandler
{
    /// <summary>
    /// Gets the most recent request sent through this handler.
    /// </summary>
    public HttpRequestMessage? LastRequest { get; private set; }

    /// <summary>
    /// Processes an HTTP request with the configured response factory.
    /// </summary>
    /// <param name="request">The request to process.</param>
    /// <param name="cancellationToken">The token used to observe cancellation.</param>
    /// <returns>The generated HTTP response.</returns>
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        LastRequest = request;
        return Task.FromResult(responseFactory(request));
    }

    /// <summary>
    /// Creates a handler that returns a successful JSON response.
    /// </summary>
    /// <param name="json">The JSON response body.</param>
    /// <returns>The configured message handler.</returns>
    public static TestHttpMessageHandler ReturningJson(string json) =>
        new(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
        });
}
