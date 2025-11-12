using LibraryManagement.Api.Rest.Client.Domain.Authors.Create;

using Microsoft.AspNetCore.Http;

namespace LibraryManagement.Api.Rest.Domains.Authors.CreateAuthor;

public interface ICreateAuthorController
{
    Task<IResult> CreateAuthor(CreateAuthorRequestDto request);
}
