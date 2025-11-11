using LibraryManagement.Domain.Domains.Books.Create;
using LibraryManagement.Domain.Domains.Books.GetSingle;
using LibraryManagement.Domain.Domains.Books.Search;

using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Domain.Domains.Books;

internal static class BookServiceRegistration
{
    internal static IServiceCollection AddBookServices(this IServiceCollection services)
    {
        return services
            .AddScoped<ICreateNewBookUseCase, CreateNewBookService>()
            .AddScoped<IGetSingleBookUseCase, GetSingleBookService>()
            .AddScoped<ISearchBooksUseCase, SearchBooksService>();
    }
}
