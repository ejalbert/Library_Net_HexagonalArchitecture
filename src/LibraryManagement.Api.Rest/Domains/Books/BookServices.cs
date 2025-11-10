using LibraryManagement.Api.Rest.Client.Domain.Books.CreateNewBook;
using LibraryManagement.Api.Rest.Domains.Books.CreateNewBook;
using LibraryManagement.Api.Rest.Domains.Books.GetSingleBook;
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
        .AddScoped<IGetBookController, GetBookController>();
        
        return services;
    }
    
    internal static WebApplication UseBookServices(this WebApplication app)
    {
        var group = app.MapGroup("/api/v1/books").WithGroupName("Books");
            
            group
            .MapPost("", ([FromBody]CreateNewBookRequestDto requestDto, ICreateNewBookController controller) => controller.CreateNewBook(requestDto))
            .WithName("Create New Book")
            .WithDescription("Create a new book in the library")
            .Produces<BookDto>();
            
            group.MapGet("{id}", (string id, IGetBookController controller) => controller.GetBookById(id))
                .Produces<BookDto>();
        
        return app;
    }
}