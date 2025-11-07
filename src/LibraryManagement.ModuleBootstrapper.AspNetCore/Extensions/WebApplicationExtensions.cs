using LibraryManagement.ModuleBootstrapper.AspNetCore.ModuleConfigurators;
using Microsoft.AspNetCore.Builder;

namespace LibraryManagement.ModuleBootstrapper.AspNetCore.Extensions;

public static class WebApplicationExtensions
{
    public static IModuleConfigurator UseApplicationModules(this WebApplication webApplication)
    {
        return new ModuleConfigurator { App = webApplication };
    }
}