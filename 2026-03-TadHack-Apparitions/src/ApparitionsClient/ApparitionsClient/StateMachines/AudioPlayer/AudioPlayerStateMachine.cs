using System;
using System.Threading.Tasks;
using Stateless;

namespace ApparitionsClient.StateMachines.AudioPlayer;

public class AudioPlayerStateMachine : IAudioPlayerStateMachine
{
    private readonly StateMachine<AudioPlayerState, AudioPlayerTrigger> _machine;
    
    public AudioPlayerStateMachine(AudioPlayerState initialState = AudioPlayerState.Uninitialized)
    {
        _machine = new StateMachine<AudioPlayerState, AudioPlayerTrigger>(initialState);

        Configure();
    }

    public AudioPlayerState State => _machine.State;

    public event Action<AudioPlayerState, AudioPlayerTrigger> OnStateChanged;
    
    public bool CanFire(AudioPlayerTrigger trigger) => _machine.CanFire(trigger);
    
    public void Fire(AudioPlayerTrigger trigger)
    {
        _machine.Fire(trigger);
    }
    
    private void Configure()
    {
        _machine.Configure(AudioPlayerState.Uninitialized)
            .Permit(AudioPlayerTrigger.InitializeRequested, AudioPlayerState.Initialized)
            .Permit(AudioPlayerTrigger.InitializeError, AudioPlayerState.Error);
        
        _machine.Configure(AudioPlayerState.Initialized)
            .Permit(AudioPlayerTrigger.StopRequested, AudioPlayerState.Stopped);
        
        _machine.Configure(AudioPlayerState.Stopped)
            .Permit(AudioPlayerTrigger.LoadRequested, AudioPlayerState.Loading);
        
        _machine.Configure(AudioPlayerState.Loading)
            .Permit(AudioPlayerTrigger.LoadSucceeded, AudioPlayerState.Playing)
            .Permit(AudioPlayerTrigger.LoadError, AudioPlayerState.Error)
            .Permit(AudioPlayerTrigger.StopRequested, AudioPlayerState.Stopped);

        _machine.Configure(AudioPlayerState.Ready)
            .Permit(AudioPlayerTrigger.PlayRequested, AudioPlayerState.Playing)
            .Permit(AudioPlayerTrigger.StopRequested, AudioPlayerState.Stopped);
        
        _machine.Configure(AudioPlayerState.Playing)
            .Permit(AudioPlayerTrigger.PauseRequested, AudioPlayerState.Paused)
            .Permit(AudioPlayerTrigger.StopRequested, AudioPlayerState.Stopped)
            .Permit(AudioPlayerTrigger.PlayError, AudioPlayerState.Error);
        
        _machine.Configure(AudioPlayerState.Paused)
            .Permit(AudioPlayerTrigger.PlayRequested, AudioPlayerState.Playing)
            .Permit(AudioPlayerTrigger.StopRequested, AudioPlayerState.Stopped);
        
        _machine.Configure(AudioPlayerState.Error)
            .Permit(AudioPlayerTrigger.LoadRequested, AudioPlayerState.Loading)
            .Permit(AudioPlayerTrigger.StopRequested, AudioPlayerState.Stopped);
        
        _machine.OnTransitioned(transition => OnStateChanged?.Invoke(transition.Destination, transition.Trigger));
    }
}