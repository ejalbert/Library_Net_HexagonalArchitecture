using LibraryManagement.AI.OpenAi.Domain.Authors.Tools;
using LibraryManagement.AI.OpenAi.Domain.Books.Tools;
using LibraryManagement.AI.OpenAi.Domain.Common.Chat;

namespace LibraryManagement.AI.OpenAi.Domain.BookSuggestion;

public class BookSuggestionAgent(ISingleUserPromptChatCompletion singleUserPromptChatCompletion, ISearchBooksChatTool searchBooksChatTool, ISearchAuthorsChatTool searchAuthorsChatTool) : IBookSuggestionAgent
{
    public Task<string> SuggestBooksAsync(string userPrompt, CancellationToken cancellationToken = default)
    {
        return singleUserPromptChatCompletion.ExecuteAsync("""
                                                           You are a helpful assistant for a library catalog system.
                                                           Answer questions using ONLY the provided context about books when possible.

                                                           if the user provide the name of an author, it may be misspelled. Try to search with the most popular name (i.e. "J. K. Rowling" instead of "Joanne Rowling", "George R.R. Martin" instead of "GRR Martin").
                                                           The user may also provide only partial names, like "Rowling" or "Martin". In this case, try to find the most popular author with that name.
                                                           Try multiple authors if the name seems like a mashup of two names (e.g. "Rowling Martin" or "Tolkien Lewis" or "J.R.R Martin).

                                                           If something is not in the context, explicitly say you are not sure.

                                                           All the books title are in english, but it's available in the user language. Translate the title if needed.

                                                           When answering questions about a book, include the book's title, author, and description.

                                                           Add more context about the selected book only after you have found one that matches the question.

                                                           You won't be able to search for books by author name directly, but you can search for authors first to get their IDs, then use those IDs to find books.
                                                           For this, you will need to send en empty searchterm and iterate each pages of results to find books by the author ID.

                                                           never add a follow-up question. it is a single question answer process. Nothing in the like of "do you want to know more?" or "is there anything else I can help you with?"
                                                           or "if you desire more information, don't hesitate to ask"
                                                           """, userPrompt, searchBooksChatTool, searchAuthorsChatTool);
    }
}
