using CommunityToolkit.Mvvm.Input;

using LibraryManagement.Api.Rest.Client.Domain.Books;

namespace LibraryManagement.Clients.Desktop.Domain.Books.Components;

internal class BookListViewModelDesignTime
{
    public IList<BookDto> Books { get; } =
        [new() { Title = "Mon livre", AuthorId = "", Id = "", Description = "Ceci est mon livre", Keywords = [] }];

    public IAsyncRelayCommand ToggleLoadBooksCommand { get; } =
        new AsyncRelayCommand(() => Task.CompletedTask);

    public IRelayCommand ToggleLoadBooksCancelCommand { get; } = new RelayCommand(() => { });

    public bool IsLoading => false;

    
}
