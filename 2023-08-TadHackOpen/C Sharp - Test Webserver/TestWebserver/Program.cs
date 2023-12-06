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
        
        var server = new HttpServer("http://192.168.137.1:8000/");
        
        var listenTask = server.HandleIncomingConnections();
        
        listenTask.GetAwaiter().GetResult();
    }
}