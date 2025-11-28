using LibraryManagement.Domain.Domains.Books;

using Riok.Mapperly.Abstractions;

namespace LibraryManagement.Persistence.Postgres.Domains.Books;

public interface IBookEntityMapper
{
    Book ToDomain(BookEntity entity);
    BookEntity ToEntity(Book domain);
}

[Mapper]
public partial class BookEntityMapper : IBookEntityMapper
{
    public partial Book ToDomain(BookEntity entity);


    public partial BookEntity ToEntity(Book domain);

    private Guid MapStringToGuid(string id)
    {
        return Guid.Parse(id);
    }

    private string MapGuidToString(Guid id)
    {
        return id.ToString();
    }

    private static BookKeywordEntity MapStringToBookKeywordEntity(string keyword)
    {
        return new BookKeywordEntity
        {
            Keyword = keyword
            // BookId will be fixed up by EF when the entity graph is tracked
        };
    }


    private static IReadOnlyCollection<string> MapKeywords(ICollection<BookKeywordEntity> keywords)
    {
        return keywords.Select(k => k.Keyword).ToArray();
    }

    private static ICollection<BookKeywordEntity> MapKeywords(IReadOnlyCollection<string> keywords, Book source)
    {
        return keywords
            .Select(k => new BookKeywordEntity
            {
                BookId = Guid.Parse(source.Id),
                Keyword = k
            })
            .ToList();
    }
}
