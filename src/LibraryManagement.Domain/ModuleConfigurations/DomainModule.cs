using LibraryManagement.Domain.Domains.Authors;
using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.ModuleBootstrapper.ModuleRegistrators;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LibraryManagement.Domain.ModuleConfigurations;

public static class DomainModule
{
    public static IModuleRegistrator<TApplicationBuilder> AddDomainModule<TApplicationBuilder>(this IModuleRegistrator<TApplicationBuilder> moduleRegistrator, Action<DomainModuleOptions>? configureOptions = null) where TApplicationBuilder : IHostApplicationBuilder
    {

        DomainModuleOptions optionsFromEnv = new();
        moduleRegistrator.ConfigurationManager.GetSection("Domain").Bind(optionsFromEnv);

        moduleRegistrator.Services.AddOptions<DomainModuleOptions>().Configure(options =>
        {
            options.Test = optionsFromEnv.Test;
        });

        if (configureOptions != null)
        {
            moduleRegistrator.Services.Configure(configureOptions);
        }

        moduleRegistrator.Services
            .AddAuthorServices()
            .AddBookServices();

        return moduleRegistrator;
    }
}
