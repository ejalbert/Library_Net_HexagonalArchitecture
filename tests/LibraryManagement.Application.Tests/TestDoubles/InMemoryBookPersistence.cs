using System.Collections.Concurrent;

using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Domain.Domains.Books.Create;
using LibraryManagement.Domain.Domains.Books.Delete;
using LibraryManagement.Domain.Domains.Books.GetSingle;
using LibraryManagement.Domain.Domains.Books.Search;

namespace LibraryManagement.Application.Tests.TestDoubles;

internal class InMemoryBookPersistence :
    ICreateNewBookPort,
    IDeleteBookPort,
    IGetSingleBookPort,
    ISearchBooksPort
{
    private readonly ConcurrentDictionary<string, Book> _books = new();

    public Task<Book> Create(string title)
    {
        var book = new Book
        {
            Id = Guid.NewGuid().ToString("N"),
            Title = title
        };

        _books[book.Id] = book;
        return Task.FromResult(book);
    }

    public Task<Book> GetById(string id)
    {
        if (_books.TryGetValue(id, out var book))
        {
            return Task.FromResult(book);
        }

        throw new KeyNotFoundException($"Book '{id}' was not found.");
    }

    public Task<IEnumerable<Book>> Search(string? searchTerm)
    {
        IEnumerable<Book> books = _books.Values;

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            books = books.Where(book =>
                book.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        }

        return Task.FromResult(books);
    }

    public Task Delete(string id)
    {
        if (_books.TryRemove(id, out _))
        {
            return Task.CompletedTask;
        }

        throw new KeyNotFoundException($"Book '{id}' was not found.");
    }

    public void Seed(params Book[] books)
    {
        foreach (var book in books)
        {
            _books[book.Id] = book;
        }
    }

    public void Reset() => _books.Clear();
}
