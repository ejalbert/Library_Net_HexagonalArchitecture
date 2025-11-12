using System.Collections.Generic;

namespace LibraryManagement.Api.Rest.Client.Domain.Books.Patch;

public record PatchBookRequestDto(
    string? Title = null,
    string? AuthorId = null,
    string? Description = null,
    IEnumerable<string>? Keywords = null);
