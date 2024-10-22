using System.Globalization;
using System.Security.Authentication;
using OpenAI.Chat;
using ShrineBackendServer.Models;
using ShrineBackendServer.NfcRead;

namespace ShrineBackendServer;

public static class Program
{
    private const bool TestServerOnly = true;
    public const bool SendFakeEventsAutomatically = true;

    public static async Task Main()
    {
        /*
            After a reboot, or upon unplugging/replugging the NFC reader,
            you will need to run the following before using the NFC reader:

            sudo modprobe -r pn533_usb && sudo modprobe -r pn533
        */
        
        Console.WriteLine("Please wait, your religious experience is loading...");
        
        if (!TestServerOnly)
        {
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
        }
        
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
        
        Console.WriteLine("Religious experience is now ready...");
        
        Console.WriteLine("Press enter to send client initialization command...");
        Console.ReadLine();
        
        HttpServer.Events.Add(
            new Event(){ Type = "InitializeEverything" });
        
        Console.WriteLine("Press enter when a parishioner enters the shrine...");
        Console.ReadLine();
        
        HttpServer.Events.Add(
            new Event(){ Type = "ParishionerEnteredShrine" });
        
        Console.WriteLine();
        
        Console.WriteLine("Press enter to exit the server...");
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

                Console.Clear();
                Console.WriteLine();
                Console.WriteLine($"Coin has been placed on the altar");
                
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