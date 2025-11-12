namespace LibraryManagement.Api.Rest.Client.Domain.Books;

public class BookDto
{
    public required string Id { get; set; }
    public required string Title { get; set; }
    public required string AuthorId { get; set; }
}
