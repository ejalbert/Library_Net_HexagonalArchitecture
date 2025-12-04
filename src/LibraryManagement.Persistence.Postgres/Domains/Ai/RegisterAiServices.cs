using LibraryManagement.Persistence.Postgres.Domains.Ai.AiConsumption;

using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Persistence.Postgres.Domains.Ai;

internal static class RegisterAiServices
{
    extension(IServiceCollection services)
    {
        internal IServiceCollection AddAiServices()
        {
            // Register AI related persistence services here

            return services.AddAiConsumptionServices();
        }
    }
}
