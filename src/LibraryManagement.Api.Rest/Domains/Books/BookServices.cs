using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using LibraryManagement.Api.Rest.Client.Domain.Books;
using LibraryManagement.Api.Rest.Client.Domain.Books.Create;
using LibraryManagement.Api.Rest.Client.Domain.Books.Patch;
using LibraryManagement.Api.Rest.Client.Domain.Books.Search;
using LibraryManagement.Api.Rest.Client.Domain.Books.Update;
using LibraryManagement.Api.Rest.Domains.Books.CreateNewBook;
using LibraryManagement.Api.Rest.Domains.Books.DeleteBook;
using LibraryManagement.Api.Rest.Domains.Books.GetSingleBook;
using LibraryManagement.Api.Rest.Domains.Books.PatchBook;
using LibraryManagement.Api.Rest.Domains.Books.Search;
using LibraryManagement.Api.Rest.Domains.Books.UpdateBook;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Api.Rest.Domains.Books;

internal static class BookServices
{
    internal static IServiceCollection AddBookServices(this IServiceCollection services)
    {
        services.AddScoped<IBookDtoMapper, BookDtoMapper>()
            .AddScoped<ICreateNewBookController, CreateNewBookController>()
            .AddScoped<IDeleteBookController, DeleteBookController>()
            .AddScoped<ISearchBooksController, SearchBooksController>()
            .AddScoped<IGetBookController, GetBookController>()
            .AddScoped<IUpdateBookController, UpdateBookController>()
            .AddScoped<IPatchBookController, PatchBookController>();

        return services;
    }

    internal static WebApplication UseBookServices(this WebApplication app)
    {
        RouteGroupBuilder group = app.MapGroup("/api").MapGroup("/v1/books").WithTags("Books");

        group
            .MapPost("",
                ([FromBody] CreateNewBookRequestDto requestDto, ICreateNewBookController controller) =>
                    controller.CreateNewBook(requestDto))
            .WithName("Create New Book")
            .WithDescription("Create a new book in the library")
            .Produces<BookDto>();

        group.MapGet("{id}",
                ([Required][Description("Book identifier")] string id, IGetBookController controller) =>
                    controller.GetBookById(id))
            .WithName("Get Book By Id")
            .WithDescription("Get a single book by its unique identifier")
            .Produces<BookDto>();

        group.MapPut("{id}",
                ([Required][Description("Book identifier")] string id, [FromBody] UpdateBookRequestDto requestDto,
                        IUpdateBookController controller) =>
                    controller.UpdateBook(id, requestDto))
            .WithName("Update Book")
            .WithDescription("Update a book in the library")
            .Produces<BookDto>();

        group.MapPatch("{id}",
                ([Required][Description("Book identifier")] string id, [FromBody] PatchBookRequestDto requestDto,
                        IPatchBookController controller) =>
                    controller.PatchBook(id, requestDto))
            .WithName("Patch Book")
            .WithDescription("Partially update a book in the library")
            .Produces<BookDto>();

        group.MapPost("/search",
                ([FromBody] SearchBooksRequestDto requestDto, ISearchBooksController controller) =>
                    controller.SearchBooks(requestDto))
            .WithName("Search Books")
            .WithDescription("Search for books in the library")
            .Produces<SearchBooksResponseDto>();

        group.MapDelete("{id}",
                ([Required][Description("Book identifier")] string id, IDeleteBookController controller) =>
                    controller.DeleteBook(id))
            .WithName("Delete Book")
            .WithDescription("Delete a book from the library")
            .Produces(StatusCodes.Status204NoContent);

        return app;
    }
}
