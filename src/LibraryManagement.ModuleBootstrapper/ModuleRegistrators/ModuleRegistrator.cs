using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LibraryManagement.ModuleBootstrapper.ModuleRegistrators;

internal class ModuleRegistrator<TApplicationBuilder> : IModuleRegistrator<TApplicationBuilder>
    where TApplicationBuilder : IHostApplicationBuilder
{
    public required  TApplicationBuilder Builder { get; init; }
    public IServiceCollection Services =>Builder.Services;
    public IConfigurationManager ConfigurationManager => Builder.Configuration;
    public IHostEnvironment Environment =>Builder.Environment;
}