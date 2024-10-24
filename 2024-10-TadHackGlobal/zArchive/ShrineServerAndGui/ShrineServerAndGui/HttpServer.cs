using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShrineServerAndGui.Models;

namespace ShrineServerAndGui;

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
        var vconCreator = new VconCreator();
        
        var testCounter = 0; 
        var lastConnection = DateTimeOffset.Now;
        
        // set the TcpListener on port 13000
        var port = 5001;
        //var server = new TcpListener(IPAddress.Any, port);
        var server = new TcpListener(new IPAddress([192, 168, 1, 118]), port);

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

            if (_sendTestEvents) Task.Run(() => 
                AddTestEventsOnDelay(ref testCounter, ref lastConnection, vconCreator));

            while (Events.Count > 10)
            {
                Events.Remove(Events.First());
            }

            var jsonString = "";

            try
            {
                jsonString = JsonConvert.SerializeObject(Events, Formatting.Indented);
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Invalid operation attempting to make json string, will retry");
            }

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
    private void AddTestEventsOnDelay(ref int testCounter, ref DateTimeOffset lastConnection, VconCreator vconCreator)
    {
        if (lastConnection < DateTimeOffset.Now.Subtract(TimeSpan.FromSeconds(5)))
        {
            Events.Clear();

            testCounter = 0;
        }
        
        lastConnection = DateTimeOffset.Now;
        
        testCounter++;
        
        if (testCounter == 10) Events.Add(new Event(){Type="InitializeEverything"});
        
        if (testCounter == 20) Events.Add(new Event(){Type="ParishionerEnteredShrine"});
        
        if (testCounter == 30) Events.Add(new Event(){Type="CoinPlacedOnAltar", Data="53C4AF8C014F80"});
        
        if (testCounter == 40) Events.Add(new Event(){Type="NewWordSpoken", Data="I"});
        if (testCounter == 44) Events.Add(new Event(){Type="NewWordSpoken", Data="fear"});
        if (testCounter == 48) Events.Add(new Event(){Type="NewWordSpoken", Data="demons"});
        if (testCounter == 52) Events.Add(new Event(){Type="NewWordSpoken", Data="because"});
        if (testCounter == 60) Events.Add(new Event(){Type="NewWordSpoken", Data="hey"});
        if (testCounter == 66) Events.Add(new Event(){Type="NewWordSpoken", Data="demons"});
        if (testCounter == 76) Events.Add(new Event(){Type="NewWordSpoken", Data="it's"});
        if (testCounter == 80) Events.Add(new Event(){Type="NewWordSpoken", Data="me"});
        if (testCounter == 84) Events.Add(new Event(){Type="NewWordSpoken", Data="ya"});
        if (testCounter == 90) Events.Add(new Event(){Type="NewWordSpoken", Data="boy."});
        if (testCounter == 92) Events.Add(new Event(){Type="NewWordSpoken", Data="But"});
        if (testCounter == 94) Events.Add(new Event(){Type="NewWordSpoken", Data="you"});
        if (testCounter == 100) Events.Add(new Event(){Type="NewWordSpoken", Data="know."});
        if (testCounter == 104) Events.Add(new Event(){Type="NewWordSpoken", Data="I"});
        if (testCounter == 108) Events.Add(new Event(){Type="NewWordSpoken", Data="mighta"});
        if (testCounter == 112) Events.Add(new Event(){Type="NewWordSpoken", Data="owed"});
        if (testCounter == 116) Events.Add(new Event(){Type="NewWordSpoken", Data="them"});
        if (testCounter == 120) Events.Add(new Event(){Type="NewWordSpoken", Data="money"});
        if (testCounter == 124) Events.Add(new Event(){Type="NewWordSpoken", Data="or"});
        if (testCounter == 128) Events.Add(new Event(){Type="NewWordSpoken", Data="something"});
        if (testCounter == 132) Events.Add(new Event(){Type="NewWordSpoken", Data="and"});
        if (testCounter == 134) Events.Add(new Event(){Type="NewWordSpoken", Data="forgotten"});
        if (testCounter == 140) Events.Add(new Event(){Type="NewWordSpoken", Data="about"});
        if (testCounter == 144) Events.Add(new Event(){Type="NewWordSpoken", Data="so"});
        if (testCounter == 150) Events.Add(new Event(){Type="NewWordSpoken", Data="they"});
        if (testCounter == 154) Events.Add(new Event(){Type="NewWordSpoken", Data="might"});
        if (testCounter == 160) Events.Add(new Event(){Type="NewWordSpoken", Data="be"});
        if (testCounter == 166) Events.Add(new Event(){Type="NewWordSpoken", Data="mad."});

        if (testCounter == 170)
        {
            Task.Run(async () =>
            {
                var vconJsonString = vconCreator.GetVconJsonString(
                    "I fear demons because hey demons it's me ya boy. But you know. " +
                    "I mighta owed them money or something and forgotten about so they might be mad.");

                Events.Add(new Event(){Type="VconWithSentiment", Data=await vconJsonString}); 
            });
        }
    }
}