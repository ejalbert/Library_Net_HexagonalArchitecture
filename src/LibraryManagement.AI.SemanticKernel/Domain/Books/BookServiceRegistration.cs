using LibraryManagement.AI.SemanticKernel.Domain.Books.Plugins;

using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.AI.SemanticKernel.Domain.Books;

internal static class BookServiceRegistration
{
    extension(IServiceCollection services)
    {
        internal IServiceCollection AddBookServices()
        {
            return services.AddScoped<ISearchBooksPlugin, SearchBooksPlugin>();
        }
    }

}
