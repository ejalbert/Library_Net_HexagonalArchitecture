using LibraryManagement.Domain.Domains.Books.CreateNewBook;
using LibraryManagement.Domain.Domains.Books.GetSingleBook;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Domain.Domains.Books;

internal static class BookServiceRegistration
{
    internal static IServiceCollection AddBookServices(this IServiceCollection services)
    {
        return services
            .AddScoped<ICreateNewBookUseCase, CreateNewBookService>()
            .AddScoped<IGetSingleBookUseCase, GetSingleBookService>();
        
    } 
}