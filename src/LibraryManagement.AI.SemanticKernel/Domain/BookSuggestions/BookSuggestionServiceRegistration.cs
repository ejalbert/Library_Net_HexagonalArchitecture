using LibraryManagement.AI.SemanticKernel.Domain.Authors.Plugins;
using LibraryManagement.AI.SemanticKernel.Domain.Books.Plugins;
using LibraryManagement.AI.SemanticKernel.Domain.BookSuggestions.Adapters;
using LibraryManagement.Domain.Domains.Ai.BookSuggestions.Create;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;

namespace LibraryManagement.AI.SemanticKernel.Domain.BookSuggestions;

internal static class BookSuggestionServiceRegistration
{
    extension(IServiceCollection services)
    {
        internal IServiceCollection AddBookSuggestionServices()
        {
            services.AddScoped<ICreateBookSuggestionPort, CreateBookSuggestionAdapter>();
            services.AddScoped<IBookSuggestionAgent, BookSuggestionAgent>();
            services.AddKeyedScoped<Kernel>(nameof(BookSuggestionAgent), (sp, key) =>
            {
                KernelPluginCollection pluginCollection = [];
                pluginCollection.AddFromObject(sp.GetRequiredService<ISearchAuthorPlugin>());
                pluginCollection.AddFromObject(sp.GetRequiredService<ISearchBooksPlugin>());

                var kernel = new Kernel(sp, pluginCollection);

                return kernel;
            });


            return services;
        }
    }
}
