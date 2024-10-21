using System;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using ShrineServerAndGui.Models;
using ShrineServerAndGui.NfcRead;

namespace ShrineServerAndGui;

public partial class MainWindow : Window
{
    public static UserControl? CurrentView { get; set; }
    
    public MainWindow()
    {
        InitializeComponent();

        CurrentView = new LightsCoalescingView();
        
        StartMainWorkers();
    }

    private async Task StartMainWorkers()
    {
        /*
            After a reboot, or upon unplugging/replugging the NFC reader,
            you will need to run the following before using the NFC reader:

            sudo modprobe -r pn533_usb
            sudo modprobe -r pn533
        */
        
        Console.WriteLine("Please wait, your religious experience is loading...");
        
        Task.Run(() =>
        {
            try
            {
                StartNfcReadWatchLoop();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in StartNfcReadWatchLoop: {ex.Message}");
            } 
        });
        
        Task.Run(() =>
        {
            try
            {
                StartHttpServer();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in StartHttpServer: {ex.Message}");
            } 
        });
        
        Task.Run(async () =>
        {
            try
            {
                await SpeechRecognizer.StartSpeechRecognitionLoop();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in StartSpeechRecognitionLoop: {ex.Message}");
            } 
        });
        
        await Task.Delay(10000);
        
        Console.WriteLine("Religious experience is now ready...");
        
        Console.WriteLine("Press enter to send client initialization command...");
        Console.ReadLine();
        
        HttpServer.Events.Add(
            new Event(){ Type = "InitializeEverything" });
        
        Console.WriteLine("Press enter when a parishioner enters the shrine...");
        Console.ReadLine();
        
        HttpServer.Events.Add(
            new Event(){ Type = "ParishionerEnteredShrine" });
        
        Console.WriteLine("Press enter again when you want to exit the server...");
        Console.ReadLine();
        
        Environment.Exit(0);
    }


    private static void StartNfcReadWatchLoop()
    {
        Console.WriteLine("NFC Reader loop starting...");
    
        var lastUidSeen = "";
        
        while (true)
        {
            try
            {
                var nfcReader = new ReaderManager();
                var uidString = nfcReader.GetUid();

                // If we didn't throw AuthenticationException above, then valid tag was found:
                
                // If it's not a new UID, skip the rest of the code
                if (lastUidSeen == uidString) continue;
                
                lastUidSeen = uidString;
                
                HttpServer.CoinPlacedTime = DateTimeOffset.Now;

                Console.WriteLine($"Coin with UID: {uidString} has been placed on the altar");
                
                HttpServer.Events.Add(
                    new Event(){ Type = "CoinPlacedOnAltar", Data = uidString });
                
                // Console.WriteLine($"Tag found with ID in NFC reader watch loop: {uidString}");
            }
            catch (AuthenticationException) { }  // No tag found, will retry on next loop 
        }
        
        // ReSharper disable once FunctionNeverReturns because it's not supposed to
    }

    private static void StartHttpServer()
    {
        var t = 
            new Thread(delegate () { var server = new HttpServer(); });
        
        t.Start();
        
        Console.WriteLine("HttpServer Started...!");
    }
}