using System.Collections.Generic;

using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Persistence.Mongo.Domains.Books;

namespace LibraryManagement.Persistence.Mongo.Tests.Domains.Books;

public class BookEntityMapperTests
{
    private readonly BookEntityMapper _mapper = new();

    [Fact]
    public void ToEntity_maps_domain_book()
    {
        Book domain = new()
        {
            Id = "book-1",
            Title = "Clean Architecture",
            AuthorId = "author-1",
            Description = "Architecture patterns",
            Keywords = new[] { "architecture" }
        };

        BookEntity entity = _mapper.ToEntity(domain);

        Assert.Equal(domain.Id, entity.Id);
        Assert.Equal(domain.Title, entity.Title);
        Assert.Equal(domain.AuthorId, entity.AuthorId);
        Assert.Equal(domain.Description, entity.Description);
        Assert.Equal(domain.Keywords, entity.Keywords);
    }

    [Fact]
    public void ToDomain_maps_entity()
    {
        BookEntity entity = new()
        {
            Id = "book-2",
            Title = "The Pragmatic Programmer",
            AuthorId = "author-2",
            Description = "Pragmatic guide",
            Keywords = new List<string> { "pragmatic" }
        };

        Book domain = _mapper.ToDomain(entity);

        Assert.Equal(entity.Id, domain.Id);
        Assert.Equal(entity.Title, domain.Title);
        Assert.Equal(entity.AuthorId, domain.AuthorId);
        Assert.Equal(entity.Description, domain.Description);
        Assert.Equal(entity.Keywords, domain.Keywords);
    }
}
