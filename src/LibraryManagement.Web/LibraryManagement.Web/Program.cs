using LibraryManagement.ModuleBootstrapper.AspNetCore.Extensions;
using LibraryManagement.ModuleBootstrapper.Extensions;
using LibraryManagement.Web.ModuleConfigurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.InitializeApplicationModuleConfiguration().AddWebModule();

var app = builder.Build();
var moduleConfigurator = app.UseApplicationModules();

app.UseHttpsRedirection();

moduleConfigurator.UseWebModule();

app.Run();
