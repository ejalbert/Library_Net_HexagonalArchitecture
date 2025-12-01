using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Persistence.Postgres.Domains.Books;

internal static class ConfigureBookDbSet
{
    internal static ModelBuilder ConfigureBooks(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookEntity>().ConfigureBookEntity();
        modelBuilder.Entity<BookKeywordEntity>().ConfigureBookKeywordEntity();

        return modelBuilder;
    }

    private static EntityTypeBuilder<BookEntity> ConfigureBookEntity(this EntityTypeBuilder<BookEntity> typeBuilder)
    {
        typeBuilder.ToTable("Books");
        typeBuilder.HasKey(b => b.Id);
        typeBuilder.HasOne(b => b.Author).WithMany(a => a.Books).HasForeignKey(b => b.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);


        return typeBuilder;
    }

    private static EntityTypeBuilder<BookKeywordEntity> ConfigureBookKeywordEntity(
        this EntityTypeBuilder<BookKeywordEntity> typeBuilder)
    {
        typeBuilder.ToTable("BookKeyword");
        typeBuilder.HasKey(k => new { k.BookId, k.Keyword });
        typeBuilder.HasOne(k => k.Book).WithMany(b => b.Keywords).HasForeignKey(k => k.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        return typeBuilder;
    }
}
