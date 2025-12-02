using System.ClientModel;

using LibraryManagement.ModuleBootstrapper.ModuleRegistrators;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using OpenAI.Chat;

namespace LibraryManagement.AI.OpenApi.ModuleConfigurations;

public static class OpenApiModule
{
    public static IModuleRegistrator<TApplicationBuilder> AddOpenApiModule<TApplicationBuilder>(
        this IModuleRegistrator<TApplicationBuilder> moduleRegistrator,
        Action<OpenApiModuleOptions>? configureOptions = null)
        where TApplicationBuilder : IHostApplicationBuilder
    {
        OpenApiModuleEnvConfiguration optionsFromEnv = new();
        moduleRegistrator.ConfigurationManager.GetSection("OpenApi").Bind(optionsFromEnv);

        moduleRegistrator.Services.AddOptions<OpenApiModuleOptions>().Configure(options =>
        {
            options.ApiKey = optionsFromEnv.ApiKey ?? string.Empty;
            options.BaseUrl = optionsFromEnv.BaseUrl ?? string.Empty;
            options.Model = optionsFromEnv.Model ?? "gpt-4o-mini";
        });

        if (configureOptions != null) moduleRegistrator.Services.Configure(configureOptions);

        moduleRegistrator.Services.AddSingleton<ChatClient>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<OpenApiModuleOptions>>().Value;

            return new ChatClient(options.Model, new ApiKeyCredential(options.ApiKey));
        });

        return moduleRegistrator;
    }
}
