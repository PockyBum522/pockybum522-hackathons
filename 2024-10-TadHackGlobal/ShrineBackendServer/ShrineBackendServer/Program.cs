using System.Security.Authentication;
using ShrineBackendServer.Models;
using ShrineBackendServer.NfcRead;

namespace ShrineBackendServer;

public static class Program
{
    public static async Task Main()
    {
        /*
            After a reboot, or upon unplugging/replugging the NFC reader,
            you will need to run the following before using the NFC reader:

            sudo modprobe -r pn533_usb
            sudo modprobe -r pn533
        */
        
        Task.Run(StartNfcReadWatchLoop);
        Task.Run(StartHttpServer);
        Task.Run(SpeechRecognizer.StartSpeechRecognitionLoop);

        await Task.Delay(3000);
        
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
        var t = new Thread(delegate ()
        {
            var vconServer = new HttpServer(5001);
        });
        
        t.Start();
        
        Console.WriteLine("HttpServer Started...!");
    }
}