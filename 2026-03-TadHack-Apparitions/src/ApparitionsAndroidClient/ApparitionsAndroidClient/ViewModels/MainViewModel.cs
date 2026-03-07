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
    
    private GpsCoordinates _currentLocation = new(0, 0);
    private GpsCoordinates _gpsByShop = new(28.594340, -81.381630);
    
    [RelayCommand]
    private async Task onViewLoaded(object sender)
    {
        while (true)
        {
            Dispatcher.UIThread.Invoke(uiThreadWork);

            await nonUiThreadWork();
        }
     
        // ReSharper disable once FunctionNeverReturns
    }

    private void uiThreadWork()
    {
        _count++;
        
        // Greeting = $"Now we have loaded: #{_count}";
        Greeting = _gpsLocation;
    }
    
    private async Task nonUiThreadWork()
    {
        _currentLocation = await GetLocation();
        
        _gpsLocation = $"""
                        Lat: {_currentLocation.Latitude}, 
                        Long: {_currentLocation.Longitude}

                        Calculated distance:
                        {GpsCoordinateDistanceCalculator.GetDistance(_currentLocation, _gpsByShop, 'F')} feet
                        """;
        
        await Task.Delay(1000);
    }
    
    private static async Task<GpsCoordinates> GetLocation()
    {
        var location = await Geolocation.GetLocationAsync();

        if (location is null) throw new NullReferenceException("GPS location was null, abandon hope");
        
        return new GpsCoordinates(location.Latitude, location.Longitude); 
    }
}
