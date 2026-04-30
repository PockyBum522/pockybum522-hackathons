using System.Threading.Tasks;
using ApparitionsClient.Services;
using System;
using System.IO;
using Android.Media;
using Avalonia.Platform;

namespace ApparitionsClient.Android.Services;

public class AndroidAudioPlayerService : IAudioPlayerService
{
    private MediaPlayer? _player;
    private float _volume = 1.0f;
    
    public bool IsPlaying  => _player?.IsPlaying ?? false;

    public float Volume {
        get => _volume;
        set
        {
            _volume = value; // probably need to do some kind of finessing on the float value for volume
                             // (i.e., ensure that it is definitely between 0 and 1)
            _player?.SetVolume(_volume, _volume); // SetVolume has left and right volume channels. Something
                                                  // to consider for later.
        }
    }

    /// Loads an audio file for playback and prepares the audio player for playing the specified audio track.
    /// This method is asynchronous and ensures that any currently loaded audio is stopped and resources are released
    /// before loading the new audio file.
    /// <param name="audioFileName">The name of the audio file to load. The file must exist and be accessible by the player.</param>
    /// <returns>A task that represents the asynchronous load operation.</returns>
    public async Task LoadAsync(string audioFileName)
    {
        // stop the player first before trying to load new audio.
        await StopAsync();
        
        var uri = new Uri($"avares://ApparitionsClient/Assets/Audio/{audioFileName}");
     
        await using var stream = AssetLoader.Open(uri);
        
        var cacheDir = global::Android.App.Application.Context.CacheDir;
        
        if (cacheDir is null) throw new NullReferenceException("Android cache directory was empty");
        
        var tempFile = Path.Combine(cacheDir.AbsolutePath, audioFileName);
        
        await using (var fileStream = File.Create(tempFile))
        {
            await stream.CopyToAsync(fileStream);
        }
        
        _player = new MediaPlayer();
        
        await _player.SetDataSourceAsync(tempFile);
        
        _player.Prepare();
        
        _player.SetVolume(_volume, _volume);
    }

    /// Starts playback of the currently loaded audio file.
    /// This method ensures that playback begins only if a valid audio file is loaded
    /// and the player is not already playing. If no audio file has been loaded, an exception is thrown.
    /// <returns>A task that represents the asynchronous play operation.</returns>
    public Task PlayAsync()
    {
        if (_player is null)
            throw new InvalidOperationException("No audio file loaded");
        
        if (!_player.IsPlaying)
            _player.Start();
        
        return Task.CompletedTask;
    }

    /// Stops the audio playback if it is currently playing and releases resources associated with the audio player.
    /// This method is asynchronous and ensures any currently playing audio is stopped.
    /// It also cleans up any internal state or resources used by the player to prepare
    /// for a future playback session.
    /// <returns>A task that represents the asynchronous stop operation.</returns>
    public Task StopAsync()
    {
        if (_player is not null)
        {
            try
            {
                if (_player.IsPlaying) _player.Stop();
                _player.Reset();
                _player.Release();
                _player.Dispose();
            }
            finally
            {
                _player = null;
            }
        }
        
        return Task.CompletedTask;
    }

    /// Pauses the playback of the currently playing audio file.
    /// This method pauses playback only if a valid audio file is loaded and is currently playing.
    /// If no audio is playing, it does nothing.
    /// <returns>A task that represents the asynchronous pause operation.</returns>
    public Task PauseAsync()
    {
        if (_player?.IsPlaying == true)
            _player.Pause();
        
        return Task.CompletedTask;
    }

    /// Sets the volume for the audio player.
    /// This method updates the playback volume and ensures that future playback operates at the specified level.
    /// <param name="volume">The desired volume level as a float. The value should be between 0.0 (muted) and 1.0 (full volume).</param>
    /// <returns>A task that represents the asynchronous operation to set the volume.</returns>
    public Task SetVolumeAsync(float volume)
    {
        Volume = volume;
        return Task.CompletedTask;
    }
}