using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Api.Rest.Client.Domain.Books.Create;

public record CreateNewBookRequestDto(
    [Required(AllowEmptyStrings = false)] string Title,
    [Required(AllowEmptyStrings = false)] string AuthorId,
    [MinLength(1)] string? Description,
    IEnumerable<string>? Keywords);
