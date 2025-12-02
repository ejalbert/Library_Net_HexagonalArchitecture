using LibraryManagement.Domain.Common.Searches;

namespace LibraryManagement.Domain.Domains.Authors.Search;

public record SearchAuthorsCommand(string? SearchTerm, Pagination? Pagination)
    : SearchCommandBase(SearchTerm, Pagination);
