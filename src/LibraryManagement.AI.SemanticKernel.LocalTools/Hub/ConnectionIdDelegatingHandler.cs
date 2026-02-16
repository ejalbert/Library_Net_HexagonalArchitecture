namespace LibraryManagement.AI.SemanticKernel.LocalTools.Hub;

public class ConnectionIdDelegatingHandler(IAddConnectionIdRequestHandler addConnectionIdRequestHandler)
    : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        return addConnectionIdRequestHandler.SendAsync(request, cancellationToken, base.SendAsync);
    }
}
