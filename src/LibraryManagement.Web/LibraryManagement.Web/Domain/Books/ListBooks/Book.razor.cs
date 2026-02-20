using LibraryManagement.Api.Rest.Client.Generated.Model;
using LibraryManagement.Api.Rest.Client.Generated.Wrapper;

using Microsoft.AspNetCore.Components;

namespace LibraryManagement.Web.Domain.Books.ListBooks;

public partial class Book(IRestApiClient restClient) : ComponentBase
{
    [Parameter] public string BookId { get; set; } = string.Empty;

    private BookDto BookDetails { get; set; } = null!;

    protected override async Task OnParametersSetAsync()
    {
        BookDetails = await restClient.Books.GetBookByIdAsync(BookId);
    }
}
