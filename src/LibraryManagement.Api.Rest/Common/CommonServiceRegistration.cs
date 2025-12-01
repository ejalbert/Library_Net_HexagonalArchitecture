using LibraryManagement.Api.Rest.Common.Searches;

using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Api.Rest.Common;

internal static class CommonServiceRegistration
{
    internal static IServiceCollection AddCommonServices(this IServiceCollection services)
    {
        return services
            .AddSearchService();
    }
}
