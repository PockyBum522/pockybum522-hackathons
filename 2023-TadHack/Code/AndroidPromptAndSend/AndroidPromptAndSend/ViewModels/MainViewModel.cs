using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.AudioRecorder;

namespace AndroidPromptAndSend.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public string Greeting => "Welcome to Avalonia!";
    
    [ObservableProperty]
    private string _promptText = "What was your favorite music when you were 17?";

    [RelayCommand]
    private async Task RecordResponse()
    {
        // Microphone permissions are in AndroidPromptAndSend.Android under SplashActivity.cs
        
        await PromptUserForPathAndStartRecording();
    }

    private async Task PromptUserForPathAndStartRecording()
    {
        var appLifetime = Avalonia.Application.Current!.ApplicationLifetime as ISingleViewApplicationLifetime;
        
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = TopLevel.GetTopLevel(appLifetime!.MainView);

        // Start async operation to open the dialog.
        var file = await topLevel!.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save Text File"
        });

        if (file is not null)
        {
            // Open writing stream from the file.
            await using var stream = await file.OpenWriteAsync();
            // using var streamWriter = new StreamWriter(stream);
            
            // Write some content to the file.
            // await streamWriter.WriteLineAsync($"LocalPath: {file.Path.LocalPath}");
            // await streamWriter.WriteLineAsync($"AbsolutePath: {file.Path.AbsolutePath}");
            // await streamWriter.WriteLineAsync($"Authority: {file.Path.Authority}");
            // await streamWriter.WriteLineAsync($"Fragment: {file.Path.Fragment}");
            // await streamWriter.WriteLineAsync($"Scheme: {file.Path.Scheme}");
            // await streamWriter.WriteLineAsync($"AbsoluteUri: {file.Path.AbsoluteUri}");
            // await streamWriter.WriteLineAsync($"OriginalString: {file.Path.OriginalString}");
            // await streamWriter.WriteLineAsync($"IsAbsoluteUri: {file.Path.IsAbsoluteUri}");
            // await streamWriter.WriteLineAsync($"PathAndQuery: {file.Path.PathAndQuery}");

            // await StartAudioRecording(file.Path.LocalPath);      //System.IO.DirectoryNotFoundException: Could not find a part of the path '/document/45'.
            // await StartAudioRecording(file.Path.AbsoluteUri);    //System.IO.DirectoryNotFoundException: Could not find a part of the path '/content:/com.android.providers.downloads.documents/document/46'.

            //var trimmedUri = file.Path.AbsoluteUri.Substring(8); // Trim leading 'content:'
            await StartAudioRecording(stream);   
        }
    }

    private async Task StartAudioRecording(Stream fileStream)
    {
        //PromptText = "Thank you for your response! It will be available shortly, at: http://website.com/dreamages";
        PromptText = "Recording now...";
        
        var recorder = new AudioRecorderService  
        {  
            StopRecordingOnSilence = false, //will stop recording after 2 seconds (default)  
            StopRecordingAfterTimeout = true,  //stop recording after a max timeout (defined below)  
            TotalAudioTimeout = TimeSpan.FromSeconds(5) //audio will stop recording after 
        };
        
        await recorder.StartRecording();

        await using var streamWriter = new StreamWriter(recorder.GetAudioFileStream());
        
        streamWriter.Close();
    }
}
