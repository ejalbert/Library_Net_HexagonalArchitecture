namespace LibraryManagement.Persistence.Postgres.Domains.Books;

public class BookEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; }
    public string AuthorId { get; set; }

    public string Description { get; set; }

    public ICollection<BookKeywordEntity> Keywords { get; set; } = new List<BookKeywordEntity>();
}

public class BookKeywordEntity
{
    public Guid BookId { get; set; }
    public string Keyword { get; set; }

    public BookEntity Book { get; set; }
}
