using System.Runtime.Versioning;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Aoc2023CSharp.ApplicationLogistics;
using Aoc2023CSharp.ViewModels;
using Aoc2023CSharp.Views;

namespace Aoc2023CSharp;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    [SupportedOSPlatform("windows")]
    public override void OnFrameworkInitializationCompleted()
    {
        // Dependency Injection:
        var loggerConfiguration = LoggerSetup.ConfigureLogger();

        // ReSharper disable once ConvertIfStatementToSwitchStatement because I don't know that it's actually cleaner
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel(loggerConfiguration)
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel(loggerConfiguration)
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}