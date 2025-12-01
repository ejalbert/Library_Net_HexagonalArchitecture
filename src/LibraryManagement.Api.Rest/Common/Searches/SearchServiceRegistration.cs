using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Api.Rest.Common.Searches;

internal static class SearchServiceRegistration
{
    internal static IServiceCollection AddSearchService(this IServiceCollection services)
    {
        return services.AddSingleton<ISearchDtoMapper, SearchDtoMapper>();
    }
}
