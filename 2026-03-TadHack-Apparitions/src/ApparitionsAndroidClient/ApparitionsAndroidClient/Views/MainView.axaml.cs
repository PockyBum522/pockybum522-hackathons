using System;
using ApparitionsAndroidClient.ViewModels;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace ApparitionsAndroidClient.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        MainViewModel? myViewModel = (MainViewModel)DataContext;

        if (myViewModel is null) throw new NullReferenceException("Mainviewmodel be screwed up yo");
        
        myViewModel.onViewLoadedCommand.Execute(null);
    }
}