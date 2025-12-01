using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Persistence.Postgres.Domains.Authors;

internal static class RegisterAuthorServices
{
    internal static IServiceCollection AddAuthorServices(this IServiceCollection services)
    {
        return services.AddSingleton<IAuthorEntityMapper, AuthorEntityMapper>();
    }

}
