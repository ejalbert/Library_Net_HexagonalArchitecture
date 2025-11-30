using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Domain.Domains.Books.Create;
using LibraryManagement.Persistence.Postgres.DbContext;

namespace LibraryManagement.Persistence.Postgres.Domains.Books.Adapters;

public class CreateNewBookAdapter(IBookEntityMapper mapper, LibraryManagementDbContext dbContext) : ICreateNewBookPort
{
    public async Task<Book> Create(string title, string authorId, string description,
        IReadOnlyCollection<string> keywords)
    {
        var entity = new BookEntity
        {
            Title = title,
            AuthorId = authorId,
            Description = description
        };

        IEnumerable<BookKeywordEntity> keywordList = keywords.Select(k => new BookKeywordEntity
        {
            Book = entity,
            Keyword = k
        });

        foreach (BookKeywordEntity keywordEntity in keywordList) entity.Keywords.Add(keywordEntity);

        dbContext.Books.Add(entity);
        await dbContext.SaveChangesAsync();

        return mapper.ToDomain(entity);
    }
}
