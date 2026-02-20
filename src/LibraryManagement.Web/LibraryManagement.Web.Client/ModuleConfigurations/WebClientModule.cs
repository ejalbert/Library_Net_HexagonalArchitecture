using LibraryManagement.Api.Rest.Client.Generated.Wrapper;
namespace LibraryManagement.Web.Client.ModuleConfigurations;

public static class WebClientModule
{
    public static IServiceCollection AddWebClientModule<TConfigurationManager>(this IServiceCollection services,
        TConfigurationManager configurationManager, string? baseAdress = null) where TConfigurationManager : IConfiguration, IConfigurationBuilder
    {
        services.AddLibraryManagementRestApiClient(baseAdress ?? configurationManager.GetConnectionString("RestApi") ?? "");
        //services.AddRestApiHttpClient(configurationManager);

        return services;
    }
}
