using System.Security.Authentication;
using ShrineBackendServer.NfcRead;

namespace ShrineBackendServer;

public static class Program
{
    private static string _lastTagSeenUidString = "";
    
    public static void Main()
    {
        // After a reboot, or upon unplugging/replugging the NFC reader,
        // you will need to run the following before using the NFC reader:
        //
        // sudo modprobe -r pn533_usb
        // sudo modprobe -r pn533
        
        StartNfcReadWatchLoop();
    }

    private static void StartNfcReadWatchLoop()
    {
        Console.WriteLine("Please tap an NFC tag to the reader when ready...");
        
        while (true)
        {
            try
            {
                var nfcReader = new ReaderManager();
                var uidString = nfcReader.GetUid();

                // If we didn't throw AuthenticationException above, then valid tag was found:
                AddTagFoundEventToEventsList(uidString);
            }
            catch (AuthenticationException) { }  // No tag found, will retry on next loop 
        }
        
        // ReSharper disable once FunctionNeverReturns because it's not supposed to
    }

    private static void AddTagFoundEventToEventsList(string uidString)
    {
        // Only do the following if it's a new tag seen (only do things once)
        if (uidString == _lastTagSeenUidString) return; 
        
        _lastTagSeenUidString = uidString;
            
        Console.WriteLine($"Tag found with ID: {uidString}");
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