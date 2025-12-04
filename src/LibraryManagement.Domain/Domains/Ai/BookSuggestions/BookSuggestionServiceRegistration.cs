using LibraryManagement.Domain.Domains.Ai.BookSuggestions.Create;

using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Domain.Domains.Ai.BookSuggestions;

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
