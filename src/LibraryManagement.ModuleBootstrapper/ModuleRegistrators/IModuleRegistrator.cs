using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LibraryManagement.ModuleBootstrapper.ModuleRegistrators;

public interface IModuleRegistrator
{
    public IServiceCollection Services { get; }
    public IConfigurationManager ConfigurationManager { get; }
    public IHostEnvironment Environment { get; }
}

public interface IModuleRegistrator<out TApplicationBuilder> : IModuleRegistrator where TApplicationBuilder : IHostApplicationBuilder
{
    TApplicationBuilder Builder { get; }
}