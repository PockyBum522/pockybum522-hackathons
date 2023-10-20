using System.Net;

namespace JambonzTextToSpeechTest;

public class HttpServer
{
    public int Port = 8080;

    private HttpListener _listener;

    public void Start()
    {
        _listener = new HttpListener();
        _listener.Prefixes.Add("http://*:" + Port.ToString() + "/");
        _listener.Start();
        Receive();
    }

    public void Stop()
    {
        _listener.Stop();
    }

    private void Receive()
    {
        _listener.BeginGetContext(new AsyncCallback(ListenerCallback), _listener);
    }

    private void ListenerCallback(IAsyncResult result)
    {
        if (_listener.IsListening)
        {
            var context = _listener.EndGetContext(result);
            var request = context.Request;

            // do something with the request
            Console.WriteLine($"Request HTTP method: {request.HttpMethod} | Request URL: {request.Url}");

            if (request.HasEntityBody)
            {
                var body = request.InputStream;
                var encoding = request.ContentEncoding;
                var reader = new StreamReader(body, encoding);
                if (request.ContentType != null)
                {
                    Console.WriteLine("Client data content type {0}", request.ContentType);
                }
                Console.WriteLine("Client data content length {0}", request.ContentLength64);

                Console.WriteLine("Start of data:");
                string s = reader.ReadToEnd();
                Console.WriteLine(s);
                Console.WriteLine("End of data:");
                reader.Close();
                body.Close();
            }

            Receive();

            var response = context.Response;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.ContentType = "text/plain";
            response.OutputStream.Write(new byte[] { }, 0, 0);
            response.OutputStream.Close();
        }
    }
}