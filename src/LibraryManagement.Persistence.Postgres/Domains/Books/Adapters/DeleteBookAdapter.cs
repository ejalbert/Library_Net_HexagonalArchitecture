using LibraryManagement.Domain.Domains.Books.Delete;
using LibraryManagement.Persistence.Postgres.DbContext;

using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Postgres.Domains.Books.Adapters;

public class DeleteBookAdapter(LibraryManagementDbContext context) : IDeleteBookPort
{
    public async Task Delete(string id)
    {
        int deleted;

        if (Guid.TryParse(id, out Guid bookId))
            deleted = await context.Books.Where(b => b.Id == bookId).ExecuteDeleteAsync();
        else
            deleted = await context.Books.Where(b => b.Id.ToString() == id).ExecuteDeleteAsync();

        if (deleted == 0) throw new InvalidOperationException($"Book '{id}' was not found.");
    }
}
