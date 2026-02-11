using LibraryManagement.Api.Rest.Client.Domain.Ai.BookSuggestions.Create;
using LibraryManagement.Api.Rest.Client.Domain.Authors;
using LibraryManagement.Api.Rest.Domains.Ai.BookSuggestions.Create;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Api.Rest.Domains.Ai.BookSuggestions;

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
            RouteGroupBuilder group = app.MapGroup("/api/v1/ai").MapGroup("/book-suggestions").WithTags("Book Suggestions");

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
