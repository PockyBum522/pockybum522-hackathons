using System;
using System.Threading;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Application = Android.App.Application;
using Avalonia;
using Avalonia.Android;
using Avalonia.ReactiveUI;

namespace AndroidPromptAndSend.Android;

[Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
public class SplashActivity : AvaloniaSplashActivity<App>
{
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        // Make sure we have microphone permissions
        if (ContextCompat.CheckSelfPermission (this, Manifest.Permission.RecordAudio) != Permission.Granted)
        {
            ActivityCompat.RequestPermissions (this, new [] { Manifest.Permission.RecordAudio }, 1);
        }
        
        if (ContextCompat.CheckSelfPermission (this, Manifest.Permission.Internet) != Permission.Granted)
        {
            ActivityCompat.RequestPermissions (this, new [] { Manifest.Permission.Internet }, 1);
        }
        
        if (ContextCompat.CheckSelfPermission (this, Manifest.Permission.ManageDocuments) != Permission.Granted)
        {
            ActivityCompat.RequestPermissions (this, new [] { Manifest.Permission.ManageDocuments }, 1);
        }

        if (ContextCompat.CheckSelfPermission (this, Manifest.Permission.ManageMedia) != Permission.Granted)
        {
            ActivityCompat.RequestPermissions (this, new [] { Manifest.Permission.ManageMedia }, 1);
        }

        if (ContextCompat.CheckSelfPermission (this, Manifest.Permission.ReadExternalStorage) != Permission.Granted)
        {
            ActivityCompat.RequestPermissions (this, new [] { Manifest.Permission.ReadExternalStorage }, 1);
        }
        
        if (ContextCompat.CheckSelfPermission (this, Manifest.Permission.WriteExternalStorage) != Permission.Granted)
        {
            ActivityCompat.RequestPermissions (this, new [] { Manifest.Permission.WriteExternalStorage }, 1);
        }
        
        if (ContextCompat.CheckSelfPermission (this, Manifest.Permission.ManageExternalStorage) != Permission.Granted)
        {
            ActivityCompat.RequestPermissions (this, new [] { Manifest.Permission.ManageExternalStorage }, 1);
        }
        
        if (ContextCompat.CheckSelfPermission (this, Manifest.Permission.ModifyAudioSettings) != Permission.Granted)
        {
            ActivityCompat.RequestPermissions (this, new [] { Manifest.Permission.ModifyAudioSettings }, 1);
        }
        
        return base.CustomizeAppBuilder(builder)
            .WithInterFont()
            .UseReactiveUI();
    }

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
    }

    protected override void OnResume()
    {
        base.OnResume();

        StartActivity(new Intent(Application.Context, typeof(MainActivity)));
    }
}
