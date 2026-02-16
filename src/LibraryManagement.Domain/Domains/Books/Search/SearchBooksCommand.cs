using LibraryManagement.Domain.Common.Searches;

namespace LibraryManagement.Domain.Domains.Books.Search;

public record SearchBooksCommand(string? SearchTerm, Pagination? Pagination)
    : SearchCommandBase(SearchTerm, Pagination);
