using System;
using System.Threading.Tasks;
using ApparitionsAndroidClient.Models;
using ApparitionsAndroidClient.Utilities;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices.Sensors;

namespace ApparitionsAndroidClient.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private int _count = 0;
    
    [ObservableProperty]
    private string _greeting = "Welcome to Avalonia!";

    private string _gpsLocation = "No location yet";
    
    [ObservableProperty]
    private string _updateTest = "Will Update";

    private string _locationRequestSuccess = "No location yet";
    
    private Location _currentLocation = new (0, 0);
    private Location _gpsByShop = new (28.594340, -81.381630);
    
    // public async Task<Location> GetCurrentLocation()
    // {
    //     try
    //     {
    //         var request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(10));
    //         var location = await Geolocation.Default.GetLocationAsync(request);
    //
    //         if (location != null)
    //         {
    //             Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}");
    //             return location;
    //         }
    //     }
    //     catch (FeatureNotSupportedException fnsEx) { /* Handle not supported */ }
    //     catch (PermissionException pEx) { /* Handle permission denied */ }
    //     catch (Exception ex) { /* Unable to get location */ }
    //
    //     return null;
    // }

    
    
    [RelayCommand]
    private async Task onViewLoaded(object sender)
    {
        await InitializeLocationListener();
        
        while (true)
        {
            
            await Dispatcher.UIThread.Invoke(uiThreadWork);
        }
        
        
     
        // ReSharper disable once FunctionNeverReturns
    }

    private async Task uiThreadWork()
    {
        // Greeting = _gpsLocation;
        await Geolocation_LocationChanged();
        await Task.Delay(1000);
    }
    
    // private async Task nonUiThreadWork()
    // {
    //     var rawLocation = await Geolocation.GetLocationAsync();
    //     
    //     if (rawLocation is null) throw new NullReferenceException("rawLocation is null");
    //     
    //     _currentLocation = new Location(rawLocation.Latitude, rawLocation.Longitude);
    //     
    //     
    //     
    //     _count++;
    //     
    //     await Task.Delay(2000);
    // }
    
    private async Task InitializeLocationListener()
    {
        // Geolocation.LocationChanged += Geolocation_LocationChanged;
        
        // Using GeolocationAccuracy.Medium as a balance between accuracy and power consumption. Developers can adjust this value to High or Low based on their specific requirements.
        var request = new GeolocationListeningRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(1));
        var success = await Geolocation.StartListeningForegroundAsync(request);
        var status = "Attempting to start listening";

        UpdateTest = success.ToString();

//          if (success)
//          {
//              _gpsLocation = $"""
//                              Shop Lat: {_gpsByShop.Latitude}, 
//                              Shop Long: {_gpsByShop.Longitude}
//
//                              Lat: {_currentLocation.Latitude}, 
//                              Long: {_currentLocation.Longitude}
//
//                              Calculated distance:
//                              {Location.CalculateDistance(_currentLocation, _gpsByShop, DistanceUnits.Kilometers)} km
//
//                              Count: 
//                              {_count}
//
//                              Success on listening init:
//                              {_locationRequestSuccess}
//                              """;
//              status =  "Success";
//          }
//          else
//          {
//              status = "Couldn't start listening.";
//          }
//          
         // _locationRequestSuccess = status;
    }

    async Task Geolocation_LocationChanged()
    {
        var currentLocation = await Geolocation.GetLastKnownLocationAsync();

        if (currentLocation is null)
        {
            throw new NullReferenceException();
        }
        
        Greeting = $"""
                        Shop Lat: {_gpsByShop.Latitude}, 
                        Shop Long: {_gpsByShop.Longitude}

                        Lat: {currentLocation.Latitude}, 
                        Long: {currentLocation.Longitude}

                        Calculated distance:
                        {Location.CalculateDistance(currentLocation, _gpsByShop, DistanceUnits.Kilometers)} km

                        Count: 
                        {_count++}

                        Success on listening init:
                        {_locationRequestSuccess}
                        """;
    }
}