using LibraryManagement.Domain.Domains.BookSuggestions.Create;

using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Domain.Domains.BookSuggestions;

internal static class BookSuggestionServiceRegistration
{
    extension(IServiceCollection services)
    {
        internal IServiceCollection AddBookSuggestionServices()
        {
            return services.AddScoped<ICreateBookSuggestionUseCase, CreateBookSuggestionService>();
        }
    }
}
