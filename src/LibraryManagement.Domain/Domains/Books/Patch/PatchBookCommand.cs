using System.Collections.Generic;

namespace LibraryManagement.Domain.Domains.Books.Patch;

public record PatchBookCommand(
    string Id,
    string? Title = null,
    string? AuthorId = null,
    string? Description = null,
    IReadOnlyCollection<string>? Keywords = null);
