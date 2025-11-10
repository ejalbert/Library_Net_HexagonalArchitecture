using LibraryManagement.ModuleBootstrapper.AspNetCore.Extensions;
using LibraryManagement.ModuleBootstrapper.AspNetCore.ModuleConfigurators;
using Microsoft.AspNetCore.Builder;

namespace LibraryManagement.ModuleBootstrapper.AspNetCore.Tests.Extensions;

public class WebApplicationExtensionsTests
{
    [Fact]
    public void UseApplicationModules_returns_configurator_bound_to_same_app_instance()
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder();
        WebApplication app = builder.Build();

        IModuleConfigurator configurator = app.UseApplicationModules();

        Assert.Same(app, configurator.App);
    }
}
