using System.Windows;

using LibraryManagement.Clients.Desktop.ModuleConfigurations;
using LibraryManagement.ModuleBootstrapper.Extensions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LibraryManagement.Clients.Desktop;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly IHost _host;

    public new static App Current => (App)Application.Current;

    public IServiceProvider Services => _host.Services;

    public App()
    {
        var builder = Host.CreateApplicationBuilder();

        builder.InitializeApplicationModuleConfiguration().AddDesktopModule();        

        _host = builder.Build();

        
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();

        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
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
