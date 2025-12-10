using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LibraryManagement.AI.SemanticKernel.LocalTools.Tools.ReadDirectory;

internal static class ReadDirectoryToolRegistration
{
    extension(IServiceCollection services)
    {
        internal IServiceCollection AddReadDirectoryTool()
        {
            services.AddSingleton<IReadDirectoryTool, ReadDirectoryTool>();
            return services;
        }
    }

    extension(IHost host)
    {
        internal IHost UseReadDirectoryTool()
        {
            host.Services.UseLocalTool<IReadDirectoryTool>();

            return host;
        }
    }
}
