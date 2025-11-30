namespace LibraryManagement.Domain.Infrastructure.Tenants.GetCurrentUserTenantId;

public interface IGetCurrentUserTenantIdUseCase
{
    string GetTenantId(GetCurrentUserTenantIdCommand command);
}
