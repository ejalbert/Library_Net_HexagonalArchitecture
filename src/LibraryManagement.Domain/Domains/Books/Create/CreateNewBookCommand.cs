namespace LibraryManagement.Domain.Domains.Books.Create;

public record CreateNewBookCommand(
    string Title,
    string AuthorId,
    string Description,
    IReadOnlyCollection<string> Keywords);
