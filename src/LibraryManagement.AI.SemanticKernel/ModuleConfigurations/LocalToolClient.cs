using System.Collections.Concurrent;
using System.Text.Json;

using LibraryManagement.AI.SemanticKernel.LocalTools.Hub;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace LibraryManagement.AI.SemanticKernel.ModuleConfigurations;

public class LocalToolClient : ILocalToolClient
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IToolHub _hub;

    private readonly ConcurrentDictionary<string, TaskCompletionSource<JsonElement>> _pending =
        new();

    private bool _disposed;

    public LocalToolClient(IHttpContextAccessor httpContextAccessor, IToolHub hub)
    {
        _hub = hub;
        _httpContextAccessor = httpContextAccessor;

        _hub.ToolCallResolved += OnToolCallResolved;
    }

    public string ConnectionId =>
        _httpContextAccessor.HttpContext!.Request.Headers[AddConnectionIdRequestHandler.ConnectionIdHeaderName]!;

    public bool IsConnected => _hub.IsConnected(ConnectionId);

    public Task<TResult> SendAsync<TResult>(string methodName, CancellationToken cancellationToken = default)
    {
        return SendRequestAsync<TResult>(
            (client, corellationId, token) => { return client.SendAsync(methodName, corellationId, token); },
            cancellationToken);
    }

    public Task<TResult> SendAsync<TResult>(string methodName, object? arg2,
        CancellationToken cancellationToken = default)
    {
        return SendRequestAsync<TResult>(
            (client, corellationId, token) => { return client.SendAsync(methodName, corellationId, arg2, token); },
            cancellationToken);
    }

    public Task<TResult> SendAsync<TResult>(string methodName, object? arg2, object? arg3,
        CancellationToken cancellationToken = default)
    {
        return SendRequestAsync<TResult>(
            (client, corellationId, token) =>
            {
                return client.SendAsync(methodName, corellationId, arg2, arg3, token);
            }, cancellationToken);
    }

    public Task<TResult> SendAsync<TResult>(string methodName, object? arg2, object? arg3, object? arg4,
        CancellationToken cancellationToken = default)
    {
        return SendRequestAsync<TResult>(
            (client, corellationId, token) =>
            {
                return client.SendAsync(methodName, corellationId, arg2, arg3, arg4, token);
            }, cancellationToken);
    }

    public void Dispose()
    {
        Dispose(true);
    }

    private async Task<TResult> SendRequestAsync<TResult>(
        Func<ISingleClientProxy, string, CancellationToken, Task> clientCall,
        CancellationToken cancellationToken = default)
    {
        var connectionId = ConnectionId;
        var correlationId = Guid.NewGuid().ToString();

        var tcs = new TaskCompletionSource<JsonElement>(
            TaskCreationOptions.RunContinuationsAsynchronously);


        if (!_pending.TryAdd(correlationId, tcs)) throw new InvalidOperationException("Duplicate correlation ID.");

        using CancellationTokenRegistration registration = cancellationToken.Register(() =>
        {
            if (_pending.TryRemove(correlationId, out TaskCompletionSource<JsonElement>? pendingTcs))
                pendingTcs.TrySetCanceled(cancellationToken);
        });

        await clientCall(_hub.GetClient(connectionId), correlationId, cancellationToken);


        return (await tcs.Task).Deserialize<TResult>()!; // Wait until client replies
    }

    private void OnToolCallResolved(object? sender, ToolHub.ToolCallResolvedEventArgs e)
    {
        if (_pending.TryRemove(e.CorrelationId, out TaskCompletionSource<JsonElement>? tcs)) tcs.SetResult(e.Result);
    }


    private void Dispose(bool disposing)
    {
        if (disposing && !_disposed)
        {
            _hub.ToolCallResolved -= OnToolCallResolved;

            _disposed = true;
        }
    }
}
