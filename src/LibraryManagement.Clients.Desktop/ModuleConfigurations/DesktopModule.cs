using LibraryManagement.Api.Rest.Client.ModuleConfigurations;
using LibraryManagement.Clients.Desktop.Domain.Books;
using LibraryManagement.ModuleBootstrapper.ModuleRegistrators;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LibraryManagement.Clients.Desktop.ModuleConfigurations;

internal static class DesktopModule
{
    public static IModuleRegistrator<TApplicationBuilder> AddDesktopModule<TApplicationBuilder>(
       this IModuleRegistrator<TApplicationBuilder> builder) where TApplicationBuilder : IHostApplicationBuilder
    {
        builder.Services
            .AddTransient<MainWindow>()
            .AddLocalization()
            .AddRestApiHttpClient(builder.ConfigurationManager)
            .AddBookServices();
            

        return builder;
    }
}
