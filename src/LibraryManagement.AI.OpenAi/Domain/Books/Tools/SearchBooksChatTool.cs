using System.Text;
using System.Text.Json;

using LibraryManagement.AI.OpenAi.Domain.Common.Chat.Tools;
using LibraryManagement.Domain.Common.Searches;
using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Domain.Domains.Books.Search;

using OpenAI.Chat;

namespace LibraryManagement.AI.OpenAi.Domain.Books.Tools;

public class SearchBooksChatTool(ISearchBooksUseCase searchBooksUseCase) : SearchChatToolBase<Book>(
    nameof(SearchBooksChatTool),
    ChatTool.CreateFunctionTool(
        nameof(SearchBooksChatTool),
        """
        Search for books in the library catalog. It is not possible to search by author name here.
        You need to cross reference with the author id. For now the best approach is to search all books by using an empty search

        the content returned will include alist of reesults with the books found, along with pagination information.
        """,
        BinaryData.FromBytes(Encoding.UTF8.GetBytes(
            $$"""
              {
                "type": "object",
                "properties": {
                  "{{nameof(SearchBooksCommand.SearchTerm)}}": {
                    "type": "string",
                    "description": "The term to search for books. Typically a name or part of a name. can be empty to get all books"
                  },
                  "{{nameof(SearchBooksCommand.Pagination)}}": {
                    "type": "object",
                    "description": "Pagination parameters for the search results.",
                    "properties": {
                      "{{nameof(Pagination.PageIndex)}}": {
                        "type": "integer",
                        "description": "The page number to retrieve, starting from 0."
                      },
                      "{{nameof(Pagination.PageSize)}}": {
                        "type": "integer",
                        "description": "The number of items per page."
                      }
                    }
                  }
                }
              }
              """)))), ISearchBooksChatTool
{
    protected override Task<SearchResult<Book>> ExecuteSearchAsync(string? searchTerm, Pagination? pagination,
        JsonDocument arguments)
    {
        return searchBooksUseCase.Search(new SearchBooksCommand(searchTerm, pagination));
    }
}
