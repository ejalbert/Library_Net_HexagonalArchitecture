using LibraryManagement.Api.Rest.Infrastructure.Tenants.Adapters;
using LibraryManagement.Domain.Infrastructure.Tenants.GetCurrentUserTenantId;

using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Api.Rest.Infrastructure.Tenants;

internal static class TenantServiceRegistration
{
    internal static IServiceCollection AddTenantServices(this IServiceCollection services)
    {
        return services
            .AddHttpContextAccessor()
            .AddScoped<IGetCurrentUserTenantIdPort, GetCurrentUserTenantIdAdapter>();
    }

}
