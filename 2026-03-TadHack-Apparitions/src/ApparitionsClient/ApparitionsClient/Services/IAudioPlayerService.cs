using System;
using System.Threading.Tasks;

namespace ApparitionsClient.Services;

public interface IAudioPlayerService
{
    bool IsPlaying { get; }
    
    float Volume { get; }

    Task LoadAsync(string audioFileName);

    Task PlayAsync();

    Task PauseAsync();

    Task StopAsync();

    Task SetVolumeAsync(float volume);
}