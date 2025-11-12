using LibraryManagement.Api.Rest.Client.Domain.Authors.Create;
using LibraryManagement.Domain.Domains.Authors.Create;

using Microsoft.AspNetCore.Http;

namespace LibraryManagement.Api.Rest.Domains.Authors.CreateAuthor;

public class CreateAuthorController(ICreateAuthorUseCase createAuthorUseCase, IAuthorDtoMapper mapper) : ICreateAuthorController
{
    public async Task<IResult> CreateAuthor(CreateAuthorRequestDto request)
    {
        var author = await createAuthorUseCase.Create(new CreateAuthorCommand(request.Name));

        return Results.Ok(mapper.ToDto(author));
    }
}
