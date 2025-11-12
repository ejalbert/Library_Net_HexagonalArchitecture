using Microsoft.Extensions.Logging;

namespace LibraryManagement.Domain.Domains.Authors.Create;

internal class CreateAuthorService(ICreateAuthorPort createAuthorPort, ILogger<CreateAuthorService> logger) : ICreateAuthorUseCase
{
    public Task<Author> Create(CreateAuthorCommand command)
    {
        return createAuthorPort.Create(command.Name);
    }
}
