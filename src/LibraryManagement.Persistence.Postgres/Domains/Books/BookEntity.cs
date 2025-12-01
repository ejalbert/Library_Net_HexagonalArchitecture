using LibraryManagement.Persistence.Postgres.DbContexts.Multitenants;
using LibraryManagement.Persistence.Postgres.Domains.Authors;

namespace LibraryManagement.Persistence.Postgres.Domains.Books;

public class BookEntity : MultitenantEntityBase
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; }

    public Guid AuthorId { get; set; }
    public AuthorEntity Author { get; set; }

    public string Description { get; set; }

    public ICollection<BookKeywordEntity> Keywords { get; set; } = new List<BookKeywordEntity>();
}

public class BookKeywordEntity : MultitenantEntityBase
{
    public Guid BookId { get; set; }
    public string Keyword { get; set; }

    public BookEntity Book { get; set; }
}
