// See https://aka.ms/new-console-template for more information

using LibraryManagement.AI.SemanticKernel.LocalTools.Hub;
using LibraryManagement.AI.SemanticKernel.LocalTools.ModuleConfiguration;
using LibraryManagement.AI.SemanticKernel.LocalTools.Tools;
using LibraryManagement.Api.Rest.Client;
using LibraryManagement.Api.Rest.Client.Domain.Ai.BookSuggestions;
using LibraryManagement.ModuleBootstrapper.Extensions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder =  Host.CreateApplicationBuilder(args);

builder.InitializeApplicationModuleConfiguration().AddLocalToolsModule();



var app = builder.Build();

app.UseLocalToolsModule();

var localToolsClient = app.Services.GetRequiredService<ILocalToolHub>();
await localToolsClient.ConnectAsync();

var restApiClient = app.Services.GetRequiredService<IRestAPiClient>();

Console.WriteLine("What to you want to be suggested ?");
var prompt = Console.ReadLine();

if (!string.IsNullOrWhiteSpace(prompt))
{
    var retsult = await restApiClient.BookSuggestions.GetBookSuggestion(prompt);

    Console.WriteLine(retsult.Suggestion);
}

await localToolsClient.DisposeAsync();
