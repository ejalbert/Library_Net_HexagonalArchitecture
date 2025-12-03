using LibraryManagement.Api.Rest.Client.Domain.Authors;
using LibraryManagement.Api.Rest.Client.Domain.BookSuggestions.Create;
using LibraryManagement.Api.Rest.Domains.BookSuggestions.Create;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Api.Rest.Domains.BookSuggestions;

internal static class BookSuggestionServices
{
    extension(IServiceCollection services)
    {
        internal IServiceCollection AddBookSuggestionServices()
        {


            return services.AddScoped<ICreateBookSuggestionController, CreateBookSuggestionController>();
        }
    }

    extension(WebApplication app)
    {
        internal WebApplication UseBookSuggestionServices()
        {
            RouteGroupBuilder group = app.MapGroup("/api").MapGroup("/v1/book-suggestions").WithGroupName("Book Suggestions");

            group.MapPost("",
                    ([FromBody] CreateBookSuggestionRequestDto request, ICreateBookSuggestionController controller) =>
                        controller.CreateBookSuggestion(request))
                .WithName("Create Book Suggestion")
                .WithDescription("Creates a new book suggestion entry")
                .Produces<AuthorDto>();

            return app;
        }
    }
}
