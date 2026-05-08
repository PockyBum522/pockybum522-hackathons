using System;
using ApparitionsClient;
using ApparitionsClient.Services;
using ApparitionsClient.Desktop.Services;
using Avalonia;
using Microsoft.Extensions.DependencyInjection;

namespace ApparitionsClient.Desktop;

internal sealed class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
    {
        DemoVcons.InitializeScenario();
        
        var services = new ServiceCollection();

        services.AddApparitionsClientCore();
        
        services.AddSingleton<IAudioPlayerService, DesktopAudioPlayerService>();
        services.AddSingleton<ILocationService, DesktopLocationService>();
        services.AddSingleton<IPermissionService, DesktopPermissionService>();
        services.AddSingleton<IScenarioStorage, DesktopScenarioStorage>();
        
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
    }
}