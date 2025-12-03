using LibraryManagement.AI.OpenAi.Domain.Books.Tools;

using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.AI.OpenAi.Domain.Books;

internal static class BookServiceRegistration
{
    extension(IServiceCollection services)
    {
        internal IServiceCollection AddBookServices()
        {
            return services.AddScoped<ISearchBooksChatTool, SearchBooksChatTool>();
        }
    }
}
