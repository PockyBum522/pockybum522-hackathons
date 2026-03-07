using System;
using System.Threading.Tasks;
using ApparitionsAndroidClient.Models;
using ApparitionsAndroidClient.Utilities;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Devices.Sensors;

namespace ApparitionsAndroidClient.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private int _count = 0;
    
    [ObservableProperty]
    private string _greeting = "Welcome to Avalonia!";

    private string _gpsLocation = "No location yet";
    private string _locationRequestSuccess = "No location yet";
    
    private GpsCoordinates _currentLocation = new(0, 0);
    private GpsCoordinates _gpsByShop = new(28.594340, -81.381630);
    
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
    
    void Geolocation_LocationChanged(object? sender, GeolocationLocationChangedEventArgs e)
    {
        _currentLocation = new GpsCoordinates(e.Location.Latitude, e.Location.Longitude);
    }
}
