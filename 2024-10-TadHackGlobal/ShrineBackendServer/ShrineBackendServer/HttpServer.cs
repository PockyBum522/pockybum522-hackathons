using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using ShrineBackendServer.Models;

namespace ShrineBackendServer;

internal class HttpServer
{
    private int _testCounter = 0; 
    
    public List<Event> Events = [];
    public DateTimeOffset LastConnection = DateTimeOffset.Now;
    
    public HttpServer(int port)
    {
        StartListener();
    }

    private void StartListener()
    {
        try
        {
            // set the TcpListener on port 13000
            var port = 5001;
            var server = new TcpListener(IPAddress.Any, port);

            // Start listening for client requests
            server.Start();

            // Buffer for reading data
            var bytes = new byte[1024];
            string data;

            //Enter the listening loop
            while (true)
            {
                Console.Write("Waiting for a connection... ");

                // Perform a blocking call to accept requests.
                // You could also use server.AcceptSocket() here.
                var client = server.AcceptTcpClient();
                Console.WriteLine("Connected!");

                // Get a stream object for reading and writing
                var stream = client.GetStream();

                int i;

                // Loop to receive all the data sent by the client.
                i = stream.Read(bytes, 0, bytes.Length);

                Console.WriteLine($"i: {i}");
                
                // Translate data bytes to an ASCII string.
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine(String.Format("Received: {0}", data));

                // Process the data sent by the client.
                data = data.ToUpper();

                AddTestEventsOnDelay();
                
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
                stream.Write(Encoding.ASCII.GetBytes("\n"));
                
                // Send back a response.
                stream.Write(msg, 0, msg.Length);
                //Console.WriteLine(String.Format("Sent: {0}", data));
                
                // Shutdown and end connection
                Console.WriteLine("Closing connection");
                client.Close();
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
        }

        Console.WriteLine("Hit enter to continue...");
        Console.Read();
    }

    private void AddTestEventsOnDelay()
    {
        if (LastConnection < DateTimeOffset.Now.Subtract(TimeSpan.FromSeconds(5)))
        {
            Events.Clear();

            _testCounter = 0;
        }
        
        LastConnection = DateTimeOffset.Now;
        
        _testCounter++;
        
        if (_testCounter == 20) Events.Add(new Event(){Type="PersonEnteredShrine"});
        
        if (_testCounter == 30) Events.Add(new Event(){Type="CoinPlaced", Data="53C4AF8C014F80"});
        
        if (_testCounter == 40) Events.Add(new Event(){Type="NewWordsSpoken", Data="I"});
        if (_testCounter == 44) Events.Add(new Event(){Type="NewWordsSpoken", Data="fear"});
        if (_testCounter == 48) Events.Add(new Event(){Type="NewWordsSpoken", Data="demons"});
        if (_testCounter == 52) Events.Add(new Event(){Type="NewWordsSpoken", Data="because"});
        if (_testCounter == 60) Events.Add(new Event(){Type="NewWordsSpoken", Data="hey"});
        if (_testCounter == 66) Events.Add(new Event(){Type="NewWordsSpoken", Data="demons"});
        if (_testCounter == 76) Events.Add(new Event(){Type="NewWordsSpoken", Data="it's"});
        if (_testCounter == 80) Events.Add(new Event(){Type="NewWordsSpoken", Data="me"});
        if (_testCounter == 84) Events.Add(new Event(){Type="NewWordsSpoken", Data="ya"});
        if (_testCounter == 90) Events.Add(new Event(){Type="NewWordsSpoken", Data="boy."});
        if (_testCounter == 92) Events.Add(new Event(){Type="NewWordsSpoken", Data="But"});
        if (_testCounter == 94) Events.Add(new Event(){Type="NewWordsSpoken", Data="you"});
        if (_testCounter == 100) Events.Add(new Event(){Type="NewWordsSpoken", Data="know."});
        if (_testCounter == 104) Events.Add(new Event(){Type="NewWordsSpoken", Data="I"});
        if (_testCounter == 108) Events.Add(new Event(){Type="NewWordsSpoken", Data="mighta"});
        if (_testCounter == 112) Events.Add(new Event(){Type="NewWordsSpoken", Data="owed"});
        if (_testCounter == 116) Events.Add(new Event(){Type="NewWordsSpoken", Data="them"});
        if (_testCounter == 120) Events.Add(new Event(){Type="NewWordsSpoken", Data="money"});
        if (_testCounter == 124) Events.Add(new Event(){Type="NewWordsSpoken", Data="or"});
        if (_testCounter == 128) Events.Add(new Event(){Type="NewWordsSpoken", Data="something"});
        if (_testCounter == 132) Events.Add(new Event(){Type="NewWordsSpoken", Data="and"});
        if (_testCounter == 134) Events.Add(new Event(){Type="NewWordsSpoken", Data="forgotten"});
        if (_testCounter == 140) Events.Add(new Event(){Type="NewWordsSpoken", Data="about"});
        if (_testCounter == 144) Events.Add(new Event(){Type="NewWordsSpoken", Data="so"});
        if (_testCounter == 150) Events.Add(new Event(){Type="NewWordsSpoken", Data="they"});
        if (_testCounter == 154) Events.Add(new Event(){Type="NewWordsSpoken", Data="might"});
        if (_testCounter == 160) Events.Add(new Event(){Type="NewWordsSpoken", Data="be"});
        if (_testCounter == 166) Events.Add(new Event(){Type="NewWordsSpoken", Data="mad."});

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
        
        if (_testCounter == 186) Events.Add(new Event(){Type="VconWithSentiment", Data=vconJsonString});
        
    }
}