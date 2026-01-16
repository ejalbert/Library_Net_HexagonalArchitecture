using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using LibraryManagement.Api.Rest.Client;
using LibraryManagement.Api.Rest.Client.Domain.Books;
using LibraryManagement.Clients.Desktop.Mvvm;

using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Clients.Desktop.Domain.Books.Components;

/// <summary>
/// Interaction logic for BookList.xaml
/// </summary>
public partial class BookList : UserControl
{
    public BookList()
    {
        DataContext = App.Current.Services.GetRequiredService<IBookListViewModel>();
        InitializeComponent();        
    }
}

internal interface IBookListViewModel : IObservableObject
{
    IList<BookDto> Books { get; }

    ICommand ToggleLoadBooksCommand { get; }

    bool IsLoading { get; }
}

internal partial class BookListViewModel :   ObservableObject, IBookListViewModel
{
    private readonly IRestAPiClient _restAPiClient;

    private CancellationTokenSource? _loadBooksCts;

    [ObservableProperty]
    private IList<BookDto> _books = [];

    [ObservableProperty]
    private bool _isLoading;

    private readonly IAsyncRelayCommand _toggleLoadBooksCommand;

    public ICommand ToggleLoadBooksCommand => _toggleLoadBooksCommand;

    public BookListViewModel(IRestAPiClient restAPiClient)
    {
        _restAPiClient = restAPiClient;
        _toggleLoadBooksCommand = new AsyncRelayCommand(
            ToggleLoadBooks,
            AsyncRelayCommandOptions.AllowConcurrentExecutions);
    }

    private async Task ToggleLoadBooks()
    {
        if (IsLoading)
        {
            CancelLoadBooks();
            return;
        }

        await LoadBooks();
    }

    private async Task LoadBooks(CancellationToken cancellationToken = default)
    {
        if (IsLoading)
        {
            return;
        }

        IsLoading = true;
        _loadBooksCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        try
        {
            await Task.Delay(5000, _loadBooksCts.Token); // Simulate loading delay
            var result = await _restAPiClient.Books.Search(new(), _loadBooksCts.Token);
            Books = result.Results.ToList();
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            _loadBooksCts.Dispose();
            _loadBooksCts = null;
            IsLoading = false;
        }
    }

    private void CancelLoadBooks()
    {
        _loadBooksCts?.Cancel();
    }
}

internal class BookListViewModelDesignTime : IBookListViewModel
{
    public IList<BookDto> Books { get; } = [new() { Title = "Mon livre", AuthorId = "", Id = "", Description = "Ceci est mon livre", Keywords = [] }];

    public ICommand ToggleLoadBooksCommand { get; } = new RelayCommand(() => { });

    public bool IsLoading => false;

    public event PropertyChangedEventHandler? PropertyChanged;
    public event PropertyChangingEventHandler? PropertyChanging;
}

