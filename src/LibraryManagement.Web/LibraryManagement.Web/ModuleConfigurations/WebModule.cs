using LibraryManagement.ModuleBootstrapper.AspNetCore.ModuleConfigurators;
using LibraryManagement.ModuleBootstrapper.ModuleRegistrators;
using LibraryManagement.Web.Client.ModuleConfigurations;
using LibraryManagement.Web.Components;
using LibraryManagement.Web.Domain.Authors;

using _Imports = LibraryManagement.Web.Client._Imports;

namespace LibraryManagement.Web.ModuleConfigurations;

public static class WebModule
{
    public static IModuleRegistrator<TApplicationBuilder> AddWebModule<TApplicationBuilder>(
        this IModuleRegistrator<TApplicationBuilder> builder) where TApplicationBuilder : IHostApplicationBuilder
    {
        var baseAdresses = builder.ConfigurationManager["ASPNETCORE_URLS"]?.Split(";") ?? [];
        var baseAdress = baseAdresses.FirstOrDefault(a => a.StartsWith("https://"),
            baseAdresses.FirstOrDefault(a => a.StartsWith("http://"), null));

        builder.Services
            .AddWebClientModule(builder.ConfigurationManager, builder.ConfigurationManager.GetConnectionString("restApi") ?? baseAdress)
            .AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

        builder.Services.AddAuthorServices();

        return builder;
    }

    public static IModuleConfigurator UseWebModule(this IModuleConfigurator configurator)
    {
        WebApplication app = configurator.App;

        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error", true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }


        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(_Imports).Assembly);

        return configurator;
    }
}
