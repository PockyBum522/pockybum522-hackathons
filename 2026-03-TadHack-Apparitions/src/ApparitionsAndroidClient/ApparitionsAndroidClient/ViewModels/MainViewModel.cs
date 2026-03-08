using System;
using System.IO;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using Android.Media;
using Avalonia.Platform;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Devices.Sensors;

namespace ApparitionsAndroidClient.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _gpsInfo = "No GPS fix acquired yet";
    
    [ObservableProperty]
    private string _locationListenerStatus = "GPS location listener not started";
    
    private readonly Location _gpsByShop = new (28.594340, -81.381630);
    
    private int _count;
    
    private MediaPlayer? _player;
    
    [RelayCommand]
    private async Task onViewLoaded(object sender)
    {
        await InitializeLocationListener();
        
        while (true)
        {
            await Dispatcher.UIThread.Invoke(uiThreadWork);
        }
        
        // ReSharper disable once FunctionNeverReturns because it's not supposed to
    }

    private async Task uiThreadWork()
    {
        await UpdateLocation();
        
        await Task.Delay(1000);
    }
    
    private async Task InitializeLocationListener()
    {
        var request = new GeolocationListeningRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(1));
        var success = await Geolocation.StartListeningForegroundAsync(request);

        LocationListenerStatus = $"Location listening start success: {success.ToString()}";
    }

    async Task UpdateLocation()
    {
        var currentLocation = await Geolocation.GetLastKnownLocationAsync();

        if (currentLocation is null)
        {
            throw new NullReferenceException();
        }

        var metersAway = Location.CalculateDistance(currentLocation, _gpsByShop, DistanceUnits.Kilometers) / 1000;
        
        GpsInfo = $"""
                        Shop Lat: {_gpsByShop.Latitude}, 
                        Shop Long: {_gpsByShop.Longitude}

                        Lat: {currentLocation.Latitude}, 
                        Long: {currentLocation.Longitude}

                        Calculated distance:
                        {metersAway:F1} meters

                        Count: 
                        {_count++}
                        """;
    }
    
    [RelayCommand, SupportedOSPlatform("Android")]
    private async Task playSoundTest(object sender)
    {
        // Copy asset stream to a temp file
        var uri = new Uri("avares://ApparitionsAndroidClient/Assets/test_sound.mp3");
        await using var stream = AssetLoader.Open(uri);

        // Use Android's native cache dir instead of FileSystem.CacheDirectory
        var cacheDir = Android.App.Application.Context.CacheDir!.AbsolutePath;
        var tempFile = Path.Combine(cacheDir, "temp_audio.mp3");

        await using (var fileStream = File.Create(tempFile))
        {
            await stream.CopyToAsync(fileStream);
        }

        _player?.Stop();
        _player?.Reset();
        _player?.Release();

        _player = new MediaPlayer();
        
        await _player.SetDataSourceAsync(tempFile);
        
        // ReSharper disable once MethodHasAsyncOverload because it's lies. It breaks things if you use the Async.
        _player.Prepare();
        
        _player.Start();

        var counter = 100;
        while (counter-- > 0)
        {
            for (float i = 0; i < 1; i += 0.01f)
            {
                await Task.Delay(20);
        
                _player.SetVolume(i, i);
            }
        
            for (float i = 1; i > 0; i -= 0.01f)
            {
                await Task.Delay(20);
        
                _player.SetVolume(i, i);
            }    
        }
    }
}