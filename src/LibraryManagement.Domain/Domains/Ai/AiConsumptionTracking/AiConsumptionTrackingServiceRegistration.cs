using LibraryManagement.Domain.Domains.Ai.AiConsumptionTracking.CreateConsumption;

using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Domain.Domains.Ai.AiConsumptionTracking;

internal static class AiConsumptionTrackingServiceRegistration
{
    extension(IServiceCollection services)
    {
        internal IServiceCollection AddAiConsumptionTrackingServices()
        {
            // Register AI consumption tracking related services here

            return services.AddScoped<ICreateAiConsumptionUseCase, CreateAiConsumptionService>();
        }
    }
}
