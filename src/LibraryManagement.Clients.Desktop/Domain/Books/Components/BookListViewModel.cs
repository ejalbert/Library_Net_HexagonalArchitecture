using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using LibraryManagement.Api.Rest.Client;
using LibraryManagement.Api.Rest.Client.Domain.Books;

namespace LibraryManagement.Clients.Desktop.Domain.Books.Components;

internal partial class BookListViewModel(IRestAPiClient restAPiClient) : ObservableRecipient
{
    
    [ObservableProperty]
    private IList<BookDto> _books = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ToggleLoadBooksCommand))]
    private bool _isLoading;    

    protected override void OnActivated()
    {
        if (!IsLoading && Books.Count == 0)
        {
            ToggleLoadBooksCommand.Execute(null);
        }
    }

    private bool CanToggleLoadBooks() => !IsLoading;

    [RelayCommand(IncludeCancelCommand = true, CanExecute = nameof(CanToggleLoadBooks))]
    private async Task ToggleLoadBooksAsync(CancellationToken cancellationToken)
    {
        if (IsLoading)
        {
            return;
        }

        IsLoading = true;

        try
        {
            var result = await restAPiClient.Books.Search(new(), cancellationToken);
            Books = result.Results.ToList();
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            IsLoading = false;
        }
    }
}
