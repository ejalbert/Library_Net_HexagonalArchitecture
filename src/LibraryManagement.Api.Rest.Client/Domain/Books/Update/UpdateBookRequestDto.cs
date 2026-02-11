using System.ComponentModel;

namespace LibraryManagement.Api.Rest.Client.Domain.Books.Update;

[Description("Request DTO for updating a book")]
public record UpdateBookRequestDto(
    [property: Description("Title of the book")]
    string Title,
    [property: Description("Author identifier for the book")]
    string AuthorId,
    [property: Description("Description of the book")]
    string Description,
    [property: Description("Keywords for the book")]
    IEnumerable<string> Keywords);
