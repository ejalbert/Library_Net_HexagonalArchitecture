using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LibraryManagement.AI.SemanticKernel.LocalTools.Tools.Authors;

internal static class AuthorToolRegistration
{
    extension(IServiceCollection services)
    {
        internal IServiceCollection AddAuthorTools()
        {
            services.AddSingleton<ISearchAuthorTool, SearchAuthorTool>();
            return services;
        }
    }

    extension(IHost host)
    {
        internal IHost UseAuthorTools()
        {
            host.Services.UseLocalTool<ISearchAuthorTool>();

            return host;
        }
    }
}
