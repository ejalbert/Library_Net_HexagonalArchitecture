using LibraryManagement.Api.Rest.Client;
using LibraryManagement.Api.Rest.Client.Domain.Books;

using Microsoft.AspNetCore.Components;

namespace LibraryManagement.Web.Domain.Books.ListBooks;

public partial class Book(IRestAPiClient restClient) : ComponentBase
{
    [Parameter]
    public string BookId { get; set; } = string.Empty;

    private BookDto BookDetails { get; set; } = null!;

    protected override async Task OnParametersSetAsync()
    {
        BookDetails = await restClient.Books.Get(BookId);
    }
}

