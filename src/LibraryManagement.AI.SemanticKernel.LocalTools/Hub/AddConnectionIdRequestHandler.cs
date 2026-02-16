using LibraryManagement.Api.Rest.Client;

using Microsoft.AspNetCore.SignalR.Client;

namespace LibraryManagement.AI.SemanticKernel.LocalTools.Hub;

public class AddConnectionIdRequestHandler(HubConnection connection)
    : DelegatingHandlerBase, IAddConnectionIdRequestHandler
{
    public const string ConnectionIdHeaderName = "X-LocalTools-ConnectionId";

    protected override Task<HttpRequestMessage> HandleRequest(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(connection.ConnectionId))
            request.Headers.Add(ConnectionIdHeaderName, connection.ConnectionId);

        return Task.FromResult(request);
    }
}
