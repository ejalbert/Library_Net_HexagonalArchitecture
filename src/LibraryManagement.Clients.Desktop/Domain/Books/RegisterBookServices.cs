using LibraryManagement.Clients.Desktop.Domain.Books.Components;

using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Clients.Desktop.Domain.Books;

internal static class RegisterBookServices
{
    extension(IServiceCollection services)
    {
        internal IServiceCollection AddBookServices()
        {

            services.AddTransient<BookListViewModel>();
            return services;
        }
    }
}
