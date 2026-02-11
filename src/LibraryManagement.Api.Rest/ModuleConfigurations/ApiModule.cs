using LibraryManagement.Api.Rest.Common;
using LibraryManagement.Api.Rest.Domains.Ai;
using LibraryManagement.Api.Rest.Domains.Ai.BookSuggestions;
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

using Scalar.AspNetCore;

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
            .AddCommonServices()
            .AddAuthorServices()
            .AddBookServices()
            .AddAiServices()
            .AddTenantServices();


        return moduleRegistrator;
    }

    public static IModuleConfigurator UseRestApiModule(this IModuleConfigurator configurator)
    {
        WebApplication app = configurator.App;
        RestApiModuleOptions options = app.Services.GetRequiredService<IOptions<RestApiModuleOptions>>().Value;

        if (app.Environment.IsDevelopment())
        {
            var openApiV1 = "/api/v1.json";
            app.MapOpenApi("/api/{documentName}.json");

            app.UseSwaggerUI(o =>
            {
                o.RoutePrefix = "dev-ui/swagger";
                o.SwaggerEndpoint(openApiV1, "OpenAPI V1");
            });

            app.UseReDoc(o =>
            {
                o.RoutePrefix = "dev-ui/api-docs";
                o.SpecUrl(openApiV1);
            });

            app.MapScalarApiReference("dev-ui/scalar",o =>
            {

                o.WithOpenApiRoutePattern("/api/{documentName}.json");
            });
        }

        app.UseRouting();

        app.UseAuthorServices();
        app.UseBookServices();
        app.UseBookSuggestionServices();

        return configurator;
    }
}
