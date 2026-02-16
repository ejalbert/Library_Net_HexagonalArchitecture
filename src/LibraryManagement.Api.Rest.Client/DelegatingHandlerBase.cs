namespace LibraryManagement.Api.Rest.Client;

public abstract class DelegatingHandlerBase : IDelegatingHandler
{
    public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken,
        Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> next)
    {
        return await HandleResponse(await next(await HandleRequest(request, cancellationToken), cancellationToken),
            cancellationToken);
    }

    protected virtual Task<HttpRequestMessage> HandleRequest(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(request);
    }

    protected virtual Task<HttpResponseMessage> HandleResponse(HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(response);
    }
}
