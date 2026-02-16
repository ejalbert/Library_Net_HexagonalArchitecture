using System.ComponentModel;

namespace LibraryManagement.Api.Rest.Client.Domain.Books;

[Description("Book DTO")]
public class BookDto
{
    [Description("Book identifier")] public required string Id { get; set; }

    [Description("Book title")] public required string Title { get; set; }

    [Description("Author identifier for the book")]
    public required string AuthorId { get; set; }

    [Description("Book description")] public required string Description { get; set; }

    [Description("Book keywords")] public required IEnumerable<string> Keywords { get; set; } = Array.Empty<string>();
}
