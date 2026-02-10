using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Api.Rest.Client.Domain.Books.Create;

[Description("Request DTO for creating a new book")]
public record CreateNewBookRequestDto(
    [property: Description("Title of the book")]
    [Required(AllowEmptyStrings = false)] string Title,
    [property: Description("Author identifier for the book")]
    [Required(AllowEmptyStrings = false)] string AuthorId,
    [property: Description("Description of the book")]
    [MinLength(1)] string? Description,
    [property: Description("Keywords for the book")]
    IEnumerable<string>? Keywords);
