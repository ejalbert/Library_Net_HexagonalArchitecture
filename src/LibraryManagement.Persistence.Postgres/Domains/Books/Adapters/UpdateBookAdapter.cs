using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Domain.Domains.Books.Update;

using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Postgres.Domains.Books.Adapters;

public class UpdateBookAdapter(IBookEntityMapper mapper, LibraryManagementDbContext dbContext) : IUpdateBookPort
{
    public async Task<Book> Update(string id, string title, string authorId, string description,
        IReadOnlyCollection<string> keywords)
    {
        BookEntity? entity = await LoadBook(id);

        if (entity is null) throw new InvalidOperationException($"Book '{id}' was not found.");

        entity.Title = title;
        entity.AuthorId = authorId;
        entity.Description = description;

        entity.Keywords.Clear();
        foreach (string keyword in keywords)
        {
            entity.Keywords.Add(new BookKeywordEntity
            {
                BookId = entity.Id,
                Book = entity,
                Keyword = keyword
            });
        }

        await dbContext.SaveChangesAsync();

        return mapper.ToDomain(entity);
    }

    private Task<BookEntity?> LoadBook(string id)
    {
        if (Guid.TryParse(id, out Guid bookId))
            return dbContext.Books.Include(b => b.Keywords).SingleOrDefaultAsync(b => b.Id == bookId);

        return dbContext.Books.Include(b => b.Keywords).SingleOrDefaultAsync(b => b.Id.ToString() == id);
    }
}
