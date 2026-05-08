using System;
using ApparitionsClient.ViewModels;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace ApparitionsClient.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        var mainViewModel = (MainViewModel?)DataContext;

        if (mainViewModel is null) throw new NullReferenceException("Mainviewmodel be screwed up yo");
        
        mainViewModel.MainViewLoadedCommand.Execute(null);
        
    }
}
