using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Domain.Domains.Books.Create;
using LibraryManagement.Domain.Domains.Books.Delete;
using LibraryManagement.Domain.Domains.Books.GetSingle;
using LibraryManagement.Domain.Domains.Books.Search;
using LibraryManagement.Domain.Domains.Books.Patch;

namespace LibraryManagement.Application.Tests.TestDoubles;

internal class InMemoryBookPersistence :
    ICreateNewBookPort,
    IDeleteBookPort,
    IGetSingleBookPort,
    ISearchBooksPort,
    IPatchBookPort
{
    private readonly ConcurrentDictionary<string, Book> _books = new();

    public Task<Book> Create(string title, string authorId, string description, IReadOnlyCollection<string> keywords)
    {
        var book = new Book
        {
            Id = Guid.NewGuid().ToString("N"),
            Title = title,
            AuthorId = authorId,
            Description = description,
            Keywords = keywords.ToArray()
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

    public Task<Book> Patch(string id, string? title, string? authorId, string? description, IReadOnlyCollection<string>? keywords)
    {
        if (!_books.TryGetValue(id, out var existing))
        {
            throw new KeyNotFoundException($"Book '{id}' was not found.");
        }

        Book updated = new()
        {
            Id = existing.Id,
            Title = title ?? existing.Title,
            AuthorId = authorId ?? existing.AuthorId,
            Description = description ?? existing.Description,
            Keywords = keywords?.ToArray() ?? existing.Keywords
        };

        _books[id] = updated;

        return Task.FromResult(updated);
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
