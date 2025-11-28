using LibraryManagement.Domain.Domains.Books.Create;
using LibraryManagement.Domain.Domains.Books.Delete;
using LibraryManagement.Domain.Domains.Books.GetSingle;
using LibraryManagement.Domain.Domains.Books.Patch;
using LibraryManagement.Domain.Domains.Books.Search;
using LibraryManagement.Domain.Domains.Books.Update;
using LibraryManagement.Persistence.Postgres.Domains.Books.Adapters;

using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Persistence.Postgres.Domains.Books;

internal static class RegisterBookServices
{
    internal static IServiceCollection AddBookServices(this IServiceCollection services)
    {
        services
            .AddSingleton<IBookEntityMapper, BookEntityMapper>()
            .AddScoped<ICreateNewBookPort, CreateNewBookAdapter>()
            .AddScoped<IGetSingleBookPort, GetSingleBookAdapter>()
            .AddScoped<IDeleteBookPort, DeleteBookAdapter>()
            .AddScoped<IUpdateBookPort, UpdateBookAdapter>()
            .AddScoped<ISearchBooksPort, SearchBooksAdapter>()
            .AddScoped<IPatchBookPort, PatchBookAdapter>();

        return services;
    }
}
