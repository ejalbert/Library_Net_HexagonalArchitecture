
using LibraryManagement.Api.Rest.Client.Generated.Wrapper;

using BookDto = LibraryManagement.Api.Rest.Client.Generated.Model.BookDto;

using SearchBooksRequestDto = LibraryManagement.Api.Rest.Client.Generated.Model.SearchBooksRequestDto;

namespace LibraryManagement.Web.Components.Pages;

public partial class Home(IRestApiClient restApiClient)
{
    private IEnumerable<BookDto> BookDetails { get; set; } = [];

    private string BookSuggestionInput { get; set; } = string.Empty;
    private string BookSuggestionResult { get; set; } = string.Empty;
    private bool IsLoadingSuggestions { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var response = await restApiClient.Books.SearchBooksAsync(new SearchBooksRequestDto());
        BookDetails = response.Results;
    }

    private static string FormatKeywords(IEnumerable<string> keywords)
    {
        return string.Join(", ", keywords ?? Array.Empty<string>());
    }

    private ValueTask GenerateBookSuggestions()
    {
        IsLoadingSuggestions = true;
        BookSuggestionResult = string.Empty;

        StateHasChanged();

        // // var response =
        // //     await restApiClient.BookSuggestions.CreateBookSuggestionAsync(
        // //         new CreateBookSuggestionRequestDto(BookSuggestionInput));
        // // CreateBookSuggestionResponseDto response =
        // //     await restApiClient.BookSuggestions.GetBookSuggestion(BookSuggestionInput);
        //
        // BookSuggestionResult = response.Suggestion;
        IsLoadingSuggestions = false;

        return ValueTask.CompletedTask;
    }
}
