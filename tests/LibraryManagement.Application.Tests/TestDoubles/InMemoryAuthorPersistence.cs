using System.Collections.Concurrent;

using LibraryManagement.Domain.Common.Searches;
using LibraryManagement.Domain.Domains.Authors;
using LibraryManagement.Domain.Domains.Authors.Create;
using LibraryManagement.Domain.Domains.Authors.Search;

namespace LibraryManagement.Application.Tests.TestDoubles;

internal class InMemoryAuthorPersistence : ICreateAuthorPort, ISearchAuthorsPort
{
    private readonly ConcurrentDictionary<string, Author> _authors = new();

    public Task<Author> Create(string name)
    {
        Author author = new()
        {
            Id = Guid.NewGuid().ToString("N"),
            Name = name
        };

        _authors[author.Id] = author;

        return Task.FromResult(author);
    }

    public Task<SearchResult<Author>> Search(string? searchTerm, Pagination pagination)
    {
        IEnumerable<Author> authors = _authors.Values;

        if (!string.IsNullOrWhiteSpace(searchTerm))
            authors = authors.Where(author =>
                author.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

        var results = authors.ToList();

        return Task.FromResult(new SearchResult<Author>
        {
            Results = results,
            Pagination = new PaginationInfo
            {
                TotalItems = results.Count,
                PageIndex = pagination.PageIndex,
                PageSize = pagination.PageSize
            }
        });
    }

    public void Seed(params Author[] authors)
    {
        foreach (Author author in authors) _authors[author.Id] = author;
    }

    public void Reset()
    {
        _authors.Clear();
    }
}
