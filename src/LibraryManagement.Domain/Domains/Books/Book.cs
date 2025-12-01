namespace LibraryManagement.Domain.Domains.Books;

public class Book
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public required string AuthorId { get; init; }
    public required string Description { get; init; }
    public required IReadOnlyCollection<string> Keywords { get; init; }

    public override string ToString()
    {
        return $"{Id} - {Title}";
    }
}
