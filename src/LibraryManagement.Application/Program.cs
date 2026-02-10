using LibraryManagement.AI.OpenAi.ModuleConfigurations;
using LibraryManagement.AI.SemanticKernel.ModuleConfigurations;
using LibraryManagement.Api.Rest.ModuleConfigurations;
using LibraryManagement.Domain.ModuleConfigurations;
using LibraryManagement.ModuleBootstrapper.AspNetCore.Extensions;
using LibraryManagement.ModuleBootstrapper.AspNetCore.ModuleConfigurators;
using LibraryManagement.ModuleBootstrapper.Extensions;
using LibraryManagement.Persistence.Mongo.ModuleConfigurations;
using LibraryManagement.Persistence.Postgres.ModuleConfiguration;
using LibraryManagement.Web.ModuleConfigurations;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder
    .AddServiceDefaults()
    .InitializeApplicationModuleConfiguration()
    .AddDomainModule()
    .AddPersistenceMongoModule()
    .AddPersistencePostgresModule()
    .AddRestApiModule()
    .AddSemanticKernelModule();
    //.AddWebModule();

WebApplication app = builder.Build();

IModuleConfigurator moduleConfigurator = app.UseApplicationModules();

app.UseHttpsRedirection();

moduleConfigurator.UseRestApiModule()
    .UseSemanticKernelModule();
    //.UseWebModule();


app.MapDefaultEndpoints();
app.Run();

namespace LibraryManagement.Application
{
    public class Program;
}
