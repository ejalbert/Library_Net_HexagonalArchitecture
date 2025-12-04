using LibraryManagement.Domain.Domains.Ai.AiConsumptionTracking;
using LibraryManagement.Domain.Domains.Ai.BookSuggestions;

using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Domain.Domains.Ai;

internal static class AiServiceRegistration
{
    extension(IServiceCollection services)
    {
        internal IServiceCollection AddAiServices()
        {
            return services
                .AddAiConsumptionTrackingServices()
                .AddBookSuggestionServices();
        }
    }
}
