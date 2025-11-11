using LibraryManagement.Domain.Domains.Books.Create;
using LibraryManagement.Domain.Domains.Books.GetSingle;
using LibraryManagement.Domain.Domains.Books.Search;
using LibraryManagement.Persistence.Mongo.Domains.Books.Adapters;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Persistence.Mongo.Domains.Books;

internal static class RegisterBookServices
{
    internal static IServiceCollection AddBookServices(this IServiceCollection services)
    {
        return services
            .AddScoped<IBookEntityMapper, BookEntityMapper>()
            .AddScoped<IBookCollection, BookCollection>()
            .AddScoped<ICreateNewBookPort, CreateNewBookAdapter>()
            .AddScoped<ISearchBooksPort, SearchBooksAdapter>()
            .AddScoped<IGetSingleBookPort, GetSingleBookAdapter>();

    }
}