using System.Text;
using System.Text.Json;

using LibraryManagement.AI.OpenAi.Domain.Common.Chat.Tools;
using LibraryManagement.Domain.Common.Searches;
using LibraryManagement.Domain.Domains.Authors;
using LibraryManagement.Domain.Domains.Authors.Search;

using OpenAI.Chat;

namespace LibraryManagement.AI.OpenAi.Domain.Authors.Tools;

public class SearchAuthorsChatTools(ISearchAuthorsUseCase searchAuthorsUseCase) : SearchChatToolBase<Author>(nameof(SearchAuthorsChatTools), ChatTool.CreateFunctionTool(
    functionName: nameof(SearchAuthorsChatTools),
    functionDescription:
    """
    Search for authors in the library catalog.
    """,
    functionParameters: BinaryData.FromBytes(Encoding.UTF8.GetBytes(
        $$"""
          {
            "type": "object",
            "properties": {
              "{{nameof(SearchCommandBase.SearchTerm)}}": {
                "type": "string",
                "description": "The term to search for authors. Typically a name or part of a name. can be empty to get all authors. when searching for an name, add a space between the first and last name."
              },
              "{{nameof(SearchCommandBase.Pagination)}}": {
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
          """)))), ISearchAuthorsChatTool
{
    protected override Task<SearchResult<Author>> ExecuteSearchAsync(string? searchTerm, Pagination? pagination, JsonDocument arguments)
    {
        return searchAuthorsUseCase.Search(new(searchTerm, pagination));
    }
}
