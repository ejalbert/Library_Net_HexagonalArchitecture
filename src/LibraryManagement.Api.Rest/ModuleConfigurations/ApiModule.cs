using LibraryManagement.Api.Rest.Domains.Books;
using LibraryManagement.ModuleBootstrapper.AspNetCore.ModuleConfigurators;
using LibraryManagement.ModuleBootstrapper.ModuleRegistrators;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LibraryManagement.Api.Rest.ModuleConfigurations;

public static class ApiModule
{
    public static IModuleRegistrator<TApplicationBuilder> AddRestApiModule<TApplicationBuilder>(
        this IModuleRegistrator<TApplicationBuilder> moduleRegistrator,
        Action<RestApiModuleOptions>? configureOptions = null)
        where TApplicationBuilder : IHostApplicationBuilder
    {
        var services = moduleRegistrator.Services;

        RestApiModuleEnvConfiguration optionsFromEnv = new();
        moduleRegistrator.ConfigurationManager.GetSection("RestApi").Bind(optionsFromEnv);

        services.AddOptions<RestApiModuleOptions>().Configure(options =>
        {
            options.BasePath = optionsFromEnv.BasePath ?? "/api";
        });

        if (configureOptions != null)
        {
            services.Configure(configureOptions);
        }

        services
            .AddOpenApi()
            .AddValidation()
            .AddProblemDetails()
            .AddBookServices();


        return moduleRegistrator;
    }

    public static IModuleConfigurator UseRestApiModule(this IModuleConfigurator configurator)
    {
        var app = configurator.App;
        var options = app.Services.GetRequiredService<Microsoft.Extensions.Options.IOptions<RestApiModuleOptions>>().Value;

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }


        app.UseBookServices();

        return configurator;
    }
}


