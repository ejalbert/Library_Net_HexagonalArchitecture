using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LibraryManagement.Api.Rest.Client.ModuleConfigurations;

public static class RestApiClientModule
{
    public static IServiceCollection AddRestApiHttpClient<TConfigurationManager>(this IServiceCollection services,
        TConfigurationManager configurationManager, Action<RestApiClientModuleOptions>? configureOptions = null)
        where TConfigurationManager : IConfiguration, IConfigurationBuilder
    {
        RestApiClientEnvConfiguration optionsFromEnv = new();
        configurationManager.GetSection("RestApi").Bind(optionsFromEnv);

        services.AddOptions<RestApiClientModuleOptions>().Configure(options =>
        {
            string basePath = optionsFromEnv.BasePath ?? "http://localhost:5007";

            basePath = basePath.TrimEnd('/');
            if (!basePath.EndsWith("/api", StringComparison.OrdinalIgnoreCase))
            {
                basePath += "/api";
            }

            options.BasePath = $"{basePath}/";
        });

        if (configureOptions != null) services.Configure(configureOptions);

        services.AddHttpClient<IRestAPiClient, RestApiClient>((serviceProvider, client) =>
        {
            RestApiClientModuleOptions options =
                serviceProvider.GetRequiredService<IOptions<RestApiClientModuleOptions>>().Value;

            client.BaseAddress = new Uri(options.BasePath);
        });

        return services;
    }
}
