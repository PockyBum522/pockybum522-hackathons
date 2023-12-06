using System.Net;
using System.Text;
using CSharpPostJsonTests;
using Newtonsoft.Json;
using Formatting = System.Xml.Formatting;

namespace TestWebserver;

public class HttpServer
{
    private readonly HttpListener _listener = new();
    private string _url;
    
    public HttpServer(string url)
    {
        _url = url;
    }
    
    private int _pageViews = 0;
    private int _requestCount = 0;
        
    private string PageData =>
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
    
    private static string _jsonString =>
        """
        {
            "From": "+12762586340",
            "To": "+14076322207",
            "Eml": "<?xml version='1.0' encoding='UTF-8'?><Response><Say>Your notification alarm for the inside trashcan has been activated. I repeat, Your notification alarm for the inside trashcan has been activated. I repeat, Your notification alarm for the inside trashcan has been activated. I repeat, Your notification alarm for the inside trashcan has been activated.</Say></Response>"
        }
        """;

    
    public async Task HandleIncomingConnections()
    {
        var runServer = true;

        _listener.Prefixes.Add(_url);
        _listener.Start();
        
        Console.WriteLine("Listening for connections on {0}", _url);

        var counterForThreshold = 0;
        
        // While a user hasn't visited the `shutdown` url, keep on handling requests
        while (runServer)
        {
            // Will wait here until we hear from a connection
            var task = _listener?.GetContextAsync();

            if (task is null) return;
            
            var ctx = await task;

            // Peel out the requests and response objects
            var req = ctx.Request;
            var resp = ctx.Response;
            
            // Print out some info about the request
            Console.WriteLine("Request #: {0}", ++_requestCount);
            Console.WriteLine(req.Url?.ToString());
            Console.WriteLine(req.HttpMethod);
            Console.WriteLine(req.UserHostName);
            Console.WriteLine(req.UserAgent);
            Console.WriteLine();

            // If `shutdown` url requested w/ POST, then shutdown the server after serving the page
            if ((req.HttpMethod == "POST") && (req.Url?.AbsolutePath == "/shutdown"))
            {
                Console.WriteLine("Shutdown requested");
                runServer = false;
            }

            // Make sure we don't increment the page views counter if `favicon.ico` is requested
            if (req.Url?.AbsolutePath != "/favicon.ico")
                _pageViews += 1;

            var queriedUrl = req.Url?.AbsolutePath;

            if (queriedUrl.Contains("/detect"))
            {
                queriedUrl = queriedUrl.Replace("http://192.168.137.1:8000/detect/", "");
                queriedUrl = queriedUrl.Replace("/detect/", "");

                Console.WriteLine($"QueriedUrl: {queriedUrl}");

                var accelValues = queriedUrl.Split("/");

                var xVal = accelValues[0];
                var yVal = accelValues[1];
                var zVal = accelValues[2];

                Console.WriteLine($"x: {xVal}, y: {yVal}, z: {zVal}");

                var floatYVal = float.Parse(yVal);

            
                if (floatYVal > .5)
                {
                    
                    counterForThreshold++;
                }
                else
                {
                    if (counterForThreshold > 0)
                    {
                        counterForThreshold--;
                    }
                }

                Console.WriteLine($"Counter for threshold: {counterForThreshold}");
            
                if (counterForThreshold > 10)
                {
                    var jsonPoster = new JsonPoster();
    
                    var httpResponse = await jsonPoster.PostJsonAsync(_jsonString);

                    var httpResponseContent = await httpResponse.Content.ReadAsStringAsync();

                    var deserializedJson = JsonConvert.DeserializeObject(httpResponseContent);
    
                    var prettyJson = JsonConvert.SerializeObject(deserializedJson, (Newtonsoft.Json.Formatting)Formatting.Indented);
    
                    Console.WriteLine(prettyJson);
                    
                    Thread.Sleep(99999999);
                }
            }
                

            // Write the response info
            var disableSubmit = !runServer ? "disabled" : "";
            var data = Encoding.UTF8.GetBytes(String.Format(PageData, _pageViews, disableSubmit));
            resp.ContentType = "text/html";
            resp.ContentEncoding = Encoding.UTF8;
            resp.ContentLength64 = data.LongLength;

            // Write out to the response stream (asynchronously), then close it
            await resp.OutputStream.WriteAsync(data, 0, data.Length);
            resp.Close();
        }
    }
}