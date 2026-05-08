using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace ApparitionsClient.Services;

public interface IAudioPlayerService
{
    Task LoadAsync(string audioFileName);

    Task PlayAsync();

    Task PauseAsync();

    Task StopAsync();

    Task SetVolumeAsync(float volume);
}