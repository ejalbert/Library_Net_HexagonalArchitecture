using LibraryManagement.AI.SemanticKernel.LocalTools.Hub;
using LibraryManagement.AI.SemanticKernel.LocalTools.Tools;
using LibraryManagement.AI.SemanticKernel.LocalTools.Tools.ReadDirectory;
using LibraryManagement.Api.Rest.Client.ModuleConfigurations;
using LibraryManagement.ModuleBootstrapper.ModuleRegistrators;

using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LibraryManagement.AI.SemanticKernel.LocalTools.ModuleConfiguration;

public static class SemanticKernelLocalToolsModule
{
    extension<TApplicationBuilder>(IModuleRegistrator<TApplicationBuilder> moduleRegistrator)
        where TApplicationBuilder : IHostApplicationBuilder
    {
        public IModuleRegistrator<TApplicationBuilder> AddLocalToolsModule()
        {
            var services = moduleRegistrator.Services;

            services.AddScoped<IAddConnectionIdRequestHandler, AddConnectionIdRequestHandler>();
            services.AddScoped<ConnectionIdDelegatingHandler>();
            services.AddRestApiHttpClient(moduleRegistrator.ConfigurationManager, null, builder => builder.AddHttpMessageHandler<ConnectionIdDelegatingHandler>());


            services.AddSingleton<HubConnection>(_ =>
                new HubConnectionBuilder().WithUrl("http://localhost:5007/api/v1/ai/tools/local").WithAutomaticReconnect().Build());
            services.AddSingleton<ILocalToolHub, LocalToolHub>();

            services.AddReadDirectoryTool();

            return moduleRegistrator;
        }
    }

    extension(IHost host)
    {
        public IHost UseLocalToolsModule()
        {
            host.UseReadDirectoryTool();

            return host;
        }
    }
}
