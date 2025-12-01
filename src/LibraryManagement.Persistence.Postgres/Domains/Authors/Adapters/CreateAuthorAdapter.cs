using LibraryManagement.Domain.Domains.Authors;
using LibraryManagement.Domain.Domains.Authors.Create;
using LibraryManagement.Persistence.Postgres.DbContexts;

namespace LibraryManagement.Persistence.Postgres.Domains.Authors.Adapters;

public class CreateAuthorAdapter(LibraryManagementDbContext context, IAuthorEntityMapper mapper) : ICreateAuthorPort
{
    public async Task<Author> Create(string name)
    {

        var author = new AuthorEntity()
        {
            Name = name
        };

        context.Authors.Add(author);
        await context.SaveChangesAsync();

        return mapper.ToDomain(author);
    }
}
