using LibraryManagement.AI.OpenAi.Domain.BookSuggestion.Adapters;
using LibraryManagement.Domain.Domains.BookSuggestions.Create;

using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.AI.OpenAi.Domain.BookSuggestion;

internal static class BookSuggestionServiceRegistration
{
    extension(IServiceCollection services)
    {
        internal IServiceCollection AddBookSuggestionServices()
        {

            return services.AddScoped<ICreateBookSuggestionPort, CreateBookSuggestionAdapter>();
        }
    }
}
