using LibraryManagement.Persistence.Postgres.Domains.Authors;

using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Postgres.Seeders.Domain.Authors;

public static class AuthorSeeder
{
    public static TDbContext SeedAuthors<TDbContext>(this TDbContext context) where TDbContext : DbContext
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
            },
            new AuthorEntity {
                Id = Guid.Parse("d1e2f3a4-b5c6-4d7e-8f9a-0b1c2d3e4f5a"),
                Name = "Suzanne Collins"
            },
            new AuthorEntity {
                Id = Guid.Parse("e2f3a4b5-c6d7-4e8f-9a0b-1c2d3e4f5a6b"),
                Name = "Rick Riordan"
            },
            new AuthorEntity {
                Id = Guid.Parse("f3a4b5c6-d7e8-4f9a-0b1c-2d3e4f5a6b7c"),
                Name = "Stephen King"
            },
            new AuthorEntity {
                Id = Guid.Parse("a4b5c6d7-e8f9-4a0b-1c2d-3e4f5a6b7c8d"),
                Name = "Brandon Sanderson"
            },
            new AuthorEntity {
                Id = Guid.Parse("b5c6d7e8-f9a0-4b1c-2d3e-4f5a6b7c8d9e"),
                Name = "C.S. Lewis"
            }
        };

        authors.AddRange(authorList);
        context.SaveChanges();

        return context;
    }

}
