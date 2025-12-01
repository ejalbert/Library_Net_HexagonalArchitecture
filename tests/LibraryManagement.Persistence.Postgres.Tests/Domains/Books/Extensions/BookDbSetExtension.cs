using LibraryManagement.Persistence.Postgres.Domains.Books;

using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Postgres.Tests.Domains.Books.Extensions;

internal static class BookDbSetExtension
{
    extension(DbSet<BookEntity> books)
    {
        internal BookEntity HarryPotterAndThePhilosophersStone => books.Single(b => b.Title == "Harry Potter and the Philosopher's Stone");
        internal BookEntity TheHobbit => books.Single(b => b.Title == "The Hobbit");
        internal BookEntity AGameOfThrones => books.Single(b => b.Title == "A Game of Thrones");
        internal BookEntity TheLightningThief => books.Single(b => b.Title == "The Lightning Thief");
        internal BookEntity TheShining => books.Single(b => b.Title == "The Shining");
    }
}
