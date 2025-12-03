using LibraryManagement.AI.OpenAi.Domain.Authors.Tools;

using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.AI.OpenAi.Domain.Authors;

internal static class AuthorServiceRegistration
{
    extension(IServiceCollection services)
    {
        internal IServiceCollection AddAuthorServices()
        {
            return services.AddScoped<ISearchAuthorsChatTool, SearchAuthorsChatTools>();
        }
    }
}
