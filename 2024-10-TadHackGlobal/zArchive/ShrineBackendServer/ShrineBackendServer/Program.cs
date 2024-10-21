using System.Globalization;
using System.Security.Authentication;
using OpenAI.Chat;
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

            sudo modprobe -r pn533_usb && sudo modprobe -r pn533
        */
        
        // Console.WriteLine("Please wait, your religious experience is loading...");
        //
        // Task.Run(() =>
        // {
        //     try
        //     {
        //         StartNfcReadWatchLoop();
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine($"Exception in StartNfcReadWatchLoop: {ex.Message}");
        //     } 
        // });
        //
        // Task.Run(() =>
        // {
        //     try
        //     {
        //         StartHttpServer();
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine($"Exception in StartHttpServer: {ex.Message}");
        //     } 
        // });
        //
        // Task.Run(async () =>
        // {
        //     try
        //     {
        //         await SpeechRecognizer.StartSpeechRecognitionLoop();
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine($"Exception in StartSpeechRecognitionLoop: {ex.Message}");
        //     } 
        // });
        //
        // await Task.Delay(10000);
        //
        // Console.WriteLine("Religious experience is now ready...");
        //
        // Console.WriteLine("Press enter to send client initialization command...");
        // Console.ReadLine();
        //
        // HttpServer.Events.Add(
        //     new Event(){ Type = "InitializeEverything" });
        //
        // Console.WriteLine("Press enter when a parishioner enters the shrine...");
        // Console.ReadLine();
        //
        // Console.Clear();
        //
        // HttpServer.Events.Add(
        //     new Event(){ Type = "ParishionerEnteredShrine" });
        //
        // Console.WriteLine();

        var openAiReq = new OpenAiRequester();

        var message =
            "I find that I’m anxious for the future, fatigued about the past, but excited about the present. The world spins fast under my feet and I hope I can run fast enough to keep up. Every thing that changes is a new thing to learn, and the burden of education is weightier for a mind ossified by time. My hope is that with enough flexibility, I can keep my mind limber enough to adapt to the world incoming with the rising tide, and be comfortable leaving my old world as it recedes behind me. Not every death is physical; death of the spirit is equally real. Adapt or die.";

        var aiSentiment = openAiReq.AnalyzeSentiment(message);

        await Task.Delay(100);

        var mostEmotionalWords = openAiReq.GetMostEmotionalWords(message);
        
        await Task.Delay(100);

        Console.WriteLine("aiSentiment:");
        Console.WriteLine(aiSentiment);
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("mostEmotionalWords:");
        Console.WriteLine(string.Join(',', mostEmotionalWords));
        Console.WriteLine();
        Console.WriteLine();
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