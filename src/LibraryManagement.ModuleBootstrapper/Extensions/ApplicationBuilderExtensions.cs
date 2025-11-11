using LibraryManagement.ModuleBootstrapper.ModuleRegistrators;

using Microsoft.Extensions.Hosting;

namespace LibraryManagement.ModuleBootstrapper.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IModuleRegistrator<TApplicationBuilder>
        InitializeApplicationModuleConfiguration<TApplicationBuilder>(this TApplicationBuilder builder)
        where TApplicationBuilder : IHostApplicationBuilder
    {
        ModuleRegistrator<TApplicationBuilder> moduleRegistrator = new()
        {
            Builder = builder
        };

        return moduleRegistrator;
    }
}
