using LibraryManagement.AI.SemanticKernel.Domain.Authors.Plugins;

using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.AI.SemanticKernel.Domain.Authors;

internal static class ConfigureAuthorServices
{
    extension(IServiceCollection services)
    {
        internal IServiceCollection AddAuthorServices()
        {
            return services.AddScoped<ISearchAuthorPlugin, SearchAuthorsPlugin>();
        }
    }

}
