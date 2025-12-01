using LibraryManagement.Persistence.Postgres.Domains.Authors;

using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Postgres.Seeders.Domain.Authors;

internal static class AuthorSeeder
{
    internal static DbContext SeedAuthors(this DbContext context)
    {
        var authors = context.Set<AuthorEntity>();

        if (authors.Any())
        {
            return context;
        }

        var authorList = new[]
        {
            new AuthorEntity {
                Id = Guid.Parse("e7a1c2e2-8b2a-4c1a-9e2a-1b2a4c1a9e2a"),
                Name = "J.K. Rowling"
            },
            new AuthorEntity {
                Id = Guid.Parse("b1c2d3e4-f5a6-4b7c-8d9e-0f1a2b3c4d5e"),
                Name = "J.R.R. Tolkien"
            },
            new AuthorEntity {
                Id = Guid.Parse("c3d4e5f6-a7b8-4c9d-8e0f-1a2b3c4d5e6f"),
                Name = "George R.R. Martin"
            }
        };

        authors.AddRange(authorList);
        context.SaveChanges();

        return context;
    }

}
