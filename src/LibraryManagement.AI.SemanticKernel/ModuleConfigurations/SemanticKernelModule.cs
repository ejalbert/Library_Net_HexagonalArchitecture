using System.Collections.Concurrent;

using LibraryManagement.AI.SemanticKernel.Domain.Authors;
using LibraryManagement.AI.SemanticKernel.Domain.Books;
using LibraryManagement.AI.SemanticKernel.Domain.BookSuggestions;
using LibraryManagement.AI.SemanticKernel.LocalTools.Hub;
using LibraryManagement.AI.SemanticKernel.SemanticKernel;
using LibraryManagement.ModuleBootstrapper.AspNetCore.ModuleConfigurators;
using LibraryManagement.ModuleBootstrapper.ModuleRegistrators;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace LibraryManagement.AI.SemanticKernel.ModuleConfigurations;

public static class SemanticKernelModule
{
    extension<TApplicationBuilder>(IModuleRegistrator<TApplicationBuilder> moduleRegistrator)
        where TApplicationBuilder : IHostApplicationBuilder
    {
        public IModuleRegistrator<TApplicationBuilder> AddSemanticKernelModule(Action<SemanticKernelModuleOptions>? configureOptions = null)
        {
            var services = moduleRegistrator.Services;

            SemanticKernelModuleEnvConfiguration optionsFromEnv = new();
            moduleRegistrator.ConfigurationManager.GetSection("OpenAi").Bind(optionsFromEnv);

            moduleRegistrator.Services.AddOptions<SemanticKernelModuleOptions>().Configure(options =>
            {
                options.ApiKey = optionsFromEnv.ApiKey ?? string.Empty;
                options.Model = optionsFromEnv.Model ?? "gpt-4.1-nano";
            });

            if (configureOptions != null) moduleRegistrator.Services.Configure(configureOptions);

            services
                .AddBookSuggestionServices();

            services.AddScoped<OpenAIChatCompletionService>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<SemanticKernelModuleOptions>>().Value;
                return new OpenAIChatCompletionService(options.Model, options.ApiKey);
            })
            .AddScoped<ITokenAwareChatCompletionService, TokenAwareChatCompletionService>()
            .AddScoped<IChatCompletionService>(sp => sp.GetRequiredService<ITokenAwareChatCompletionService>())
            .AddAuthorServices()
            .AddBookServices()
            .AddBookSuggestionServices().AddSignalR();


            return moduleRegistrator;
        }
    }

    extension(IModuleConfigurator moduleConfigurator)
    {
        public IModuleConfigurator UseSemanticKernelModule()
        {
            moduleConfigurator.App.MapHub<ToolHub>("api/v1/ai/tools/local");

            return moduleConfigurator;
        }
    }
}

public interface ILocalToolClient : IDisposable
{
    bool IsConnected { get; }
    string ConnectionId { get; }

    Task<TResult> SendAsync<TResult>(string methodName, CancellationToken cancellationToken = default);
    Task<TResult> SendAsync<TResult>(string methodName, object? arg2, CancellationToken cancellationToken = default);
    Task<TResult> SendAsync<TResult>(string methodName, object? arg2, object? arg3, CancellationToken cancellationToken = default);
}


public class LocalToolClient : ILocalToolClient
{
    private readonly ConcurrentDictionary<string, TaskCompletionSource<object>> _pending =
        new();

    private readonly ToolHub _hub;
    private readonly HttpContextAccessor _httpContextAccessor;

    private bool _disposed = false;

    public string ConnectionId => _httpContextAccessor.HttpContext!.Request.Headers[AddConnectionIdRequestHandler.ConnectionIdHeaderName]!;
    public bool IsConnected => _hub.IsConnected(ConnectionId);

    public LocalToolClient(HttpContextAccessor httpContextAccessor, ToolHub hub)
    {
        _hub = hub;
        _httpContextAccessor = httpContextAccessor;

        _hub.ToolCallResolved += OnToolCallResolved;
    }

    private async Task<TResult> SendRequestAsync<TResult>(
        Func<ToolHub, string, string, CancellationToken, Task> clientCall,
        CancellationToken cancellationToken = default)
    {
        var connectionId = ConnectionId;
        var correlationId = Guid.NewGuid().ToString();

        var tcs = new TaskCompletionSource<object>(
            TaskCreationOptions.RunContinuationsAsynchronously);


        if (!_pending.TryAdd(ConnectionId, tcs))
        {
            throw new InvalidOperationException("Duplicate correlation ID.");
        }

        using var registration = cancellationToken.Register(() =>
        {
            if (_pending.TryRemove(correlationId, out var pendingTcs))
            {
                pendingTcs.TrySetCanceled(cancellationToken);
            }
        });

        await clientCall(_hub, correlationId, connectionId, cancellationToken);



        return  (TResult)await tcs.Task; // Wait until client replies
    }

    public Task<TResult> SendAsync<TResult>(string methodName, CancellationToken cancellationToken = default)
    {
        return SendRequestAsync<TResult>((hub, corellationId, connectionId, token) =>
        {
            return hub.Clients.Client(connectionId)
                .SendAsync(methodName, corellationId, token);
        }, cancellationToken);
    }

    public Task<TResult> SendAsync<TResult>(string methodName, object? arg2, CancellationToken cancellationToken = default)
    {
        return SendRequestAsync<TResult>((hub, corellationId, connectionId, token) =>
        {
            return hub.Clients.Client(connectionId)
                .SendAsync(methodName, corellationId, arg2, token);
        }, cancellationToken);
    }

    public Task<TResult> SendAsync<TResult>(string methodName, object? arg2, object? arg3, CancellationToken cancellationToken = default)
    {
        return SendRequestAsync<TResult>((hub, corellationId, connectionId, token) =>
        {
            return hub.Clients.Client(connectionId)
                .SendAsync(methodName, corellationId, arg2, arg3, token);
        }, cancellationToken);
    }

    private void OnToolCallResolved(object? sender, ToolHub.ToolCallResolvedEventArgs e)
    {
        if (_pending.TryRemove(e.CorrelationId, out var tcs))
        {
            tcs.SetResult(e.Result);
        }
    }


    private void Dispose(bool disposing)
    {
        if (disposing && !_disposed)
        {
            _hub.ToolCallResolved -= OnToolCallResolved;

            _disposed = true;
        }
    }
    public void Dispose()
    {
        Dispose(true);
    }
}


public class ToolHub(IHttpContextAccessor httpContext) : Hub
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


    public async Task LocalToolResponse(string corellationId, string toolName, object result)
    {
        ToolCallResolved?.Invoke(this, new ToolCallResolvedEventArgs(corellationId, result));
        await Task.CompletedTask;
    }

    public delegate void ToolCallResolvedEventHandler(object? sender, ToolCallResolvedEventArgs e);
    public class ToolCallResolvedEventArgs(string correlationId, object result) : EventArgs
    {
        public string CorrelationId { get; } = correlationId;
        public object Result { get; } = result;
    }


}
