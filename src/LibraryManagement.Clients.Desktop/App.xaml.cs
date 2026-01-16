using System;
using System.Windows;

using CommunityToolkit.Mvvm.DependencyInjection;

using LibraryManagement.Clients.Desktop.ModuleConfigurations;
using LibraryManagement.ModuleBootstrapper.Extensions;

using Microsoft.Extensions.Hosting;

namespace LibraryManagement.Clients.Desktop;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        var builder = Host.CreateApplicationBuilder();

        builder.InitializeApplicationModuleConfiguration().AddDesktopModule();        

        _host = builder.Build();
        Ioc.Default.ConfigureServices(_host.Services);
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();

        var mainWindow = Ioc.Default.GetService<MainWindow>()
            ?? throw new InvalidOperationException("MainWindow is not registered.");
        mainWindow.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _host.StopAsync();
        _host.Dispose();

        base.OnExit(e);
    }
}
