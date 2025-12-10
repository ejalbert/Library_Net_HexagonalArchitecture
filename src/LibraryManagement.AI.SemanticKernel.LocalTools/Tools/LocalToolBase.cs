using Microsoft.AspNetCore.SignalR.Client;

namespace LibraryManagement.AI.SemanticKernel.LocalTools.Tools;

public abstract class LocalToolBase(HubConnection connection) : ILocalTool
{
    public HubConnection Connection => connection;

    private bool _isDisposed;

    public void Dispose()
    {
        Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
        if(disposing && !_isDisposed)
        {
            _isDisposed = true;
        }
    }

    protected virtual ValueTask DisposeAsync(bool disposing)
    {
        if(disposing && !_isDisposed)
        {
            _isDisposed = true;
        }
        return ValueTask.CompletedTask;
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
}
