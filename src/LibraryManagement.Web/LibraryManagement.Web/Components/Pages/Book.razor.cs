using LibraryManagement.Api.Rest.Client;
using LibraryManagement.Api.Rest.Client.Domain.Books;
using LibraryManagement.Api.Rest.Client.Domain.Books.Search;
using Microsoft.AspNetCore.Components;

namespace LibraryManagement.Web.Components.Pages;

public partial class Book(IRestAPiClient restAPiClient) : ComponentBase
{
    


    
    private IEnumerable<BookDto> BookDetails { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        BookDetails = (await restAPiClient.Books.Search(new())).Books;
    }
}