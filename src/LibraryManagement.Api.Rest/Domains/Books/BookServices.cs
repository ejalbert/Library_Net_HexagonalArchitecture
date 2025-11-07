using LibraryManagement.Api.Rest.Client.Domain.Books.CreateNewBook;
using LibraryManagement.Api.Rest.Domains.Books.CreateNewBook;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Api.Rest.Domains.Books;

internal static class BookServices
{
    internal static IServiceCollection AddBookServices(this IServiceCollection services)
    {
        services.AddScoped<IBookDtoMapper, BookDtoMapper>();
        services.AddScoped<ICreateNewBookController, CreateNewBookController>();
        
        return services;
    }
    
    internal static WebApplication UseBookServices(this WebApplication app)
    {
        app.MapGroup("/api/v1/books")
            .MapPost("", ([FromBody]CreateNewBookRequestDto requestDto, ICreateNewBookController controller) => controller.CreateNewBook(requestDto))
            .Produces<BookDto>();
        
        return app;
    }
}