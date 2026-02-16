using LibraryManagement.AI.SemanticKernel.Domain.Authors;
using LibraryManagement.AI.SemanticKernel.Domain.Books;
using LibraryManagement.AI.SemanticKernel.Domain.BookSuggestions;
using LibraryManagement.AI.SemanticKernel.SemanticKernel;
using LibraryManagement.ModuleBootstrapper.AspNetCore.ModuleConfigurators;
using LibraryManagement.ModuleBootstrapper.ModuleRegistrators;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace LibraryManagement.AI.SemanticKernel.ModuleConfigurations;

public static class SemanticKernelModule
{
    extension<TApplicationBuilder>(IModuleRegistrator<TApplicationBuilder> moduleRegistrator)
        where TApplicationBuilder : IHostApplicationBuilder
    {
        public IModuleRegistrator<TApplicationBuilder> AddSemanticKernelModule(
            Action<SemanticKernelModuleOptions>? configureOptions = null)
        {
            IServiceCollection services = moduleRegistrator.Services;

            SemanticKernelModuleEnvConfiguration optionsFromEnv = new();
            moduleRegistrator.ConfigurationManager.GetSection("OpenAi").Bind(optionsFromEnv);

            moduleRegistrator.Services.AddOptions<SemanticKernelModuleOptions>().Configure(options =>
            {
                options.ApiKey = optionsFromEnv.ApiKey ?? string.Empty;
                options.Model = optionsFromEnv.Model ?? "gpt-4.1-nano";
            });

            if (configureOptions != null) moduleRegistrator.Services.Configure(configureOptions);

            services
                .AddBookSuggestionServices();

            services.AddScoped<OpenAIChatCompletionService>(sp =>
                {
                    SemanticKernelModuleOptions options =
                        sp.GetRequiredService<IOptions<SemanticKernelModuleOptions>>().Value;
                    return new OpenAIChatCompletionService(options.Model, options.ApiKey);
                })
                .AddScoped<ILocalToolClient, LocalToolClient>()
                .AddScoped<ITokenAwareChatCompletionService, TokenAwareChatCompletionService>()
                .AddScoped<IChatCompletionService>(sp => sp.GetRequiredService<ITokenAwareChatCompletionService>())
                .AddSingleton<ToolHub>()
                .AddScoped<IToolHub>(sp => sp.GetRequiredService<ToolHub>())
                .AddAuthorServices()
                .AddBookServices()
                .AddBookSuggestionServices().AddSignalR();


            return moduleRegistrator;
        }
    }

    extension(IModuleConfigurator moduleConfigurator)
    {
        public IModuleConfigurator UseSemanticKernelModule()
        {
            moduleConfigurator.App.MapHub<ToolHub>("api/v1/ai/tools/local");

            return moduleConfigurator;
        }
    }
}
