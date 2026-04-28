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
        var myViewModel = (MainViewModel?)DataContext;

        if (myViewModel is null) throw new NullReferenceException("Mainviewmodel be screwed up yo");
        
        //myViewModel.onViewLoadedCommand.Execute(null);
    }
}
