using Microsoft.AspNetCore.SignalR.Client;

namespace LibraryManagement.AI.SemanticKernel.LocalTools.Tools;

public abstract class LocalToolBase(HubConnection connection) : ILocalTool
{
    private bool _isDisposed;
    public HubConnection Connection => connection;

    public void Dispose()
    {
        Dispose(true);
    }

    public async ValueTask DisposeAsync()
    {
        if (!_isDisposed)
        {
            await UnregisterAsync();
            await DisposeAsync(true);
        }
    }


    public abstract Task RegisterAsync(CancellationToken cancellationToken = default);
    public abstract Task UnregisterAsync(CancellationToken cancellationToken = default);

    protected Task SendToolResponseAsync<TResponse>(string corellationId, string toolName, TResponse response)
    {
        return Connection.SendAsync("LocalToolResponse", corellationId, toolName, response);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing && !_isDisposed) _isDisposed = true;
    }

    protected virtual ValueTask DisposeAsync(bool disposing)
    {
        if (disposing && !_isDisposed) _isDisposed = true;
        return ValueTask.CompletedTask;
    }
}
