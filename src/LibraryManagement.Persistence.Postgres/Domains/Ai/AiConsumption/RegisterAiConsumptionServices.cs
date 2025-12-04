using LibraryManagement.Domain.Domains.Ai.AiConsumptionTracking.CreateConsumption;
using LibraryManagement.Persistence.Postgres.Domains.Ai.AiConsumption.Adapters;

using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Persistence.Postgres.Domains.Ai.AiConsumption;

internal static class RegisterAiConsumptionServices
{
    extension(IServiceCollection services)
    {
        internal IServiceCollection AddAiConsumptionServices()
        {
            return services.AddScoped<ICreateAiConsumptionPort, CreateAiConsumptionAdapter>();
        }
    }
}
