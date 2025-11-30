using LibraryManagement.Domain.Infrastructure.Tenants.GetCurrentUserTenantId;

using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Domain.Infrastructure.Tenants;

internal static class TenantServiceRegistration
{
    internal static IServiceCollection AddTenantServices(this IServiceCollection services)
    {
        return services
            .AddScoped<IGetCurrentUserTenantIdUseCase, GetCurrentUserTenantIdService>();
    }
}
