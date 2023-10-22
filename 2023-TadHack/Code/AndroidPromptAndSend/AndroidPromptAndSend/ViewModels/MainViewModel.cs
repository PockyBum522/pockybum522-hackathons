using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AndroidPromptAndSend.Views;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.AudioRecorder;
using RestSharp;

namespace AndroidPromptAndSend.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly MainView? _myParentMainView;
    
    [ObservableProperty] private bool _welcomeControlsVisible = true;
    [ObservableProperty] private bool _instructionsMessageControlsVisible;
    [ObservableProperty] private bool _promptControlsVisible;
    [ObservableProperty] private bool _confirmationControlsVisible;

    [ObservableProperty] private string _promptText = "";
    [ObservableProperty] private string _userResponseText = "";

    private List<string> _prompts = new()
    {
        "What was your favorite song or artist when you were a teenager and why?",
        "What is your favorite comfort food and why?",
        "What is your favorite beverage and why?",
        "Describe your favorite hobby.",
        "Describe your favorite place on earth.",
        "Describe your dream vacation.",
        "Describe your favorite animal.",
        "Describe your favorite book.",
        "Describe your favorite halloween costume.",
        "What is your favorite color and why?",
        "What was your favorite toy as a child?"
    };

    public MainViewModel(MainView mainView)
    {
        _myParentMainView = mainView;
    }

    // Just here so we can preview the UI when working on a computer, since program will only ever actually run on android, this will never run
    public MainViewModel()
    {
        
    }

    // UI Controls visibility set methods to move through the UI states
    
    [RelayCommand]
    private void ShowInstructionsMessage()
    {
        WelcomeControlsVisible = false;
        InstructionsMessageControlsVisible = true;

        SetRandomPromptText();
    }

    private void SetRandomPromptText()
    {
        var oldPromptText = PromptText;

        do
        {
            // Set up the prompt for later
            var random = new Random(
                int.Parse(
                    DateTime.Now.ToString("fffff"))); // Milliseconds for seed

            var randomPromptNumber = random.Next(0, _prompts.Count);

            PromptText = _prompts[randomPromptNumber];
        } 
        while (PromptText == oldPromptText);
        // If we're using the change prompt button, this makes sure it doesn't randomly set the next prompt to the old one
    }

    [RelayCommand]
    private void TryAnotherPrompt()
    {
        SetRandomPromptText();
    }
    
    [RelayCommand]
    private void ShowPrompt()
    {
        InstructionsMessageControlsVisible = false;
        PromptControlsVisible = true;

        _myParentMainView?.UserResponseTextBox.Focus();
    }
    
    [RelayCommand]
    private void ClearResponse()
    {
        UserResponseText = "";
    }

    [RelayCommand]
    private async Task SendResponse()
    {
        // Make sure they're not accidentally hitting send before filling content
        if (UserResponseText.Length < 1) return;
        
        PromptControlsVisible = false;
        ConfirmationControlsVisible = true;
        
        // SEND RESPONSE TO STACUITY HERE
        await SendResponseToStacuity($"{PromptText}---{UserResponseText}");    
        
        UserResponseText = "";
    }

    private async Task SendResponseToStacuity(string userResponseText)
    {
        // Set up random number for key name
        var random = new Random(
            int.Parse(
                DateTime.Now.ToString("fffff")));  // Milliseconds for seed
        
        var randomPromptNumber = random.Next(0, 1999999999);

        var urlSafeUserResponse = HttpUtility.UrlEncode(userResponseText);
        
        // TEMPORARY

        // var debugRequestMessage = $"Request built url: {requestUrl}" + Environment.NewLine;
        // debugRequestMessage += $"Built userResponseText: {urlSafeUserResponse}";

        // UserResponseText = debugRequestMessage;
        // END TEMPORARY
        
        var options = new RestClientOptions($"http://edge.stacuity.io/ep/kv/usr_resp_{randomPromptNumber}");
        var client = new RestClient(options);
        var request = new RestRequest("");
        request.AddStringBody("\"" + urlSafeUserResponse + "\"", "application/*+json");
        var response = await client.PostAsync(request);

        UserResponseText = response.Content;
    }
        
        
        

    [RelayCommand]
    private void ResetToStart()
    {
        ConfirmationControlsVisible = false;
        WelcomeControlsVisible = true;
    }
    
    

    
}
