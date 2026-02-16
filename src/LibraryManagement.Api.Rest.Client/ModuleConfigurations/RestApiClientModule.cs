using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LibraryManagement.Api.Rest.Client.ModuleConfigurations;

public static class RestApiClientModule
{
    public static IServiceCollection AddRestApiHttpClient<TConfigurationManager>(this IServiceCollection services,
        TConfigurationManager configurationManager, Action<RestApiClientModuleOptions>? configureOptions = null,
        Action<IHttpClientBuilder>? configureClient = null)
        where TConfigurationManager : IConfiguration, IConfigurationBuilder
    {
        RestApiClientEnvConfiguration optionsFromEnv = new();
        configurationManager.GetSection("RestApi").Bind(optionsFromEnv);

        services.AddOptions<RestApiClientModuleOptions>().Configure(options =>
        {
            var basePath = optionsFromEnv.BasePath ?? "http://localhost:5007";

            basePath = basePath.TrimEnd('/');
            if (!basePath.EndsWith("/api", StringComparison.OrdinalIgnoreCase)) basePath += "/api";

            options.BasePath = $"{basePath}/";
        });

        if (configureOptions != null) services.Configure(configureOptions);

        IHttpClientBuilder httpClientBuilder =
            services.AddHttpClient<IRestAPiClient, RestApiClient>((serviceProvider, client) =>
            {
                RestApiClientModuleOptions options =
                    serviceProvider.GetRequiredService<IOptions<RestApiClientModuleOptions>>().Value;

                client.BaseAddress = new Uri(options.BasePath);
            });

        if (configureClient != null) configureClient(httpClientBuilder);

        return services;
    }
}
