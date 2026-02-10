using System.ComponentModel;

namespace LibraryManagement.Api.Rest.Client.Domain.Books.Patch;

[Description("Request DTO for patching a book")]
public record PatchBookRequestDto(
    [property: Description("Updated title of the book")]
    string? Title = null,
    [property: Description("Updated author identifier for the book")]
    string? AuthorId = null,
    [property: Description("Updated description of the book")]
    string? Description = null,
    [property: Description("Updated keywords for the book")]
    IEnumerable<string>? Keywords = null);
