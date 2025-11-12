using System.Collections.Concurrent;

using LibraryManagement.Domain.Domains.Authors;
using LibraryManagement.Domain.Domains.Authors.Create;

namespace LibraryManagement.Application.Tests.TestDoubles;

internal class InMemoryAuthorPersistence : ICreateAuthorPort
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
}
