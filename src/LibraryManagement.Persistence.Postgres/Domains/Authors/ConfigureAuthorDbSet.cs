using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Persistence.Postgres.Domains.Authors;

internal static class ConfigureAuthorDbSet
{
    internal static ModelBuilder ConfigureAuthors(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthorEntity>().ConfigureAuthorEntity();

        return modelBuilder;
    }

    private static EntityTypeBuilder<AuthorEntity> ConfigureAuthorEntity(this EntityTypeBuilder<AuthorEntity> typeBuilder)
    {
        typeBuilder.ToTable("Authors");
        typeBuilder.HasKey(a => a.Id);

        return typeBuilder;
    }

}
