using LibraryManagement.Persistence.Postgres.Domains.Books;

namespace LibraryManagement.Persistence.Postgres.Domains.Authors;

public class AuthorEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<BookEntity> Books { get; set; } = new List<BookEntity>();
}
