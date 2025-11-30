using LibraryManagement.Domain.Infrastructure.Tenants.GetCurrentUserTenantId;

using Microsoft.AspNetCore.Http;

namespace LibraryManagement.Api.Rest.Infrastructure.Tenants.Adapters;

public class GetCurrentUserTenantIdAdapter(IHttpContextAccessor httpContextAccessor) : IGetCurrentUserTenantIdPort
{
    public string GetTenantId()
    {
        var httpContext = httpContextAccessor.HttpContext ?? throw new InvalidOperationException("No HttpContext.");;

        return httpContext.User.FindFirst("tenant_id")?.Value ?? "00000000-0000-0000-0000-000000000001";
    }
}
