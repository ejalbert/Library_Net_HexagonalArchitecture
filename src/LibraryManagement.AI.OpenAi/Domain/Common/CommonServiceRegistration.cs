using LibraryManagement.AI.OpenAi.Domain.Common.Chat;

using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.AI.OpenAi.Domain.Common;

internal static class CommonServiceRegistration
{
    extension(IServiceCollection services)
    {
        internal IServiceCollection AddCommonServices()
        {
            return services.AddScoped<ISingleUserPromptChatCompletion, SingleUserPromptChatCompletion>();
        }
    }
}
