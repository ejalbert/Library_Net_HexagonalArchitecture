using Microsoft.AspNetCore.SignalR.Client;

namespace LibraryManagement.AI.SemanticKernel.LocalTools.Tools;

public interface ILocalTool : IDisposable, IAsyncDisposable
{
    HubConnection Connection { get;  }

    Task RegisterAsync(CancellationToken cancellationToken = default);
    Task UnregisterAsync(CancellationToken cancellationToken = default);
}
