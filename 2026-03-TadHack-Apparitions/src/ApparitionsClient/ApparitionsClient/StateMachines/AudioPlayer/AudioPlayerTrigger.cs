namespace ApparitionsClient.StateMachines.AudioPlayer;

public enum AudioPlayerTrigger
{
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