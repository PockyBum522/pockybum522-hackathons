using System;
using System.IO;
using System.Text;
using System.Net;
using System.Threading.Tasks;

namespace TestWebserver;

public static class Program
{
    public static HttpListener listener;
    public static string url = "http://localhost:8000/";
    public static int pageViews = 0;
    public static int requestCount = 0;
    public static string pageData = 
        """<!DOCTYPE>
                <html>
                    <head>
                        <title>HttpListener Example</title>
                    </head>
                    <body>
                        <p>Page Views: {0}</p>
                        <form method="post" action="shutdown">
                        <input type="submit" value="Shutdown" {1}>
                    </form>
          </body>
        </html>""";

    public static void Main()
    {
        
        // Create a Http server and start listening for incoming connections
        var listener = new HttpListener();
        listener.Prefixes.Add(url);
        listener.Start();
        Console.WriteLine("Listening for connections on {0}", url);

        // Handle requests
        Task listenTask = listener.HandleIncomingConnections();
        listenTask.GetAwaiter().GetResult();

        // Close the listener
        listener.Close();
    }
}