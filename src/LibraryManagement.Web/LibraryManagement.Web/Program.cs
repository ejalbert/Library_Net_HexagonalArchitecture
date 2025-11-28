using LibraryManagement.ModuleBootstrapper.AspNetCore.Extensions;
using LibraryManagement.ModuleBootstrapper.AspNetCore.ModuleConfigurators;
using LibraryManagement.ModuleBootstrapper.Extensions;
using LibraryManagement.Web.ModuleConfigurations;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.InitializeApplicationModuleConfiguration().AddWebModule();

WebApplication app = builder.Build();
IModuleConfigurator moduleConfigurator = app.UseApplicationModules();

app.UseHttpsRedirection();

moduleConfigurator.UseWebModule();

app.Run();
