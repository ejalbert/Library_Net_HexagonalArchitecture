using System.ComponentModel;

namespace LibraryManagement.Api.Rest.Client.Domain.Authors.Create;

[Description("Request DTO for creating a new author")]
public record CreateAuthorRequestDto(
    [property: Description("The name of the author to create")]
    string Name);
