using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using Android.Media;
using ApparitionsAndroidClient.Models;
using Avalonia.Platform;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Xna.Framework.Media;
using MediaPlayer = Android.Media.MediaPlayer;

namespace ApparitionsAndroidClient.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    
    
    
    
    
    [ObservableProperty]
    private bool _scrollingTextControlsVisible;     // See constructor to set these

    

    [ObservableProperty]
    private string _gpsInfo = "No GPS fix acquired yet";
    
    [ObservableProperty]
    private string _locationListenerStatus = "GPS location listener not started";

    [ObservableProperty] private string _scrollingText = "Vcon not loaded";
        
    [ObservableProperty]
    private int _topScrollValue = 920;
    
    private float _currentPlayerVolume = 1.0f;

    // Destination GPS coordinates handling
    private static double _latFromVcon;
    private static double _lonFromVcon;
    
    [ObservableProperty]
    private bool _debugControlsVisible;             // See constructor to set these
    
    private Location _currentDestinationCoordinates = new();
    
    private static string _audioFilename = "test_sound.mp3";
    
    private int _count;
    
    private MediaPlayer? _player;
    private int _gpsLoopCount;

    // To keep it simple for now the values will error out if the OS is anything but Android. iOS/Desktop/Browser support
    // will be implemented later.
    #if !ANDROID
        [RelayCommand]
        private Task onViewLoaded(object sender)
        {
            DebugControlsVisible = false;
            ScrollingTextControlsVisible = true;
            GpsInfo = "Android GSP/audio behavior is not available in the Desktop version.";
            LocationListenerStatus = "Desktop version loaded.";
            ScrollingText = "Desktop version loaded.";
            
            return Task.CompletedTask;
        }

        [RelayCommand]
        private Task UpdateLocation()
        {
            GpsInfo = "Android GSP/audio behavior is not yet available in a non-Android version.";
            return Task.CompletedTask;
        }

        // Sound will not play on a non-Android version (for now)
        [RelayCommand]
        private Task playSound(object sender)
        {
            return Task.CompletedTask;
        }

        [RelayCommand]
        private Task stopSoundTest(object sender)
        {
            return Task.CompletedTask;
        }
        
    #endif
    
    
    
}

/**

 private static VconRoot _currentVcon = new();
    private static List<VconRoot> _eventSequence = [];
    private bool _forceTextScrolling;

    [RelayCommand, SupportedOSPlatform("Android")]
    private async Task onViewLoaded(object sender)
    {
        _eventSequence.Add(DemoVcons.GaryGrandfatherTreeByGarageVcon);
        _eventSequence.Add(DemoVcons.LarryVcon);
        
        // Set the current Vcon to whichever event we want to test with
        _currentVcon = _eventSequence[1];
        
        _forceTextScrolling = true;
        
        // ENABLE/DISABLE DEBUG CONTROLS HERE
        DebugControlsVisible = false;
        
        
        // This one sets automatically, just change the above line
        ScrollingTextControlsVisible = !DebugControlsVisible;

        attachVconContentToUiControls();

        await InitializeLocationListener();

        // Start playing for demos
        await playSound(new object());
        
        while (true)
        {
            await Dispatcher.UIThread.Invoke(uiThreadWork);
        }
        
        // ReSharper disable once FunctionNeverReturns because it's not supposed to
    }

    private void attachVconContentToUiControls()
    {
        // Attach vcon content to controls
        ScrollingText = _currentVcon.Dialog.First().Body;
        
        _latFromVcon = Double.Parse(_currentVcon.Attachments.First().Body[0]);
        _lonFromVcon = Double.Parse(_currentVcon.Attachments.First().Body[1]);

        _currentDestinationCoordinates = new(_latFromVcon, _lonFromVcon);

        _audioFilename = _currentVcon.Attachments.First().Body[2];
    }

    [SupportedOSPlatform("Android")]
    private async Task uiThreadWork()
    {
        if (_gpsLoopCount++ > 20)
        {
            await UpdateLocation();

            _gpsLoopCount = 0;
        }

        if (_forceTextScrolling)
            _currentPlayerVolume = 1f;
        
        if (_currentPlayerVolume > 0)
        {
            if (!_player?.IsPlaying ?? true) _player?.Start();
            
            TopScrollValue -= 1;
        }
        else
        {
            if (_player?.IsPlaying ?? false) _player?.Pause();
        }
        
        await Task.Delay(50);
    }
    
    private async Task InitializeLocationListener()
    {
        var request = new GeolocationListeningRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(1));
        var success = await Geolocation.StartListeningForegroundAsync(request);

        LocationListenerStatus = $"Location listening start success: {success.ToString()}";
    }

    [RelayCommand, SupportedOSPlatform("Android")]
    async Task UpdateLocation()
    {
        var currentLocation = await Geolocation.GetLastKnownLocationAsync();

        if (currentLocation is null)
        {
            throw new NullReferenceException();
        }

        var metersAway = Location.CalculateDistance(currentLocation, _currentDestinationCoordinates, DistanceUnits.Kilometers) * 1000;

        _currentPlayerVolume = (float)Map((decimal)metersAway, 11, 1, 0, 0.8m);

        if (_currentPlayerVolume < 0) _currentPlayerVolume = 0;
        if (_currentPlayerVolume > 1) _currentPlayerVolume = 1f;
        
        GpsInfo = $"""
                    Shop Lat: {_currentDestinationCoordinates.Latitude}, 
                    Shop Long: {_currentDestinationCoordinates.Longitude}

                    Lat: {currentLocation.Latitude}, 
                    Long: {currentLocation.Longitude}

                    Calculated distance: {metersAway:F1} meters

                    Count: {_count++}
                    
                    Player IsPlaying: {_player?.IsPlaying}
                    _currentPlayerVolume: {_currentPlayerVolume}
                    """;
        
        if (_player is null) return;
        
        if (_forceTextScrolling)
            _currentPlayerVolume = 1f;
        
        if (_player.IsPlaying)
        {
            _player.SetVolume(_currentPlayerVolume, _currentPlayerVolume);
        }
    }

    private static decimal Map(decimal value, decimal fromSource, decimal toSource, decimal fromTarget, decimal toTarget)
    {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }
    
    [RelayCommand, SupportedOSPlatform("Android")]
    private async Task playSound(object sender)
    {
        // Copy asset stream to a temp file
        var uri = new Uri($"avares://ApparitionsAndroidClient/Assets/Audio/{_audioFilename}");
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

    [RelayCommand, SupportedOSPlatform("Android")]
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
    }*/