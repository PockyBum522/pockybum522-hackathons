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

[SupportedOSPlatform("Android")]
public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _gpsInfo = "No GPS fix acquired yet";
    
    [ObservableProperty]
    private string _locationListenerStatus = "GPS location listener not started";

    private float _currentPlayerVolume = 1.0f;

    private readonly Location _gpsByShop = new (28.594340, -81.381630);
    
    private int _count;
    
    private MediaPlayer? _player;

    private async Task onViewLoaded(object sender)
    {
        await InitializeLocationListener();
        
        while (true)
        {
            await Dispatcher.UIThread.Invoke(uiThreadWork);
        }
        
        // ReSharper disable once FunctionNeverReturns because it's not supposed to
    }

    [SupportedOSPlatform("Android")]
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

    [RelayCommand]
    async Task UpdateLocation()
    {
        var currentLocation = await Geolocation.GetLastKnownLocationAsync();

        if (currentLocation is null)
        {
            throw new NullReferenceException();
        }

        var metersAway = Location.CalculateDistance(currentLocation, _gpsByShop, DistanceUnits.Kilometers) * 1000;

        _currentPlayerVolume = (float)Map((decimal)metersAway, 11, 1, 0, 0.8m);

        if (_currentPlayerVolume < 0) _currentPlayerVolume = 0;
        if (_currentPlayerVolume > 1) _currentPlayerVolume = 1f;
        
        GpsInfo = $"""
                    Shop Lat: {_gpsByShop.Latitude}, 
                    Shop Long: {_gpsByShop.Longitude}

                    Lat: {currentLocation.Latitude}, 
                    Long: {currentLocation.Longitude}

                    Calculated distance: {metersAway:F1} meters

                    Count: {_count++}
                    
                    Player IsPlaying: {_player?.IsPlaying}
                    _currentPlayerVolume: {_currentPlayerVolume}
                    """;
        
        if (_player is null) return;

        if (_player.IsPlaying)
        {
            _player.SetVolume(_currentPlayerVolume, _currentPlayerVolume);
        }
    }

    private static decimal Map(decimal value, decimal fromSource, decimal toSource, decimal fromTarget, decimal toTarget)
    {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }
    
    [RelayCommand]
    private async Task playSoundTest(object sender)
    {
        // Copy asset stream to a temp file
        var uri = new Uri("avares://ApparitionsAndroidClient/Assets/Audio/test_sound.mp3");
        await using var stream = AssetLoader.Open(uri);

        // Use Android's native cache dir instead of FileSystem.CacheDirectory
        var cacheDir = Android.App.Application.Context.CacheDir!.AbsolutePath;
        var tempFile = Path.Combine(cacheDir, "temp_audio.mp3");

        await using (var fileStream = File.Create(tempFile))
        {
            await stream.CopyToAsync(fileStream);
        }

        // Reset player
        stopSoundTestCommand.Execute(null);
        
        if (_player is null) throw new NullReferenceException("Player was null");
        
        await _player.SetDataSourceAsync(tempFile);
        
        // ReSharper disable once MethodHasAsyncOverload because it's lies. It breaks things if you use the Async.
        _player.Prepare();
        
        _player.SetVolume(0f, 0f);
        
        _player.Start();
    }

    [RelayCommand]
    private Task stopSoundTest(object sender)
    {
        try
        {
            _player?.Stop();
            _player?.Reset();
            _player?.Release();

            _player = new MediaPlayer();
            
            return Task.CompletedTask;
        }
        catch (Exception exception)
        {
            return Task.FromException(exception);
        }
    }
}