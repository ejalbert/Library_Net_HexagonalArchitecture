namespace LibraryManagement.Domain.Common.Searches;

public record SearchCommandBase(string? SearchTerm, Pagination? Pagination);
