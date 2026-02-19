//using LibraryManagement.Api.Rest.Client.Generated.Wrapper;
//using LibraryManagement.Api.Rest.Client.ModuleConfigurations;

namespace LibraryManagement.Web.Client.ModuleConfigurations;

public static class WebClientModule
{
    public static IServiceCollection AddWebClientModule<TConfigurationManager>(this IServiceCollection services,
        TConfigurationManager configurationManager) where TConfigurationManager : IConfiguration, IConfigurationBuilder
    {
        //services.AddRestApiHttpClient(configurationManager);

        return services;
    }
}
