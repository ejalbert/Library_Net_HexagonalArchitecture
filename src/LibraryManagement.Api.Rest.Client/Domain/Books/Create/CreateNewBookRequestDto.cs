using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Api.Rest.Client.Domain.Books.Create;

public record CreateNewBookRequestDto(
    [Required][MinLength(1)] string Title
);