using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using ShrineBackendServer.Models;

namespace ShrineBackendServer;

public class HttpServer
{
    private readonly bool _sendTestEvents = true;

    public static List<Event> Events { get; } = [];
    public static DateTimeOffset CoinPlacedTime { get; set; } = DateTimeOffset.MaxValue;
    
    public HttpServer()
    {
        StartListener();
    }

    private void StartListener()
    {
        var testCounter = 0; 
        var lastConnection = DateTimeOffset.Now;
        
        // set the TcpListener on port 13000
        var port = 5001;
        var server = new TcpListener(IPAddress.Any, port);

        // Start listening for client requests
        server.Start();

        // Buffer for reading data
        var bytes = new byte[1024];

        //Enter the listening loop
        while (true)
        {
            // Console.Write("Waiting for a connection... ");

            // Perform a blocking call to accept requests.
            // You could also use server.AcceptSocket() here.
            var client = server.AcceptTcpClient();
            
            // Console.WriteLine("Connected!");

            // Get a stream object for reading and writing
            var stream = client.GetStream();
            
            // Receive all the data sent by the client.
            var i = stream.Read(bytes, 0, bytes.Length);

            //Console.WriteLine($"i: {i}");
            // Translate data bytes to an ASCII string.
            var data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
            
            // Console.WriteLine(String.Format("Received: {0}", data));

            // Process the data sent by the client.
            data = data.ToUpper();

            if (_sendTestEvents) AddTestEventsOnDelay(ref testCounter, ref lastConnection);
            
            while (Events.Count > 10)
            {
                Events.Remove(Events.First());
            }
            
            var jsonString = JsonConvert.SerializeObject(Events, Formatting.Indented);
            
            var msg = Encoding.ASCII.GetBytes(jsonString);

            // Send headers
            stream.Write(Encoding.ASCII.GetBytes("HTTP/1.1 200 OK\n"));
            stream.Write(Encoding.ASCII.GetBytes("Access-Control-Allow-Origin: *\n"));
            stream.Write(Encoding.ASCII.GetBytes("Access-Control-Allow-Headers: *\n"));
            stream.Write(Encoding.ASCII.GetBytes("Content-Type: text/plain\n"));
            stream.Write(Encoding.ASCII.GetBytes("\n"));
            
            // Send back a response.
            stream.Write(msg, 0, msg.Length);
            // Console.WriteLine(String.Format("Sent: {0}", data));
            
            // Shutdown and end connection
            // Console.WriteLine("Closing connection");
            client.Close();
        }
        
        // ReSharper disable once FunctionNeverReturns
    }

    // ReSharper disable once CognitiveComplexity because sometimes it's just wrong
    private void AddTestEventsOnDelay(ref int testCounter, ref DateTimeOffset lastConnection)
    {
        if (lastConnection < DateTimeOffset.Now.Subtract(TimeSpan.FromSeconds(5)))
        {
            Events.Clear();

            testCounter = 0;
        }
        
        lastConnection = DateTimeOffset.Now;
        
        testCounter++;
        
        if (testCounter == 20) Events.Add(new Event(){Type="PersonEnteredShrine"});
        
        if (testCounter == 30) Events.Add(new Event(){Type="CoinPlaced", Data="53C4AF8C014F80"});
        
        if (testCounter == 40) Events.Add(new Event(){Type="NewWordsSpoken", Data="I"});
        if (testCounter == 44) Events.Add(new Event(){Type="NewWordsSpoken", Data="fear"});
        if (testCounter == 48) Events.Add(new Event(){Type="NewWordsSpoken", Data="demons"});
        if (testCounter == 52) Events.Add(new Event(){Type="NewWordsSpoken", Data="because"});
        if (testCounter == 60) Events.Add(new Event(){Type="NewWordsSpoken", Data="hey"});
        if (testCounter == 66) Events.Add(new Event(){Type="NewWordsSpoken", Data="demons"});
        if (testCounter == 76) Events.Add(new Event(){Type="NewWordsSpoken", Data="it's"});
        if (testCounter == 80) Events.Add(new Event(){Type="NewWordsSpoken", Data="me"});
        if (testCounter == 84) Events.Add(new Event(){Type="NewWordsSpoken", Data="ya"});
        if (testCounter == 90) Events.Add(new Event(){Type="NewWordsSpoken", Data="boy."});
        if (testCounter == 92) Events.Add(new Event(){Type="NewWordsSpoken", Data="But"});
        if (testCounter == 94) Events.Add(new Event(){Type="NewWordsSpoken", Data="you"});
        if (testCounter == 100) Events.Add(new Event(){Type="NewWordsSpoken", Data="know."});
        if (testCounter == 104) Events.Add(new Event(){Type="NewWordsSpoken", Data="I"});
        if (testCounter == 108) Events.Add(new Event(){Type="NewWordsSpoken", Data="mighta"});
        if (testCounter == 112) Events.Add(new Event(){Type="NewWordsSpoken", Data="owed"});
        if (testCounter == 116) Events.Add(new Event(){Type="NewWordsSpoken", Data="them"});
        if (testCounter == 120) Events.Add(new Event(){Type="NewWordsSpoken", Data="money"});
        if (testCounter == 124) Events.Add(new Event(){Type="NewWordsSpoken", Data="or"});
        if (testCounter == 128) Events.Add(new Event(){Type="NewWordsSpoken", Data="something"});
        if (testCounter == 132) Events.Add(new Event(){Type="NewWordsSpoken", Data="and"});
        if (testCounter == 134) Events.Add(new Event(){Type="NewWordsSpoken", Data="forgotten"});
        if (testCounter == 140) Events.Add(new Event(){Type="NewWordsSpoken", Data="about"});
        if (testCounter == 144) Events.Add(new Event(){Type="NewWordsSpoken", Data="so"});
        if (testCounter == 150) Events.Add(new Event(){Type="NewWordsSpoken", Data="they"});
        if (testCounter == 154) Events.Add(new Event(){Type="NewWordsSpoken", Data="might"});
        if (testCounter == 160) Events.Add(new Event(){Type="NewWordsSpoken", Data="be"});
        if (testCounter == 166) Events.Add(new Event(){Type="NewWordsSpoken", Data="mad."});

        var vconJsonString = GetVconJsonString();

        if (testCounter == 186) Events.Add(new Event(){Type="VconWithSentiment", Data=vconJsonString});
        
    }

    private static string GetVconJsonString()
    {
        var vconDialog = new Dialog()
        {
            Alg = "SHA-512",
            Duration = 30.0,
            Parties = [0],
            Start = DateTime.Now
        };

        var vconParty = new Party()
        {
            Name = "Parishioner"
        };

        var vconAnalysis = new Analysis()
        {
            Type = "transcript",
            Dialog = 0,
            Vendor = "Temple of Computing",
            Encoding = "none"
        };
        
        vconAnalysis.Body.Add(
            new Body()
            {
                Speaker = "Parishioner",
                Message = "I fear demons because hey demons it's me ya boy. But you know. I mighta owed them money or something and forgotten about so they might be mad.",
                Emotion = 0.2362
                
            });
        
        var testVcon = new Vcon();
        
        testVcon.Dialog.Add(vconDialog);
        testVcon.Parties.Add(vconParty);
        testVcon.Analysis.Add(vconAnalysis);
        
        var vconJsonString = JsonConvert.SerializeObject(testVcon);
        return vconJsonString;
    }
}