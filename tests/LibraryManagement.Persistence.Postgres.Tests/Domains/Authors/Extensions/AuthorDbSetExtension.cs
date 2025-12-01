using LibraryManagement.Persistence.Postgres.Domains.Authors;

using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Postgres.Tests.Domains.Authors.Extensions;

internal static class AuthorDbSetExtension
{
    extension(DbSet<AuthorEntity> authors)
    {
        internal AuthorEntity JkRowling => authors.Single(a=>a.Name == "J.K. Rowling");
        internal AuthorEntity JrrTolkien => authors.Single(a=>a.Name == "J.R.R. Tolkien");
        internal AuthorEntity GeorgeRrMartin => authors.Single(a=>a.Name == "George R.R. Martin");
        internal AuthorEntity SuzanneCollins => authors.Single(a=>a.Name == "Suzanne Collins");
        internal AuthorEntity RickRiordan => authors.Single(a=>a.Name == "Rick Riordan");
        internal AuthorEntity StephenKing => authors.Single(a=>a.Name == "Stephen King");
        internal AuthorEntity BrandonSanderson => authors.Single(a=>a.Name == "Brandon Sanderson");
        internal AuthorEntity CsLewis => authors.Single(a=>a.Name == "C.S. Lewis");
    }
}
