using System;
using System.Threading.Tasks;
using Avalonia.Interactivity;
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
    
    private string _gpsLocation = "Welcome to Avalonia!";
    
    [RelayCommand]
    private async Task onViewLoaded(object sender)
    {
        while (true)
        {
            Dispatcher.UIThread.Invoke(uiThreadWork);

            await nonUiThreadWork();
        }
    }

    private void uiThreadWork()
    {
        _count++;
        
        // Greeting = $"Now we have loaded: #{_count}";
        Greeting = _gpsLocation + " - " + _count;
    }
    
    private async Task nonUiThreadWork()
    {
        _gpsLocation = await GetCachedLocation();
        
        await Task.Delay(1500);
    }

    public static async Task<string> GetCachedLocation()
    {
        var location = await Geolocation.Default.GetLocationAsync();

        if (location is null) throw new NullReferenceException("GPS location was null, abandon hope");
        
        return $"Latitude: {location.Latitude} {Environment.NewLine}Longitude: {location.Longitude} {Environment.NewLine}Altitude: {location.Altitude} {Environment.NewLine}";
    }
}
