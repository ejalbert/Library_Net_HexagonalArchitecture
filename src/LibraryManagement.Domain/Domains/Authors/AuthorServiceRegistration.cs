using LibraryManagement.Domain.Domains.Authors.Create;

using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Domain.Domains.Authors;

internal static class AuthorServiceRegistration
{
    internal static IServiceCollection AddAuthorServices(this IServiceCollection services)
    {
        services.AddScoped<ICreateAuthorUseCase, CreateAuthorService>();
        return services;
    }
}
