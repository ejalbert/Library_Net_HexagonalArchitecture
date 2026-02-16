using LibraryManagement.Application.Tests.TestDoubles;
using LibraryManagement.Domain.Domains.Authors.Create;
using LibraryManagement.Domain.Domains.Authors.Search;
using LibraryManagement.Domain.Domains.Books.Create;
using LibraryManagement.Domain.Domains.Books.Delete;
using LibraryManagement.Domain.Domains.Books.GetSingle;
using LibraryManagement.Domain.Domains.Books.Patch;
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
            services.RemoveAll<ICreateAuthorPort>();
            services.RemoveAll<ICreateNewBookPort>();
            services.RemoveAll<IDeleteBookPort>();
            services.RemoveAll<IGetSingleBookPort>();
            services.RemoveAll<ISearchBooksPort>();
            services.RemoveAll<IPatchBookPort>();
            services.RemoveAll<ISearchAuthorsPort>();

            services.AddSingleton<InMemoryAuthorPersistence>();
            services.AddSingleton<InMemoryBookPersistence>();
            services.AddScoped<ICreateAuthorPort>(provider => provider.GetRequiredService<InMemoryAuthorPersistence>());
            services.AddScoped<ISearchAuthorsPort>(provider =>
                provider.GetRequiredService<InMemoryAuthorPersistence>());
            services.AddScoped<ICreateNewBookPort>(ResolvePersistence);
            services.AddScoped<IDeleteBookPort>(ResolvePersistence);
            services.AddScoped<IGetSingleBookPort>(ResolvePersistence);
            services.AddScoped<ISearchBooksPort>(ResolvePersistence);
            services.AddScoped<IPatchBookPort>(ResolvePersistence);
        });
    }

    private static InMemoryBookPersistence ResolvePersistence(IServiceProvider provider)
    {
        return provider.GetRequiredService<InMemoryBookPersistence>();
    }
}
