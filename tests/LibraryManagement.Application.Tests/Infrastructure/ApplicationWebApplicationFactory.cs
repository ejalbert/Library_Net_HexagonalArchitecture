using LibraryManagement.Application;
using LibraryManagement.Application.Tests.TestDoubles;
using LibraryManagement.Domain.Domains.Books.Create;
using LibraryManagement.Domain.Domains.Books.GetSingle;
using LibraryManagement.Domain.Domains.Books.Search;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace LibraryManagement.Application.Tests.Infrastructure;

public class ApplicationWebApplicationFactory : WebApplicationFactory<ApplicationAssemblyMarker>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment(Environments.Development);
        builder.UseSolutionRelativeContentRoot("src/LibraryManagement.Application");

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<ICreateNewBookPort>();
            services.RemoveAll<IGetSingleBookPort>();
            services.RemoveAll<ISearchBooksPort>();

            services.AddSingleton<InMemoryBookPersistence>();
            services.AddScoped<ICreateNewBookPort>(ResolvePersistence);
            services.AddScoped<IGetSingleBookPort>(ResolvePersistence);
            services.AddScoped<ISearchBooksPort>(ResolvePersistence);
        });
    }

    private static InMemoryBookPersistence ResolvePersistence(IServiceProvider provider) =>
        provider.GetRequiredService<InMemoryBookPersistence>();
}
