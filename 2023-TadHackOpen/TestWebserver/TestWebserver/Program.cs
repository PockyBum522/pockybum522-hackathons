using System;
using System.IO;
using System.Text;
using System.Net;
using System.Threading.Tasks;

namespace TestWebserver;

public static class Program
{
    public static void Main()
    {
        // Handle requests
        var listenTask = HttpServer.HandleIncomingConnections();
        listenTask.GetAwaiter().GetResult();
    }
}