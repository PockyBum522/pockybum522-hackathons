using Android.App;
using Android.Content.PM;
using Avalonia.Android;

namespace Aoc2023CSharp.Android;

[Activity(Label = "Aoc2023CSharp.Android", Theme = "@style/MyTheme.NoActionBar", Icon = "@drawable/icon", LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity
{
}
