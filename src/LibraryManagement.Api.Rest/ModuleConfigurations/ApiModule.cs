using LibraryManagement.Api.Rest.Domains.Authors;
using LibraryManagement.Api.Rest.Domains.Books;
using LibraryManagement.Api.Rest.Infrastructure.Tenants;
using LibraryManagement.ModuleBootstrapper.AspNetCore.ModuleConfigurators;
using LibraryManagement.ModuleBootstrapper.ModuleRegistrators;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace LibraryManagement.Api.Rest.ModuleConfigurations;

public static class ApiModule
{
    public static IModuleRegistrator<TApplicationBuilder> AddRestApiModule<TApplicationBuilder>(
        this IModuleRegistrator<TApplicationBuilder> moduleRegistrator,
        Action<RestApiModuleOptions>? configureOptions = null)
        where TApplicationBuilder : IHostApplicationBuilder
    {
        IServiceCollection services = moduleRegistrator.Services;

        RestApiModuleEnvConfiguration optionsFromEnv = new();
        moduleRegistrator.ConfigurationManager.GetSection("RestApi").Bind(optionsFromEnv);

        services.AddOptions<RestApiModuleOptions>().Configure(options =>
        {
            options.BasePath = optionsFromEnv.BasePath ?? "/api";
        });

        if (configureOptions != null) services.Configure(configureOptions);

        services
            .AddOpenApi()
            .AddValidation()
            .AddProblemDetails()
            .AddAuthorServices()
            .AddBookServices()
            .AddTenantServices();


        return moduleRegistrator;
    }

    public static IModuleConfigurator UseRestApiModule(this IModuleConfigurator configurator)
    {
        WebApplication app = configurator.App;
        RestApiModuleOptions options = app.Services.GetRequiredService<IOptions<RestApiModuleOptions>>().Value;

        if (app.Environment.IsDevelopment()) app.MapOpenApi("/api/{documentName}.json");

        app.UseRouting();

        app.UseAuthorServices();
        app.UseBookServices();

        return configurator;
    }
}
