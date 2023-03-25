using System.Net;
using System.Text;

namespace TestWebserver;

public class HttpServer
{
    private static HttpListener? _listener = new();
        
    private static string url = "http://192.168.137.1:8000/";
        
    private static int _pageViews = 0;
    private static int _requestCount = 0;
        
    private static string _pageData =>
                            $"""
                                <!DOCTYPE>
                                        <html>
                                            <head>
                                                <title>HttpListener Example</title>
                                            </head>
                                            <body>
                                                <p>Page Views: {_requestCount}</p>
                                                
                                            </body>
                                </html>
                            """;
    //     <form method="post" action="shutdown">
    // <input type="submit" value="Shutdown">
    // </form>
    public static async Task HandleIncomingConnections()
    {
       
        bool runServer = true;

        _listener.Prefixes.Add(url);
        _listener.Start();
        
        Console.WriteLine("Listening for connections on {0}", url);

        
        // While a user hasn't visited the `shutdown` url, keep on handling requests
        while (runServer)
        {
            // Will wait here until we hear from a connection
            HttpListenerContext ctx = await _listener.GetContextAsync();

            // Peel out the requests and response objects
            HttpListenerRequest req = ctx.Request;
            HttpListenerResponse resp = ctx.Response;

            // Print out some info about the request
            Console.WriteLine("Request #: {0}", ++_requestCount);
            Console.WriteLine(req.Url.ToString());
            Console.WriteLine(req.HttpMethod);
            Console.WriteLine(req.UserHostName);
            Console.WriteLine(req.UserAgent);
            Console.WriteLine();

            // If `shutdown` url requested w/ POST, then shutdown the server after serving the page
            if ((req.HttpMethod == "POST") && (req.Url.AbsolutePath == "/shutdown"))
            {
                Console.WriteLine("Shutdown requested");
                runServer = false;
            }

            // Make sure we don't increment the page views counter if `favicon.ico` is requested
            if (req.Url.AbsolutePath != "/favicon.ico")
                _pageViews += 1;

            // Write the response info
            string disableSubmit = !runServer ? "disabled" : "";
            byte[] data = Encoding.UTF8.GetBytes(String.Format(_pageData, _pageViews, disableSubmit));
            resp.ContentType = "text/html";
            resp.ContentEncoding = Encoding.UTF8;
            resp.ContentLength64 = data.LongLength;

            // Write out to the response stream (asynchronously), then close it
            await resp.OutputStream.WriteAsync(data, 0, data.Length);
            resp.Close();
        }
    }
}