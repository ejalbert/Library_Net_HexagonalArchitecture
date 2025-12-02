using LibraryManagement.Domain.Domains.Authors.Create;
using LibraryManagement.Domain.Domains.Authors.Search;
using LibraryManagement.Persistence.Mongo.Domains.Authors.Adapters;

using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Persistence.Mongo.Domains.Authors;

internal static class RegisterAuthorServices
{
    internal static IServiceCollection AddAuthorServices(this IServiceCollection services)
    {
        return services
            .AddScoped<IAuthorCollection, AuthorCollection>()
            .AddScoped<IAuthorEntityMapper, AuthorEntityMapper>()
            .AddScoped<ICreateAuthorPort, CreateAuthorAdapter>()
            .AddScoped<ISearchAuthorsPort, SearchAuthorsAdapter>();
    }
}
