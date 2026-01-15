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

    ICommand LoadBooksCommand { get; }

}

internal partial class BookListViewModel :   ObservableObject, IBookListViewModel
{
    private readonly IRestAPiClient _restAPiClient;

    [ObservableProperty]
    private IList<BookDto> _books = [];


    public ICommand LoadBooksCommand { get; }


    public BookListViewModel(IRestAPiClient restAPiClient)
    {
        _restAPiClient = restAPiClient;
        LoadBooksCommand = new AsyncRelayCommand(LoadBooks);

    }
    

    private async Task LoadBooks(CancellationToken cancellationToken = default)
    {
        
        var result = await _restAPiClient.Books.Search(new(), cancellationToken);
        Books = result.Results.ToList();
    }
}

internal class BookListViewModelDesignTime : IBookListViewModel
{
    public IList<BookDto> Books { get; } = [new() { Title = "Mon livre", AuthorId = "", Id = "", Description = "Ceci est mon livre", Keywords = [] }];

    public ICommand LoadBooksCommand { get; } = new RelayCommand(() => { });

    public event PropertyChangedEventHandler? PropertyChanged;
    public event PropertyChangingEventHandler? PropertyChanging;
}

