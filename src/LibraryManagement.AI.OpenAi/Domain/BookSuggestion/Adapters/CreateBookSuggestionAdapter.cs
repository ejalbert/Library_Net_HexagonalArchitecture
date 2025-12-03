using LibraryManagement.AI.OpenAi.Domain.Authors.Tools;
using LibraryManagement.AI.OpenAi.Domain.Books.Tools;
using LibraryManagement.AI.OpenAi.Domain.Common.Chat;
using LibraryManagement.Domain.Domains.BookSuggestions.Create;

using Microsoft.Extensions.Logging;

using OpenAI.Chat;

namespace LibraryManagement.AI.OpenAi.Domain.BookSuggestion.Adapters;

public class CreateBookSuggestionAdapter(ChatClient chatClient, ISearchBooksChatTool searchBooksChatTool, ISearchAuthorsChatTool searchAuthorsChatTool, ILogger<CreateBookSuggestionAdapter> logger) : ICreateBookSuggestionPort
{
    public Task<string> SuggestAsync(string prompt)
    {
        var executor = new SingleUserPromptChatCompletion(chatClient, logger);

        return executor.ExecuteAsync("""
                                      You are a helpful assistant for a library catalog system.
                                      Answer questions using ONLY the provided context about books when possible.
                                      If something is not in the context, explicitly say you are not sure.

                                      All the books title are in english, but it's available in the user language. Translate the title if needed.

                                      When answering questions about a book, include the book's title, author, and description.

                                      Add more context about the selected book only after you have found one that matches the question.

                                      You won't be able to search for books by author name directly, but you can search for authors first to get their IDs, then use those IDs to find books.
                                      For this, you will need to send en empty searchterm and iterate each pages of results to find books by the author ID.

                                      never add a follow-up question. it is a single question answer process. Nothing in the like of "do you want to know more?" or "is there anything else I can help you with?"
                                      or "if you desire more information, don't hesitate to ask"
                                      """, prompt, searchBooksChatTool, searchAuthorsChatTool);
    }
}



