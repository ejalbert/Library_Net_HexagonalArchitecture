using System.Collections.Generic;
using LibraryManagement.Domain.Domains.Books;

namespace LibraryManagement.Domain.Domains.Books.Patch;

public interface IPatchBookPort
{
    Task<Book> Patch(string id, string? title, string? authorId, string? description, IReadOnlyCollection<string>? keywords);
}
