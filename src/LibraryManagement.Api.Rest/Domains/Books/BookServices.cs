using LibraryManagement.Api.Rest.Client.Domain.Books;
using LibraryManagement.Api.Rest.Client.Domain.Books.Create;
using LibraryManagement.Api.Rest.Client.Domain.Books.Search;
using LibraryManagement.Api.Rest.Domains.Books.CreateNewBook;
using LibraryManagement.Api.Rest.Domains.Books.GetSingleBook;
using LibraryManagement.Api.Rest.Domains.Books.Search;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Api.Rest.Domains.Books;

internal static class BookServices
{
    internal static IServiceCollection AddBookServices(this IServiceCollection services)
    {
        services.AddScoped<IBookDtoMapper, BookDtoMapper>()
        .AddScoped<ICreateNewBookController, CreateNewBookController>()
        .AddScoped<ISearchBooksController, SearchBooksController>()
        .AddScoped<IGetBookController, GetBookController>();
        
        return services;
    }
    
    internal static WebApplication UseBookServices(this WebApplication app)
    {
        var group = app.MapGroup("/api").MapGroup("/v1/books").WithGroupName("Books");
            
            group
            .MapPost("", ([FromBody]CreateNewBookRequestDto requestDto, ICreateNewBookController controller) => controller.CreateNewBook(requestDto))
            .WithName("Create New Book")
            .WithDescription("Create a new book in the library")
            .Produces<BookDto>();
            
            group.MapGet("{id}", (string id, IGetBookController controller) => controller.GetBookById(id))
                .WithName("Get Book By Id")
                .WithDescription("Get a single book by its unique identifier")
                .Produces<BookDto>();

            group.MapPost("/search", ([FromBody]SearchBooksRequestDto requestDto, ISearchBooksController controller) => controller.SearchBooks(requestDto))
                .WithName("Search Books")
                .WithDescription("Search for books in the library")
                .Produces<SearchBooksResponseDto>();
        
        return app;
    }
}