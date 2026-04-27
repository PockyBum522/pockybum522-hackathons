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