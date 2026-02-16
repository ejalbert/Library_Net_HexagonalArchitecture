using LibraryManagement.AI.SemanticKernel.LocalTools.Tools;

using Microsoft.AspNetCore.SignalR.Client;

namespace LibraryManagement.AI.SemanticKernel.LocalTools.Hub;

public interface ILocalToolHub : IAsyncDisposable
{
    Task ConnectAsync(CancellationToken cancellationToken = default);
    Task DisconnectAsync(CancellationToken cancellationToken = default);
}

public class LocalToolHub(HubConnection connection) : ILocalToolHub
{
    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        await connection.RegisterLocalToolsAsync(cancellationToken);
        await connection.StartAsync(cancellationToken);
    }

    public async Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        await connection.UnregisterLocalToolsAsync(cancellationToken);
        await connection.StopAsync(cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        if (connection.State != HubConnectionState.Disconnected) await DisconnectAsync();
    }
}
