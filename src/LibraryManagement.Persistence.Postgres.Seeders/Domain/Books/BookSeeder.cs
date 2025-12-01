using LibraryManagement.Persistence.Postgres.Domains.Books;

using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Postgres.Seeders.Domain.Books;

internal static class BookSeeder
{
    internal static DbContext SeedBooks(this DbContext context)
    {
        var books = context.Set<BookEntity>();

        if (books.Any())
        {
            return context;
        }

        var bookList = new[]
        {
            new BookEntity {
                Id = Guid.Parse("a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d"),
                Title = "Harry Potter and the Philosopher's Stone",
                AuthorId = Guid.Parse("e7a1c2e2-8b2a-4c1a-9e2a-1b2a4c1a9e2a"),
                Description = "The first book in the Harry Potter series."
            },
            new BookEntity {
                Id = Guid.Parse("b2c3d4e5-f6a7-4b8c-9d0e-1f2a3b4c5d6e"),
                Title = "Harry Potter and the Chamber of Secrets",
                AuthorId = Guid.Parse("e7a1c2e2-8b2a-4c1a-9e2a-1b2a4c1a9e2a"),
                Description = "The second book in the Harry Potter series."
            },
            new BookEntity {
                Id = Guid.Parse("c3d4e5f6-a7b8-4c9d-8e0f-1a2b3c4d5e6f"),
                Title = "A Game of Thrones",
                AuthorId = Guid.Parse("c3d4e5f6-a7b8-4c9d-8e0f-1a2b3c4d5e6f"),
                Description = "The first book in A Song of Ice and Fire."
            },
            new BookEntity {
                Id = Guid.Parse("d4e5f6a7-b8c9-4d0e-8f1a-2b3c4d5e6f7a"),
                Title = "A Clash of Kings",
                AuthorId = Guid.Parse("c3d4e5f6-a7b8-4c9d-8e0f-1a2b3c4d5e6f"),
                Description = "The second book in A Song of Ice and Fire."
            },
            new BookEntity {
                Id = Guid.Parse("e5f6a7b8-c9d0-4e1f-8a2b-3c4d5e6f7a8b"),
                Title = "The Hobbit",
                AuthorId = Guid.Parse("b1c2d3e4-f5a6-4b7c-8d9e-0f1a2b3c4d5e"),
                Description = "A fantasy novel and children's book by J.R.R. Tolkien."
            },
            new BookEntity {
                Id = Guid.Parse("f6a7b8c9-d0e1-4f2a-8b3c-4d5e6f7a8b9c"),
                Title = "The Lord of the Rings: The Fellowship of the Ring",
                AuthorId = Guid.Parse("b1c2d3e4-f5a6-4b7c-8d9e-0f1a2b3c4d5e"),
                Description = "The first volume of The Lord of the Rings."
            }
        };

        books.AddRange(bookList);
        context.SaveChanges();

        return context;
    }
}
