namespace ApparitionsClient.StateMachines.AudioPlayer;
public enum AudioPlayerState
{
    Uninitialized,
    Initializing,
    Initialized,
    Stopped,
    Loading,
    Ready,
    Playing,
    Paused,
    Error
}
