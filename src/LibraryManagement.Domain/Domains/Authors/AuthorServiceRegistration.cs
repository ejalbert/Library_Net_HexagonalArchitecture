using LibraryManagement.Domain.Domains.Authors.Create;
using LibraryManagement.Domain.Domains.Authors.Search;

using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Domain.Domains.Authors;

internal static class AuthorServiceRegistration
{
    internal static IServiceCollection AddAuthorServices(this IServiceCollection services)
    {
        return services
            .AddScoped<ICreateAuthorUseCase, CreateAuthorService>()
            .AddScoped<ISearchAuthorsUseCase, SearchAuthorsService>();
    }
}
