using LibraryManagement.Api.Rest.Domains.Ai.BookSuggestions;

using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Api.Rest.Domains.Ai;

internal static class AiServiceRegistration
{
    extension(IServiceCollection services)
    {
        internal IServiceCollection AddAiServices()
        {


            return services.AddBookSuggestionServices();
        }
    }
}
