using LibraryManagement.Api.Rest.Client;
using LibraryManagement.Api.Rest.Client.Domain.Books;

namespace LibraryManagement.Web.Components.Pages;

public partial class Home(IRestAPiClient restApiClient)
{

    private IEnumerable<BookDto> BookDetails { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        BookDetails = (await restApiClient.Books.Search(new())).Books;
    }

    private static string FormatKeywords(IEnumerable<string> keywords) =>
        string.Join(", ", keywords ?? Array.Empty<string>());
}
