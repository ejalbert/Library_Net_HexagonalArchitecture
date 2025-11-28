namespace LibraryManagement.Api.Rest.Client.Tests.Http;

internal sealed class TestHttpMessageHandler(
    Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handler) : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        return handler(request, cancellationToken);
    }
}
