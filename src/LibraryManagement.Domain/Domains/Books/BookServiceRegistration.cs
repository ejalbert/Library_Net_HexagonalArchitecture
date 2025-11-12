using LibraryManagement.Domain.Domains.Books.Create;
using LibraryManagement.Domain.Domains.Books.Delete;
using LibraryManagement.Domain.Domains.Books.GetSingle;
using LibraryManagement.Domain.Domains.Books.Search;
using LibraryManagement.Domain.Domains.Books.Update;

using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Domain.Domains.Books;

internal static class BookServiceRegistration
{
    internal static IServiceCollection AddBookServices(this IServiceCollection services)
    {
        return services
            .AddScoped<ICreateNewBookUseCase, CreateNewBookService>()
            .AddScoped<IDeleteBookUseCase, DeleteBookService>()
            .AddScoped<IGetSingleBookUseCase, GetSingleBookService>()
            .AddScoped<ISearchBooksUseCase, SearchBooksService>()
            .AddScoped<IUpdateBookUseCase, UpdateBookService>();
    }
}
