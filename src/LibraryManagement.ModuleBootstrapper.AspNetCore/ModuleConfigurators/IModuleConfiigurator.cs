using Microsoft.AspNetCore.Builder;

namespace LibraryManagement.ModuleBootstrapper.AspNetCore.ModuleConfigurators;

public interface IModuleConfigurator
{
    WebApplication App { get; }
}

internal class ModuleConfigurator : IModuleConfigurator
{
    public required WebApplication App { get; init; }
}
