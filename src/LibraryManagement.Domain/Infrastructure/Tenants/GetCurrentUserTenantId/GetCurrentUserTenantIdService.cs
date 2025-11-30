namespace LibraryManagement.Domain.Infrastructure.Tenants.GetCurrentUserTenantId;

public class GetCurrentUserTenantIdService(IGetCurrentUserTenantIdPort currentUserTenantIdPort) : IGetCurrentUserTenantIdUseCase
{
    public string GetTenantId(GetCurrentUserTenantIdCommand command)
    {
        return currentUserTenantIdPort.GetTenantId();
    }
}
