using LibraryManagement.Api.Rest.Client.Domain.Authors;
using LibraryManagement.Api.Rest.Client.Domain.Authors.Create;
using LibraryManagement.Api.Rest.Client.Domain.Authors.Search;
using LibraryManagement.Api.Rest.Domains.Authors.CreateAuthor;
using LibraryManagement.Api.Rest.Domains.Authors.Search;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Api.Rest.Domains.Authors;

internal static class AuthorServices
{
    internal static IServiceCollection AddAuthorServices(this IServiceCollection services)
    {
        services
            .AddScoped<IAuthorDtoMapper, AuthorDtoMapper>()
            .AddScoped<ICreateAuthorController, CreateAuthorController>()
            .AddScoped<ISearchAuthorsController, SearchAuthorsController>();

        return services;
    }

    internal static WebApplication UseAuthorServices(this WebApplication app)
    {
        RouteGroupBuilder group = app.MapGroup("/api").MapGroup("/v1/authors").WithGroupName("Authors");

        group.MapPost("",
                ([FromBody] CreateAuthorRequestDto request, ICreateAuthorController controller) =>
                    controller.CreateAuthor(request))
            .WithName("Create Author")
            .WithDescription("Creates a new author entry")
            .Produces<AuthorDto>();

        group.MapPost("/search",
                ([FromBody] SearchAuthorsRequestDto request, ISearchAuthorsController controller) =>
                    controller.SearchAuthors(request))
            .WithName("Search Authors")
            .WithDescription("Search for authors in the library")
            .Produces<SearchAuthorsResponseDto>();

        return app;
    }
}
