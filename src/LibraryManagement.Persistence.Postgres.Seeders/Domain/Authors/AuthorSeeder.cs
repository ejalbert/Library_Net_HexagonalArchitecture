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



        return context;
    }

}
