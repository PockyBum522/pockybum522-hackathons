using System.Collections.Generic;
using System.Net;

namespace JambonzTextToSpeechTest;

internal static class Program
{
    private static bool _keepRunning = true;

    static async Task Main(string[] args)
    {
        Console.CancelKeyPress += delegate(object? sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            Program._keepRunning = false;
        };

        Console.WriteLine("Starting HTTP listener...");

        var httpServer = new HttpServer();
        httpServer.Start();

        while (Program._keepRunning) { }

        httpServer.Stop();
        await Task.Delay(1000); // Give it a sec to actually stop
        
        Console.WriteLine("Exiting gracefully...");
    }
}
