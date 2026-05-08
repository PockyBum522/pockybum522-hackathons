namespace ApparitionsClient.StateMachines.AudioPlayer;

public enum AudioPlayerTrigger
{
    InitializeRequested,
    InitializeSucceeded,
    InitializeError,
    LoadRequested,
    LoadSucceeded,
    LoadError,
    PlayRequested,
    PlaySucceeded,
    PlayError,
    PauseRequested,
    PauseSucceeded,
    PauseError,
    StopRequested,
    StopSucceeded,
    StopError,
}