using LibraryManagement.Api.Rest.Client.ModuleConfigurations;
using LibraryManagement.ModuleBootstrapper.AspNetCore.ModuleConfigurators;
using LibraryManagement.ModuleBootstrapper.ModuleRegistrators;
using LibraryManagement.Web.Client.ModuleConfigurations;
using LibraryManagement.Web.Components;

namespace LibraryManagement.Web.ModuleConfigurations;

public static class WebModule
{
    public static IModuleRegistrator<TApplicationBuilder> AddWebModule<TApplicationBuilder>(this IModuleRegistrator<TApplicationBuilder> builder) where TApplicationBuilder : IHostApplicationBuilder
    {
        builder.Services
            .AddWebClientModule(builder.ConfigurationManager)
            .AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

        return builder;
    }

    public static IModuleConfigurator UseWebModule(this IModuleConfigurator configurator)
    {
        var app = configurator.App;

        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }


        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(LibraryManagement.Web.Client._Imports).Assembly);

        return configurator;
    }
}