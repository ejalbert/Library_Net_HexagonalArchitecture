using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.AI.SemanticKernel.LocalTools.Tools;

internal static class LocalToolsHubConnectionExtensions
{
    private static readonly Dictionary<HubConnection, IList<ILocalTool>> HubToolsMap = new();

    extension(HubConnection connection)
    {
        internal IList<ILocalTool> LocalTools
        {
            get
            {
                if (HubToolsMap.ContainsKey(connection)) return HubToolsMap[connection];

                var tools = new List<ILocalTool>();
                HubToolsMap[connection] = tools;
                return tools;
            }
        }

        internal HubConnection UseTool<TTool>(IServiceProvider sp) where TTool : ILocalTool
        {
            connection.LocalTools.Add(sp.GetRequiredService<TTool>());

            return connection;
        }

        internal Task RegisterLocalToolsAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<Task> tasks = connection.LocalTools.Select(tool => tool.RegisterAsync(cancellationToken));
            return Task.WhenAll(tasks);
        }

        internal Task UnregisterLocalToolsAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<Task> tasks = connection.LocalTools.Select(tool => tool.UnregisterAsync(cancellationToken));
            return Task.WhenAll(tasks);
        }
    }

    extension(IServiceProvider sp)
    {
        internal IServiceProvider UseLocalTool<TTool>() where TTool : ILocalTool
        {
            sp.GetRequiredService<HubConnection>().LocalTools.Add(sp.GetRequiredService<TTool>());

            return sp;
        }
    }
}
