using LibraryManagement.Persistence.Postgres.Domains.Books;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Postgres.Seeders.Domain.Books;

public static class BookSeeder
{
    public static TDbContext SeedBooks<TDbContext>(this TDbContext context) where TDbContext : DbContext
    {
        var books = context.Set<BookEntity>();
        var authors = context.Set<LibraryManagement.Persistence.Postgres.Domains.Authors.AuthorEntity>();

        if (books.Any())
        {
            Console.WriteLine("Books already seeded. Skipping book seeding.");
            return context;
        }

        Console.WriteLine("Retrieving author IDs from database...");
        var jkRowlingId = authors.FirstOrDefault(a => a.Name == "J.K. Rowling")?.Id;
        var tolkienId = authors.FirstOrDefault(a => a.Name == "J.R.R. Tolkien")?.Id;
        var martinId = authors.FirstOrDefault(a => a.Name == "George R.R. Martin")?.Id;

        if (jkRowlingId == null || tolkienId == null || martinId == null)
        {
            Console.WriteLine("Error: One or more required authors not found in database. Aborting book seeding.");
            return context;
        }

        Console.WriteLine("Seeding books...");

        // Harry Potter series
        var hpBooks = new[]
        {
            new BookEntity {
                Title = "Harry Potter and the Philosopher's Stone",
                AuthorId = jkRowlingId.Value,
                Description = "The first book in the Harry Potter series.",
                Keywords = new List<BookKeywordEntity> {
                    new() { Keyword = "fantasy" },
                    new() { Keyword = "magic" },
                    new() { Keyword = "school" },
                    new() { Keyword = "adventure" }
                }
            },
            new BookEntity {
                Title = "Harry Potter and the Chamber of Secrets",
                AuthorId = jkRowlingId.Value,
                Description = "The second book in the Harry Potter series.",
                Keywords = new List<BookKeywordEntity> {
                    new() { Keyword = "fantasy" },
                    new() { Keyword = "magic" },
                    new() { Keyword = "mystery" }
                }
            },
            new BookEntity {
                Title = "Harry Potter and the Prisoner of Azkaban",
                AuthorId = jkRowlingId.Value,
                Description = "The third book in the Harry Potter series.",
                Keywords = new List<BookKeywordEntity> {
                    new() { Keyword = "fantasy" },
                    new() { Keyword = "magic" },
                    new() { Keyword = "time travel" }
                }
            },
            new BookEntity {
                Title = "Harry Potter and the Goblet of Fire",
                AuthorId = jkRowlingId.Value,
                Description = "The fourth book in the Harry Potter series.",
                Keywords = new List<BookKeywordEntity> {
                    new() { Keyword = "fantasy" },
                    new() { Keyword = "magic" },
                    new() { Keyword = "tournament" }
                }
            },
            new BookEntity {
                Title = "Harry Potter and the Order of the Phoenix",
                AuthorId = jkRowlingId.Value,
                Description = "The fifth book in the Harry Potter series.",
                Keywords = new List<BookKeywordEntity> {
                    new() { Keyword = "fantasy" },
                    new() { Keyword = "magic" },
                    new() { Keyword = "rebellion" }
                }
            },
            new BookEntity {
                Title = "Harry Potter and the Half-Blood Prince",
                AuthorId = jkRowlingId.Value,
                Description = "The sixth book in the Harry Potter series.",
                Keywords = new List<BookKeywordEntity> {
                    new() { Keyword = "fantasy" },
                    new() { Keyword = "magic" },
                    new() { Keyword = "secrets" }
                }
            },
            new BookEntity {
                Title = "Harry Potter and the Deathly Hallows",
                AuthorId = jkRowlingId.Value,
                Description = "The seventh book in the Harry Potter series.",
                Keywords = new List<BookKeywordEntity> {
                    new() { Keyword = "fantasy" },
                    new() { Keyword = "magic" },
                    new() { Keyword = "war" }
                }
            }
        };

        // Lord of the Rings series
        var lotrBooks = new[]
        {
            new BookEntity {
                Title = "The Hobbit",
                AuthorId = tolkienId.Value,
                Description = "A fantasy novel and children's book by J.R.R. Tolkien.",
                Keywords = new List<BookKeywordEntity> {
                    new() { Keyword = "fantasy" },
                    new() { Keyword = "adventure" },
                    new() { Keyword = "dragons" }
                }
            },
            new BookEntity {
                Title = "The Fellowship of the Ring",
                AuthorId = tolkienId.Value,
                Description = "The first volume of The Lord of the Rings.",
                Keywords = new List<BookKeywordEntity> {
                    new() { Keyword = "fantasy" },
                    new() { Keyword = "quest" },
                    new() { Keyword = "friendship" }
                }
            },
            new BookEntity {
                Title = "The Two Towers",
                AuthorId = tolkienId.Value,
                Description = "The second volume of The Lord of the Rings.",
                Keywords = new List<BookKeywordEntity> {
                    new() { Keyword = "fantasy" },
                    new() { Keyword = "war" },
                    new() { Keyword = "journey" }
                }
            },
            new BookEntity {
                Title = "The Return of the King",
                AuthorId = tolkienId.Value,
                Description = "The third volume of The Lord of the Rings.",
                Keywords = new List<BookKeywordEntity> {
                    new() { Keyword = "fantasy" },
                    new() { Keyword = "war" },
                    new() { Keyword = "victory" }
                }
            }
        };

        // A Song of Ice and Fire series
        var asoiafBooks = new[]
        {
            new BookEntity {
                Title = "A Game of Thrones",
                AuthorId = martinId.Value,
                Description = "The first book in A Song of Ice and Fire.",
                Keywords = new List<BookKeywordEntity> {
                    new() { Keyword = "fantasy" },
                    new() { Keyword = "politics" },
                    new() { Keyword = "dragons" }
                }
            },
            new BookEntity {
                Title = "A Clash of Kings",
                AuthorId = martinId.Value,
                Description = "The second book in A Song of Ice and Fire.",
                Keywords = new List<BookKeywordEntity> {
                    new() { Keyword = "fantasy" },
                    new() { Keyword = "war" },
                    new() { Keyword = "politics" }
                }
            },
            new BookEntity {
                Title = "A Storm of Swords",
                AuthorId = martinId.Value,
                Description = "The third book in A Song of Ice and Fire.",
                Keywords = new List<BookKeywordEntity> {
                    new() { Keyword = "fantasy" },
                    new() { Keyword = "betrayal" },
                    new() { Keyword = "war" }
                }
            },
            new BookEntity {
                Title = "A Feast for Crows",
                AuthorId = martinId.Value,
                Description = "The fourth book in A Song of Ice and Fire.",
                Keywords = new List<BookKeywordEntity> {
                    new() { Keyword = "fantasy" },
                    new() { Keyword = "politics" },
                    new() { Keyword = "intrigue" }
                }
            },
            new BookEntity {
                Title = "A Dance with Dragons",
                AuthorId = martinId.Value,
                Description = "The fifth book in A Song of Ice and Fire.",
                Keywords = new List<BookKeywordEntity> {
                    new() { Keyword = "fantasy" },
                    new() { Keyword = "dragons" },
                    new() { Keyword = "war" }
                }
            }
        };

        // Suzanne Collins - The Hunger Games
        var collinsId = authors.FirstOrDefault(a => a.Name == "Suzanne Collins")?.Id;
        var hungerGamesBooks = new[]
        {
            new BookEntity {
                Title = "The Hunger Games",
                AuthorId = collinsId.Value,
                Description = "First book in The Hunger Games trilogy.",
                Keywords = new List<BookKeywordEntity> {
                    new BookKeywordEntity { Keyword = "dystopian" },
                    new BookKeywordEntity { Keyword = "survival" },
                    new BookKeywordEntity { Keyword = "rebellion" }
                }
            },
            new BookEntity {
                Title = "Catching Fire",
                AuthorId = collinsId.Value,
                Description = "Second book in The Hunger Games trilogy.",
                Keywords = new List<BookKeywordEntity> {
                    new BookKeywordEntity { Keyword = "dystopian" },
                    new BookKeywordEntity { Keyword = "revolution" },
                    new BookKeywordEntity { Keyword = "games" }
                }
            },
            new BookEntity {
                Title = "Mockingjay",
                AuthorId = collinsId.Value,
                Description = "Final book in The Hunger Games trilogy.",
                Keywords = new List<BookKeywordEntity> {
                    new BookKeywordEntity { Keyword = "dystopian" },
                    new BookKeywordEntity { Keyword = "war" },
                    new BookKeywordEntity { Keyword = "freedom" }
                }
            }
        };

        // Rick Riordan - Percy Jackson
        var riordanId = authors.FirstOrDefault(a => a.Name == "Rick Riordan")?.Id;
        var percyJacksonBooks = new[]
        {
            new BookEntity {
                Title = "The Lightning Thief",
                AuthorId = riordanId.Value,
                Description = "First book in Percy Jackson & the Olympians.",
                Keywords = new List<BookKeywordEntity> {
                    new BookKeywordEntity { Keyword = "fantasy" },
                    new BookKeywordEntity { Keyword = "mythology" },
                    new BookKeywordEntity { Keyword = "adventure" }
                }
            },
            new BookEntity {
                Title = "The Sea of Monsters",
                AuthorId = riordanId.Value,
                Description = "Second book in Percy Jackson & the Olympians.",
                Keywords = new List<BookKeywordEntity> {
                    new BookKeywordEntity { Keyword = "fantasy" },
                    new BookKeywordEntity { Keyword = "mythology" },
                    new BookKeywordEntity { Keyword = "quest" }
                }
            },
            new BookEntity {
                Title = "The Titan's Curse",
                AuthorId = riordanId.Value,
                Description = "Third book in Percy Jackson & the Olympians.",
                Keywords = new List<BookKeywordEntity> {
                    new BookKeywordEntity { Keyword = "fantasy" },
                    new BookKeywordEntity { Keyword = "mythology" },
                    new BookKeywordEntity { Keyword = "prophecy" }
                }
            },
            new BookEntity {
                Title = "The Battle of the Labyrinth",
                AuthorId = riordanId.Value,
                Description = "Fourth book in Percy Jackson & the Olympians.",
                Keywords = new List<BookKeywordEntity> {
                    new BookKeywordEntity { Keyword = "fantasy" },
                    new BookKeywordEntity { Keyword = "mythology" },
                    new BookKeywordEntity { Keyword = "labyrinth" }
                }
            },
            new BookEntity {
                Title = "The Last Olympian",
                AuthorId = riordanId.Value,
                Description = "Fifth book in Percy Jackson & the Olympians.",
                Keywords = new List<BookKeywordEntity> {
                    new BookKeywordEntity { Keyword = "fantasy" },
                    new BookKeywordEntity { Keyword = "mythology" },
                    new BookKeywordEntity { Keyword = "war" }
                }
            }
        };

        // Stephen King - The Dark Tower
        var kingId = authors.FirstOrDefault(a => a.Name == "Stephen King")?.Id;
        var darkTowerBooks = new[]
        {
            new BookEntity {
                Title = "The Gunslinger",
                AuthorId = kingId.Value,
                Description = "First book in The Dark Tower series.",
                Keywords = new List<BookKeywordEntity> {
                    new BookKeywordEntity { Keyword = "fantasy" },
                    new BookKeywordEntity { Keyword = "western" },
                    new BookKeywordEntity { Keyword = "quest" }
                }
            },
            new BookEntity {
                Title = "The Drawing of the Three",
                AuthorId = kingId.Value,
                Description = "Second book in The Dark Tower series.",
                Keywords = new List<BookKeywordEntity> {
                    new BookKeywordEntity { Keyword = "fantasy" },
                    new BookKeywordEntity { Keyword = "portal" },
                    new BookKeywordEntity { Keyword = "adventure" }
                }
            },
            new BookEntity {
                Title = "The Waste Lands",
                AuthorId = kingId.Value,
                Description = "Third book in The Dark Tower series.",
                Keywords = new List<BookKeywordEntity> {
                    new BookKeywordEntity { Keyword = "fantasy" },
                    new BookKeywordEntity { Keyword = "dystopian" },
                    new BookKeywordEntity { Keyword = "journey" }
                }
            },
            new BookEntity {
                Title = "Wizard and Glass",
                AuthorId = kingId.Value,
                Description = "Fourth book in The Dark Tower series.",
                Keywords = new List<BookKeywordEntity> {
                    new BookKeywordEntity { Keyword = "fantasy" },
                    new BookKeywordEntity { Keyword = "magic" },
                    new BookKeywordEntity { Keyword = "romance" }
                }
            }
            // ...other books omitted for brevity...
        };

        // Brandon Sanderson - Mistborn
        var sandersonId = authors.FirstOrDefault(a => a.Name == "Brandon Sanderson")?.Id;
        var mistbornBooks = new[]
        {
            new BookEntity {
                Title = "Mistborn: The Final Empire",
                AuthorId = sandersonId.Value,
                Description = "First book in Mistborn trilogy.",
                Keywords = new List<BookKeywordEntity> {
                    new BookKeywordEntity { Keyword = "fantasy" },
                    new BookKeywordEntity { Keyword = "magic" },
                    new BookKeywordEntity { Keyword = "rebellion" }
                }
            },
            new BookEntity {
                Title = "Mistborn: The Well of Ascension",
                AuthorId = sandersonId.Value,
                Description = "Second book in Mistborn trilogy.",
                Keywords = new List<BookKeywordEntity> {
                    new BookKeywordEntity { Keyword = "fantasy" },
                    new BookKeywordEntity { Keyword = "magic" },
                    new BookKeywordEntity { Keyword = "politics" }
                }
            },
            new BookEntity {
                Title = "Mistborn: The Hero of Ages",
                AuthorId = sandersonId.Value,
                Description = "Third book in Mistborn trilogy.",
                Keywords = new List<BookKeywordEntity> {
                    new BookKeywordEntity { Keyword = "fantasy" },
                    new BookKeywordEntity { Keyword = "magic" },
                    new BookKeywordEntity { Keyword = "apocalypse" }
                }
            }
        };

        // C.S. Lewis - Narnia
        var lewisId = authors.FirstOrDefault(a => a.Name == "C.S. Lewis")?.Id;
        var narniaBooks = new[]
        {
            new BookEntity {
                Title = "The Lion, the Witch and the Wardrobe",
                AuthorId = lewisId.Value,
                Description = "First published Narnia book.",
                Keywords = new List<BookKeywordEntity> {
                    new BookKeywordEntity { Keyword = "fantasy" },
                    new BookKeywordEntity { Keyword = "magic" },
                    new BookKeywordEntity { Keyword = "adventure" }
                }
            },
            new BookEntity {
                Title = "Prince Caspian",
                AuthorId = lewisId.Value,
                Description = "Second published Narnia book.",
                Keywords = new List<BookKeywordEntity> {
                    new BookKeywordEntity { Keyword = "fantasy" },
                    new BookKeywordEntity { Keyword = "war" },
                    new BookKeywordEntity { Keyword = "return" }
                }
            },
            new BookEntity {
                Title = "The Voyage of the Dawn Treader",
                AuthorId = lewisId.Value,
                Description = "Third published Narnia book.",
                Keywords = new List<BookKeywordEntity> {
                    new BookKeywordEntity { Keyword = "fantasy" },
                    new BookKeywordEntity { Keyword = "sea" },
                    new BookKeywordEntity { Keyword = "adventure" }
                }
            }
            // ...other books omitted for brevity...
        };

        var allBooks = hpBooks.Concat(lotrBooks).Concat(asoiafBooks)
            .Concat(hungerGamesBooks)
            .Concat(percyJacksonBooks)
            .Concat(darkTowerBooks)
            .Concat(mistbornBooks)
            .Concat(narniaBooks)
            .ToList();
        books.AddRange(allBooks);
        context.SaveChanges();
        Console.WriteLine($"Seeded {allBooks.Count} books with embedded keywords.");

        return context;
    }
}
