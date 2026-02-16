using System.ComponentModel;

namespace LibraryManagement.Api.Rest.Client.Domain.Authors;

[Description("Author DTO")]
public class AuthorDto
{
    [Description("Author identifier")] public required string Id { get; init; }

    [Description("Author name")] public required string Name { get; init; }
}
