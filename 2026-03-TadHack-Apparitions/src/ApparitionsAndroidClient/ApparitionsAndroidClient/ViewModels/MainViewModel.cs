using System;
using System.Threading.Tasks;
using ApparitionsAndroidClient.Models;
using ApparitionsAndroidClient.Utilities;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Devices.Sensors;
using System.IO;
using System.Runtime.Versioning;
using Android.Media;
using Avalonia.Platform;


namespace ApparitionsAndroidClient.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private int _count;
    
    [ObservableProperty]
    private string _greeting = "Welcome to Avalonia!";
    
    [ObservableProperty]
    private string _updateTestText = "No updatey :(";

    private string _gpsLocation = "No location yet";
    private string _locationRequestSuccess = "No location yet";
    
    private GpsCoordinates _currentLocation = new(0, 0);
    private readonly GpsCoordinates _gpsByShop = new(28.594340, -81.381630);
    private MediaPlayer? _player;

    [RelayCommand]
    private async Task onViewLoaded(object sender)
    {
        await InitializeLocationListener();
        
        while (true)
        {
            Dispatcher.UIThread.Invoke(uiThreadWork);

            await nonUiThreadWork();
        }
     
        // ReSharper disable once FunctionNeverReturns
    }

    private void uiThreadWork()
    {
        Greeting = _gpsLocation;
    }
    
    private async Task nonUiThreadWork()
    {
        var rawLocation = await Geolocation.GetLocationAsync();
        
        if (rawLocation is null) throw new NullReferenceException("rawLocation is null");
        
        _currentLocation = new GpsCoordinates(rawLocation.Latitude, rawLocation.Longitude);
        
        _gpsLocation = $"""
                        Lat: {_currentLocation.Latitude}, 
                        Long: {_currentLocation.Longitude}

                        Calculated distance:
                        {GpsCoordinateDistanceCalculator.GetDistance(_currentLocation, _gpsByShop, 'F')} feet

                        Count: 
                        {_count}

                        Success on listening init:
                        {_locationRequestSuccess}
                        """;
        
        _count++;
        
        await Task.Delay(2000);
    }
    
    private async Task InitializeLocationListener()
    {
        //Geolocation.LocationChanged += Geolocation_LocationChanged;
        
        // Using GeolocationAccuracy.Medium as a balance between accuracy and power consumption. Developers can adjust this value to High or Low based on their specific requirements.
        var request = new GeolocationListeningRequest(GeolocationAccuracy.Best);
        var success = await Geolocation.StartListeningForegroundAsync(request);

        string status = success
            ? "Started listening for foreground location"
            : "Couldn't start listening";
        
        _locationRequestSuccess = status;
    }

    [RelayCommand, SupportedOSPlatform("Android")]
    private async Task updateTest(object sender)
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
