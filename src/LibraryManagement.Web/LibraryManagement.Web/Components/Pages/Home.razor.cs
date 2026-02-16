using LibraryManagement.Api.Rest.Client;
using LibraryManagement.Api.Rest.Client.Domain.Ai.BookSuggestions;
using LibraryManagement.Api.Rest.Client.Domain.Ai.BookSuggestions.Create;
using LibraryManagement.Api.Rest.Client.Domain.Books;
using LibraryManagement.Api.Rest.Client.Domain.Books.Search;

namespace LibraryManagement.Web.Components.Pages;

public partial class Home(IRestAPiClient restApiClient)
{
    private IEnumerable<BookDto> BookDetails { get; set; } = [];

    private string BookSuggestionInput { get; set; } = string.Empty;
    private string BookSuggestionResult { get; set; } = string.Empty;
    private bool IsLoadingSuggestions { get; set; }

    protected override async Task OnInitializedAsync()
    {
        BookDetails = (await restApiClient.Books.Search(new SearchBooksRequestDto())).Results;
    }

    private static string FormatKeywords(IEnumerable<string> keywords)
    {
        return string.Join(", ", keywords ?? Array.Empty<string>());
    }

    private async ValueTask GenerateBookSuggestions()
    {
        IsLoadingSuggestions = true;
        BookSuggestionResult = string.Empty;

        StateHasChanged();

        CreateBookSuggestionResponseDto response =
            await restApiClient.BookSuggestions.GetBookSuggestion(BookSuggestionInput);

        BookSuggestionResult = response.Suggestion;
        IsLoadingSuggestions = false;
    }
}
