using System.ClientModel;

using LibraryManagement.AI.OpenAi.Domain.Authors;
using LibraryManagement.AI.OpenAi.Domain.Books;
using LibraryManagement.AI.OpenAi.Domain.BookSuggestion;
using LibraryManagement.ModuleBootstrapper.ModuleRegistrators;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using OpenAI.Chat;

namespace LibraryManagement.AI.OpenAi.ModuleConfigurations;

public static class OpenAiModule
{
    public static IModuleRegistrator<TApplicationBuilder> AddOpenAiModule<TApplicationBuilder>(
        this IModuleRegistrator<TApplicationBuilder> moduleRegistrator,
        Action<OpenAiModuleOptions>? configureOptions = null)
        where TApplicationBuilder : IHostApplicationBuilder
    {
        OpenAiModuleEnvConfiguration optionsFromEnv = new();
        moduleRegistrator.ConfigurationManager.GetSection("OpenAi").Bind(optionsFromEnv);

        moduleRegistrator.Services.AddOptions<OpenAiModuleOptions>().Configure(options =>
        {
            options.ApiKey = optionsFromEnv.ApiKey ?? string.Empty;
            options.Model = optionsFromEnv.Model ?? "gpt-4.1-nano";
        });

        if (configureOptions != null) moduleRegistrator.Services.Configure(configureOptions);

        moduleRegistrator.Services.AddSingleton<ChatClient>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<OpenAiModuleOptions>>().Value;

            return new ChatClient(options.Model, new ApiKeyCredential(options.ApiKey));
        });

        moduleRegistrator.Services
            .AddBookServices()
            .AddAuthorServices()
            .AddBookSuggestionServices();

        return moduleRegistrator;
    }
}
