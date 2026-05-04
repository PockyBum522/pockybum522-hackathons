using Android.App;
using Android.Content.PM;
using ApparitionsClient.Services;
using ApparitionsClient.Android.Services;
using Avalonia;
using Avalonia.Android;
using Avalonia.Maui;
using Microsoft.Extensions.DependencyInjection;

[assembly: UsesPermission(Android.Manifest.Permission.AccessCoarseLocation)]
[assembly: UsesPermission(Android.Manifest.Permission.AccessFineLocation)]
[assembly: UsesFeature("android.hardware.location", Required = true)]
[assembly: UsesFeature("android.hardware.location.gps", Required = true)]
[assembly: UsesFeature("android.hardware.location.network", Required = true)]
[assembly: UsesPermission(Android.Manifest.Permission.AccessBackgroundLocation)]

namespace ApparitionsClient.Android;

[Activity(
    Label = "ApparitionsClient.Android",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>
{
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        var services = new ServiceCollection();
        
        services.AddApparitionsClientCore();
        
        services.AddSingleton<IAudioPlayerService, AndroidAudioPlayerService>();
        services.AddSingleton<ILocationService, AndroidLocationService>();
        services.AddSingleton<IPermissionService, AndroidPermissionService>();
        services.AddSingleton<IScenarioStorage, AndroidScenarioStorage>();
        
        return base.CustomizeAppBuilder(builder)
            .UseMaui<MauiApplication>(this)
            .WithInterFont();
    }
}
