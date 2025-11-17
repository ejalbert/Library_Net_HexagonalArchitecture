using LibraryManagement.Api.Rest.ModuleConfigurations;
using LibraryManagement.Domain.ModuleConfigurations;
using LibraryManagement.ModuleBootstrapper.AspNetCore.Extensions;
using LibraryManagement.ModuleBootstrapper.Extensions;
using LibraryManagement.Persistence.Mongo.ModuleConfigurations;
using LibraryManagement.Web.ModuleConfigurations;

var builder = WebApplication.CreateBuilder(args);

builder
    .InitializeApplicationModuleConfiguration()
    .AddDomainModule()
    .AddPersistenceMongoModule()
    .AddRestApiModule()
    .AddWebModule();

var app = builder.Build();

var moduleConfigurator = app.UseApplicationModules();

app.UseHttpsRedirection();

moduleConfigurator.UseRestApiModule().UseWebModule();

app.Run();

namespace LibraryManagement.Application
{
    public partial class Program;
}
