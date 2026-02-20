using LibraryManagement.Api.Rest.Client.Domain.Books;

namespace LibraryManagement.Clients.Desktop.Domain.Books.Components;

internal class BookListViewModelDesignTime
{
    public IList<BookDto> Books { get; } =
        [new() { Title = "Mon livre", AuthorId = "", Id = "", Description = "Ceci est mon livre", Keywords = [] },
    new() { Title = "Mon livre 2", AuthorId = "", Id = "", Description = "C'est la suite de mon livre", Keywords = [] }];

    public bool IsLoading => false;
}
