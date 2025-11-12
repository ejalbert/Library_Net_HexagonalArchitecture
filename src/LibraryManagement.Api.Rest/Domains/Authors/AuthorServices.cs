using LibraryManagement.Api.Rest.Client.Domain.Authors;
using LibraryManagement.Api.Rest.Client.Domain.Authors.Create;
using LibraryManagement.Api.Rest.Domains.Authors.CreateAuthor;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Api.Rest.Domains.Authors;

internal static class AuthorServices
{
    internal static IServiceCollection AddAuthorServices(this IServiceCollection services)
    {
        services
            .AddScoped<IAuthorDtoMapper, AuthorDtoMapper>()
            .AddScoped<ICreateAuthorController, CreateAuthorController>();

        return services;
    }

    internal static WebApplication UseAuthorServices(this WebApplication app)
    {
        var group = app.MapGroup("/api").MapGroup("/v1/authors").WithGroupName("Authors");

        group.MapPost("", ([FromBody] CreateAuthorRequestDto request, ICreateAuthorController controller) => controller.CreateAuthor(request))
            .WithName("Create Author")
            .WithDescription("Creates a new author entry")
            .Produces<AuthorDto>();

        return app;
    }
}
