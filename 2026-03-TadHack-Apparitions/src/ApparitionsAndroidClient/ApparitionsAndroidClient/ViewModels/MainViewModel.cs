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
        
        Greeting = $"Now we have loaded: #{_count}";
    }
    
    private async Task nonUiThreadWork()
    {
        await Task.Delay(500);
    }

    public static async Task<string> GetCachedLocation()
    {
        var location = await Geolocation.Default.GetLastKnownLocationAsync();

        if (location != null)
            return $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}";
    
        // catch (FeatureNotSupportedException fnsEx)
        // {
        //     // Handle not supported on device exception
        // }
        // catch (FeatureNotEnabledException fneEx)
        // {
        //     // Handle not enabled on device exception
        // }
        // catch (PermissionException pEx)
        // {
        //     // Handle permission exception
        // }
        // catch (Exception ex)
        // {
        //     // Unable to get location
        // }

        return "None";
    }
}
