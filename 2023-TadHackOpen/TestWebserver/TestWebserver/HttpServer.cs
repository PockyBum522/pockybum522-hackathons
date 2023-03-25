using System.Net;
using System.Text;

namespace TestWebserver;

static class HttpServer
{
    public static async Task HandleIncomingConnections()
    {
        bool runServer = true;

        // While a user hasn't visited the `shutdown` url, keep on handling requests
        while (runServer)
        {
            // Will wait here until we hear from a connection
            HttpListenerContext ctx = await listener.GetContextAsync();

            // Peel out the requests and response objects
            HttpListenerRequest req = ctx.Request;
            HttpListenerResponse resp = ctx.Response;

            // Print out some info about the request
            Console.WriteLine("Request #: {0}", ++requestCount);
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
                pageViews += 1;

            // Write the response info
            string disableSubmit = !runServer ? "disabled" : "";
            byte[] data = Encoding.UTF8.GetBytes(String.Format(pageData, pageViews, disableSubmit));
            resp.ContentType = "text/html";
            resp.ContentEncoding = Encoding.UTF8;
            resp.ContentLength64 = data.LongLength;

            // Write out to the response stream (asynchronously), then close it
            await resp.OutputStream.WriteAsync(data, 0, data.Length);
            resp.Close();
        }
    }
}