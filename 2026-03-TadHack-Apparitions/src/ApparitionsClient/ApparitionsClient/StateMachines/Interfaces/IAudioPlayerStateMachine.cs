using System;

namespace ApparitionsClient.StateMachines.AudioPlayer;

public interface IAudioPlayerStateMachine
{
    AudioPlayerState State { get; }
    
    event Action<AudioPlayerState, AudioPlayerTrigger> OnStateChanged;
    
    bool CanFire(AudioPlayerTrigger trigger);
    
    void Fire(AudioPlayerTrigger trigger);
}