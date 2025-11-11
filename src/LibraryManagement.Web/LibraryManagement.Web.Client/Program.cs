using LibraryManagement.Web.Client.ModuleConfigurations;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddWebClientModule(builder.Configuration);


await builder.Build().RunAsync();
