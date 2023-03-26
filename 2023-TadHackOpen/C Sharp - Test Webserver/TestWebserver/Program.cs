using System;
using System.IO;
using System.Text;
using System.Net;
using System.Threading.Tasks;

namespace TestWebserver;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("Make sure you run this program from an administrator powershell or command prompt.");
        Console.WriteLine();
        Console.WriteLine();
        
        if (args.Length == 0 || string.IsNullOrWhiteSpace(args[0]))
        {
            Console.WriteLine("""No host URL specified. Run this app like: ".\TestWebserver.exe http://192.168.1.123:8000/" where 192.168.1.123 is the LAN IP your computer currently has, on the network that you want to host it on, and 8000 is the port to host on. Also make sure to make an inbound TCP rule for that port in Windows firewall. """);
            Console.WriteLine();
            Console.WriteLine();

            Environment.Exit(0);
        }

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine($"""Setting up server on "{args[0]}" """);
        
        var server = new HttpServer(args[0]);
        
        var listenTask = server.HandleIncomingConnections();
        
        listenTask.GetAwaiter().GetResult();
    }
}