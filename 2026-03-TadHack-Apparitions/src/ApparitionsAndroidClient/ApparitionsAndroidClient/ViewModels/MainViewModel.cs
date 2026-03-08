using System;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Devices.Sensors;

namespace ApparitionsAndroidClient.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private int _count;
    
    [ObservableProperty]
    private string _greeting = "Welcome to Avalonia!";
    
    [ObservableProperty]
    private string _updateTest = "Will Update";
    
    private readonly Location _gpsByShop = new (28.594340, -81.381630);
    
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

        UpdateTest = $"Location listening start success: {success.ToString()}";
    }

    async Task UpdateLocation()
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
                        """;
    }
}