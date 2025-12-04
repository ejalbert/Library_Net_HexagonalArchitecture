using LibraryManagement.Api.Rest.Domains.Authors;
using LibraryManagement.Api.Rest.Domains.Authors.CreateAuthor;
using LibraryManagement.Api.Rest.Domains.Books;
using LibraryManagement.Api.Rest.Domains.Books.CreateNewBook;
using LibraryManagement.Api.Rest.Domains.Books.DeleteBook;
using LibraryManagement.Api.Rest.Domains.Books.GetSingleBook;
using LibraryManagement.Api.Rest.Domains.Books.Search;
using LibraryManagement.Api.Rest.Domains.Books.UpdateBook;
using LibraryManagement.Api.Rest.ModuleConfigurations;
using LibraryManagement.Domain.Domains.Ai.BookSuggestions.Create;
using LibraryManagement.Domain.Domains.Authors.Create;
using LibraryManagement.Domain.Domains.Authors.Search;
using LibraryManagement.Domain.Domains.Books.Create;
using LibraryManagement.Domain.Domains.Books.Delete;
using LibraryManagement.Domain.Domains.Books.GetSingle;
using LibraryManagement.Domain.Domains.Books.Patch;
using LibraryManagement.Domain.Domains.Books.Search;
using LibraryManagement.Domain.Domains.Books.Update;
using LibraryManagement.ModuleBootstrapper.AspNetCore.Extensions;
using LibraryManagement.ModuleBootstrapper.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using Moq;

namespace LibraryManagement.Api.Rest.Tests.ModuleConfigurations;

public class ApiModuleTests
{
    [Fact]
    public void AddRestApiModule_UsesConfigurationAndDelegateOverrides()
    {
        WebApplicationBuilder builder = CreateBuilder();
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["RestApi:BasePath"] = "/configured"
        });

        builder
            .InitializeApplicationModuleConfiguration()
            .AddRestApiModule(options => options.BasePath = "/delegate");

        using ServiceProvider provider = builder.Services.BuildServiceProvider();
        IOptions<RestApiModuleOptions> options = provider.GetRequiredService<IOptions<RestApiModuleOptions>>();

        Assert.Equal("/delegate", options.Value.BasePath);
    }

    [Fact]
    public void AddRestApiModule_DefaultsBasePathWhenConfigurationMissing()
    {
        WebApplicationBuilder builder = CreateBuilder();

        builder
            .InitializeApplicationModuleConfiguration()
            .AddRestApiModule();

        using ServiceProvider provider = builder.Services.BuildServiceProvider();
        IOptions<RestApiModuleOptions> options = provider.GetRequiredService<IOptions<RestApiModuleOptions>>();

        Assert.Equal("/api", options.Value.BasePath);
    }

    [Fact]
    public void AddRestApiModule_RegistersServices()
    {
        WebApplicationBuilder builder = CreateBuilder();
        RegisterUseCases(builder.Services);

        builder
            .InitializeApplicationModuleConfiguration()
            .AddRestApiModule();

        using ServiceProvider provider = builder.Services.BuildServiceProvider();

        Assert.IsType<BookDtoMapper>(provider.GetRequiredService<IBookDtoMapper>());
        Assert.IsType<AuthorDtoMapper>(provider.GetRequiredService<IAuthorDtoMapper>());
        Assert.NotNull(provider.GetRequiredService<ICreateAuthorController>());
        Assert.NotNull(provider.GetRequiredService<ICreateNewBookController>());
        Assert.NotNull(provider.GetRequiredService<IDeleteBookController>());
        Assert.NotNull(provider.GetRequiredService<IGetBookController>());
        Assert.NotNull(provider.GetRequiredService<ISearchBooksController>());
        Assert.NotNull(provider.GetRequiredService<IUpdateBookController>());
    }

    [Fact]
    public void UseRestApiModule_MapsBookEndpoints()
    {
        WebApplicationBuilder builder = CreateBuilder();
        RegisterUseCases(builder.Services);

        builder
            .InitializeApplicationModuleConfiguration()
            .AddRestApiModule();

        using WebApplication app = builder.Build();
        app.UseApplicationModules().UseRestApiModule();

        var endpoints = GetRouteEndpoints(app).ToList();
        var routes = endpoints.Select(endpoint => endpoint.RoutePattern.RawText ?? string.Empty).ToList();

        Assert.Contains(routes, pattern => pattern.Contains("/api/v1/books"));
        Assert.Contains(routes, pattern => pattern.Contains("/api/v1/authors"));
        Assert.Contains(routes, pattern => pattern.Contains("/api/v1/books/{id}"));
        Assert.Contains(routes, pattern => pattern.Contains("/api/v1/books/search"));
        Assert.Contains(endpoints, endpoint =>
            (endpoint.RoutePattern.RawText ?? string.Empty).Contains("/api/v1/books/{id}") &&
            endpoint.Metadata.OfType<HttpMethodMetadata>().Any(metadata => metadata.HttpMethods.Contains("DELETE")));
        Assert.Contains(endpoints, endpoint =>
            (endpoint.RoutePattern.RawText ?? string.Empty).Contains("/api/v1/books/{id}") &&
            endpoint.Metadata.OfType<HttpMethodMetadata>().Any(metadata => metadata.HttpMethods.Contains("PUT")));
    }

    private static WebApplicationBuilder CreateBuilder()
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder();
        builder.Environment.EnvironmentName = Environments.Development;
        return builder;
    }

    private static void RegisterUseCases(IServiceCollection services)
    {
        services.AddSingleton(Mock.Of<ICreateAuthorUseCase>());
        services.AddSingleton(Mock.Of<ISearchAuthorsUseCase>());
        services.AddSingleton(Mock.Of<ICreateNewBookUseCase>());
        services.AddSingleton(Mock.Of<IDeleteBookUseCase>());
        services.AddSingleton(Mock.Of<IGetSingleBookUseCase>());
        services.AddSingleton(Mock.Of<ISearchBooksUseCase>());
        services.AddSingleton(Mock.Of<IUpdateBookUseCase>());
        services.AddSingleton(Mock.Of<IPatchBookUseCase>());
        services.AddSingleton(Mock.Of<ICreateBookSuggestionUseCase>());
    }

    private static IEnumerable<RouteEndpoint> GetRouteEndpoints(WebApplication app)
    {
        var routeBuilder = (IEndpointRouteBuilder)app;
        return routeBuilder.DataSources
            .SelectMany(source => source.Endpoints.OfType<RouteEndpoint>());
    }
}
