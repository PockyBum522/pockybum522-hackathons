using System.Threading.Tasks;
using ApparitionsClient.Services;

namespace ApparitionsClient.Desktop.Services;

public class DesktopAudioPlayerService : IAudioPlayerService
{
    public bool IsPlaying => false;

    public float Volume => 0f;
    
    public Task LoadAsync(string audioFilename)
    {
        return Task.CompletedTask;
    }
    
    public Task PlayAsync()
    {
        return Task.CompletedTask;
    }
    
    public Task PauseAsync()
    {
        return Task.CompletedTask;
    }
    
    public Task StopAsync()
    {
        return Task.CompletedTask;
    }
    
    public Task SetVolumeAsync(float volume)
    {
        return Task.CompletedTask;
    }
}