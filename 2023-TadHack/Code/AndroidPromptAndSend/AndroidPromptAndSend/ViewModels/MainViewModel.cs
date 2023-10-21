using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AndroidPromptAndSend.Views;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.AudioRecorder;

namespace AndroidPromptAndSend.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly MainView _myParentMainView;
    
    [ObservableProperty] private bool _welcomeControlsVisible = true;
    [ObservableProperty] private bool _instructionsMessageControlsVisible;
    [ObservableProperty] private bool _promptControlsVisible;
    [ObservableProperty] private bool _confirmationControlsVisible;

    [ObservableProperty]
    private string _promptText = "";

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

    // UI Controls visibility set methods to move through the UI states
    
    [RelayCommand]
    private void ShowInstructionsMessage()
    {
        WelcomeControlsVisible = false;
        InstructionsMessageControlsVisible = true;
        
        // Set up the prompt for later
        var random = new Random(
            int.Parse(
                DateTime.Now.ToString("ffff")));  // Milliseconds for seed
        
        var randomPromptNumber = random.Next(0, _prompts.Count);

        PromptText = _prompts[randomPromptNumber];
    }
    
    [RelayCommand]
    private void ShowPrompt()
    {
        InstructionsMessageControlsVisible = false;
        PromptControlsVisible = true;

        _myParentMainView.UserResponseTextBox.Focus();
    }
    
    [RelayCommand]
    private void ClearResponse()
    {
        
    }

    [RelayCommand]
    private void SendResponse()
    {
        PromptControlsVisible = false;
        ConfirmationControlsVisible = true;
    }
    
    [RelayCommand]
    private void ResetToStart()
    {
        ConfirmationControlsVisible = false;
        WelcomeControlsVisible = true;
    }
    
    

    
}
