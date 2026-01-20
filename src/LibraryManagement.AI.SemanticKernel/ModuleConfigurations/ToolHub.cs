using System.Collections.Concurrent;
using System.Text.Json;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace LibraryManagement.AI.SemanticKernel.ModuleConfigurations;

public interface IToolHub
{
    bool IsConnected(string connectionId);
    ISingleClientProxy GetClient(string connectionId);

    event ToolHub.ToolCallResolvedEventHandler ToolCallResolved;
}

public class ToolHub : Hub, IToolHub
{
    private ConcurrentDictionary<string, bool> _connectedClients =
        new();



    public event ToolCallResolvedEventHandler ToolCallResolved;

    public override Task OnConnectedAsync()
    {
        _connectedClients.TryAdd(Context.ConnectionId, true);
        return base.OnConnectedAsync();
    }
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _connectedClients.TryRemove(Context.ConnectionId, out _);
        return base.OnDisconnectedAsync(exception);
    }

    public bool IsConnected(string connectionId) => _connectedClients.ContainsKey(connectionId);

    public ISingleClientProxy GetClient(string connectionId)
    {
        return Clients.Client(connectionId);
    }


    public async Task LocalToolResponse(string corellationId, string toolName, JsonElement result)
    {
        ToolCallResolved?.Invoke(this, new ToolCallResolvedEventArgs(corellationId, result));
        await Task.CompletedTask;
    }

    public delegate void ToolCallResolvedEventHandler(object? sender, ToolCallResolvedEventArgs e);
    public class ToolCallResolvedEventArgs(string correlationId, JsonElement result) : EventArgs
    {
        public string CorrelationId { get; } = correlationId;
        public JsonElement Result { get; } = result;
    }
}
