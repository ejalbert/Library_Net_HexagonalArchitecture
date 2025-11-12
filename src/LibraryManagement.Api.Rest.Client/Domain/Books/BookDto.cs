namespace LibraryManagement.Api.Rest.Client.Domain.Books;

public class BookDto
{
    public required string Id { get; set; }
    public required string Title { get; set; }
    public required string AuthorId { get; set; }
    public required string Description { get; set; }
    public required IEnumerable<string> Keywords { get; set; } = Array.Empty<string>();
}
