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
            Title = "Clean Architecture"
        };

        BookEntity entity = _mapper.ToEntity(domain);

        Assert.Equal(domain.Id, entity.Id);
        Assert.Equal(domain.Title, entity.Title);
    }

    [Fact]
    public void ToDomain_maps_entity()
    {
        BookEntity entity = new()
        {
            Id = "book-2",
            Title = "The Pragmatic Programmer"
        };

        Book domain = _mapper.ToDomain(entity);

        Assert.Equal(entity.Id, domain.Id);
        Assert.Equal(entity.Title, domain.Title);
    }
}
