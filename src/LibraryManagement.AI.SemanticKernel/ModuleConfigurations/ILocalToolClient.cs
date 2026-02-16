namespace LibraryManagement.AI.SemanticKernel.ModuleConfigurations;

public interface ILocalToolClient : IDisposable
{
    bool IsConnected { get; }
    string ConnectionId { get; }

    Task<TResult> SendAsync<TResult>(string methodName, CancellationToken cancellationToken = default);
    Task<TResult> SendAsync<TResult>(string methodName, object? arg2, CancellationToken cancellationToken = default);

    Task<TResult> SendAsync<TResult>(string methodName, object? arg2, object? arg3,
        CancellationToken cancellationToken = default);

    Task<TResult> SendAsync<TResult>(string methodName, object? arg2, object? arg3, object? arg4,
        CancellationToken cancellationToken = default);
}
