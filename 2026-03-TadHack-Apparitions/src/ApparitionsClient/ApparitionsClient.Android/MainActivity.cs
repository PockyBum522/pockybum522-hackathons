using Android.App;
using Android.Content.PM;
using Avalonia;
using Avalonia.Android;
using Avalonia.Maui;

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
        
        
        return base.CustomizeAppBuilder(builder)
            .UseMaui<MauiApplication>(this)
            .WithInterFont();
    }
}
