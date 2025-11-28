using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Domain.Domains.Books.Patch;

using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Postgres.Domains.Books.Adapters;

public class PatchBookAdapter(IBookEntityMapper mapper, LibraryManagementDbContext dbContext) : IPatchBookPort
{
    public async Task<Book> Patch(string id, string? title, string? authorId, string? description,
        IReadOnlyCollection<string>? keywords)
    {
        BookEntity? entity = await LoadBook(id);

        if (entity is null) throw new InvalidOperationException($"Book '{id}' was not found.");

        bool hasUpdates = false;

        if (title is not null)
        {
            entity.Title = title;
            hasUpdates = true;
        }

        if (authorId is not null)
        {
            entity.AuthorId = authorId;
            hasUpdates = true;
        }

        if (description is not null)
        {
            entity.Description = description;
            hasUpdates = true;
        }

        if (keywords is not null)
        {
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

            hasUpdates = true;
        }

        if (hasUpdates) await dbContext.SaveChangesAsync();

        return mapper.ToDomain(entity);
    }

    private Task<BookEntity?> LoadBook(string id)
    {
        if (Guid.TryParse(id, out Guid bookId))
            return dbContext.Books.Include(b => b.Keywords).SingleOrDefaultAsync(b => b.Id == bookId);

        return dbContext.Books.Include(b => b.Keywords).SingleOrDefaultAsync(b => b.Id.ToString() == id);
    }
}
