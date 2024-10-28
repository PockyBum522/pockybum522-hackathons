using System.Net.WebSockets;
using System.Text;
using Newtonsoft.Json;
using WebSocketsServer.Models;

namespace WebSocketsServer;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        var app = builder.Build();

        // <snippet_UseWebSockets>
        var webSocketOptions = new WebSocketOptions
        {
            KeepAliveInterval = TimeSpan.FromMinutes(2)
        };

        app.UseWebSockets(webSocketOptions);
        // </snippet_UseWebSockets>
        
        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.MapControllers();

        AcceptWebSocketAsync(app);
        
        app.Run();
    }
    
    private static void AcceptWebSocketAsync(WebApplication app)
    {
        // <snippet_AcceptWebSocketAsync>
        app.Use(async (context, next) =>
        {
            if (context.Request.Path == "/ws")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    await Echo(webSocket);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                }
            }
            else
            {
                await next(context);
            }

        });
        // </snippet_AcceptWebSocketAsync>
    }
    
    
    // <snippet_Echo>
    private static async Task Echo(WebSocket webSocket)
    {
        var buffer = new byte[1024 * 4];

        Console.WriteLine("Waiting on receive result");
        
        // var receiveResult = await webSocket.ReceiveAsync(
        //     new ArraySegment<byte>(buffer), CancellationToken.None);

        var eventsDelayCounter = 0;
        
        Console.WriteLine("About to start events loop");
        
        //while (!receiveResult.CloseStatus.HasValue)
        while (true)
        {
            // receiveResult = await webSocket.ReceiveAsync(
            //     new ArraySegment<byte>(buffer), CancellationToken.None);
            
            await AddTestEventsOnDelay(eventsDelayCounter++, webSocket);
            
            await Task.Delay(100, CancellationToken.None);
        }

        // Don't really need this if we LOOP FOREVERRRRRR
        
        // await webSocket.CloseAsync(
        //     receiveResult.CloseStatus.Value,
        //     receiveResult.CloseStatusDescription,
        //     CancellationToken.None);
     
        // ReSharper disable once FunctionNeverReturns
    }
    // </snippet_Echo>
    
    // ReSharper disable once CognitiveComplexity because sometimes it's just wrong
    private static async Task AddTestEventsOnDelay(int eventsDelayCounter, WebSocket webSocket)
    {
        const double adjustMultiplier = 2.0;
        
	    if (isApproximately(eventsDelayCounter, 10 * adjustMultiplier)) await SendWebsocketEventMessage(webSocket, new Event(){Type="InitializeEverything"});

	    if (isApproximately(eventsDelayCounter, 20 * adjustMultiplier)) await SendWebsocketEventMessage(webSocket, new Event(){Type="ParishionerEnteredShrine"});

	    if (isApproximately(eventsDelayCounter, 30 * adjustMultiplier)) await SendWebsocketEventMessage(webSocket, new Event(){Type="CoinPlacedOnAltar", Data="53C4AF8C014F80"});

	    if (isApproximately(eventsDelayCounter, 40 * adjustMultiplier)) await SendWebsocketEventMessage(webSocket, new Event(){Type="NewWordSpoken", Data="I"});
	    if (isApproximately(eventsDelayCounter, 44 * adjustMultiplier)) await SendWebsocketEventMessage(webSocket, new Event(){Type="NewWordSpoken", Data="fear"});
	    if (isApproximately(eventsDelayCounter, 48 * adjustMultiplier)) await SendWebsocketEventMessage(webSocket, new Event(){Type="NewWordSpoken", Data="demons"});
	    if (isApproximately(eventsDelayCounter, 52 * adjustMultiplier)) await SendWebsocketEventMessage(webSocket, new Event(){Type="NewWordSpoken", Data="because"});
	    if (isApproximately(eventsDelayCounter, 60 * adjustMultiplier)) await SendWebsocketEventMessage(webSocket, new Event(){Type="NewWordSpoken", Data="hey"});
	    if (isApproximately(eventsDelayCounter, 66 * adjustMultiplier)) await SendWebsocketEventMessage(webSocket, new Event(){Type="NewWordSpoken", Data="demons"});
	    if (isApproximately(eventsDelayCounter, 76 * adjustMultiplier)) await SendWebsocketEventMessage(webSocket, new Event(){Type="NewWordSpoken", Data="it's"});
	    if (isApproximately(eventsDelayCounter, 80 * adjustMultiplier)) await SendWebsocketEventMessage(webSocket, new Event(){Type="NewWordSpoken", Data="me"});
	    if (isApproximately(eventsDelayCounter, 84 * adjustMultiplier)) await SendWebsocketEventMessage(webSocket, new Event(){Type="NewWordSpoken", Data="ya"});
	    if (isApproximately(eventsDelayCounter, 90 * adjustMultiplier)) await SendWebsocketEventMessage(webSocket, new Event(){Type="NewWordSpoken", Data="boy."});
	    if (isApproximately(eventsDelayCounter, 92 * adjustMultiplier)) await SendWebsocketEventMessage(webSocket, new Event(){Type="NewWordSpoken", Data="But"});
	    if (isApproximately(eventsDelayCounter, 94 * adjustMultiplier)) await SendWebsocketEventMessage(webSocket, new Event(){Type="NewWordSpoken", Data="you"});
	    if (isApproximately(eventsDelayCounter, 100 * adjustMultiplier)) await SendWebsocketEventMessage(webSocket, new Event(){Type="NewWordSpoken", Data="know."});
	    if (isApproximately(eventsDelayCounter, 104 * adjustMultiplier)) await SendWebsocketEventMessage(webSocket, new Event(){Type="NewWordSpoken", Data="I"});
	    if (isApproximately(eventsDelayCounter, 108 * adjustMultiplier)) await SendWebsocketEventMessage(webSocket, new Event(){Type="NewWordSpoken", Data="mighta"});
	    if (isApproximately(eventsDelayCounter, 112 * adjustMultiplier)) await SendWebsocketEventMessage(webSocket, new Event(){Type="NewWordSpoken", Data="owed"});
	    if (isApproximately(eventsDelayCounter, 116 * adjustMultiplier)) await SendWebsocketEventMessage(webSocket, new Event(){Type="NewWordSpoken", Data="them"});
	    if (isApproximately(eventsDelayCounter, 120 * adjustMultiplier)) await SendWebsocketEventMessage(webSocket, new Event(){Type="NewWordSpoken", Data="money"});
	    if (isApproximately(eventsDelayCounter, 124 * adjustMultiplier)) await SendWebsocketEventMessage(webSocket, new Event(){Type="NewWordSpoken", Data="or"});
	    if (isApproximately(eventsDelayCounter, 128 * adjustMultiplier)) await SendWebsocketEventMessage(webSocket, new Event(){Type="NewWordSpoken", Data="something"});
	    if (isApproximately(eventsDelayCounter, 132 * adjustMultiplier)) await SendWebsocketEventMessage(webSocket, new Event(){Type="NewWordSpoken", Data="and"});
	    if (isApproximately(eventsDelayCounter, 134 * adjustMultiplier)) await SendWebsocketEventMessage(webSocket, new Event(){Type="NewWordSpoken", Data="forgotten"});
	    if (isApproximately(eventsDelayCounter, 140 * adjustMultiplier)) await SendWebsocketEventMessage(webSocket, new Event(){Type="NewWordSpoken", Data="about"});
	    if (isApproximately(eventsDelayCounter, 144 * adjustMultiplier)) await SendWebsocketEventMessage(webSocket, new Event(){Type="NewWordSpoken", Data="so"});
	    if (isApproximately(eventsDelayCounter, 150 * adjustMultiplier)) await SendWebsocketEventMessage(webSocket, new Event(){Type="NewWordSpoken", Data="they"});
	    if (isApproximately(eventsDelayCounter, 154 * adjustMultiplier)) await SendWebsocketEventMessage(webSocket, new Event(){Type="NewWordSpoken", Data="might"});
	    if (isApproximately(eventsDelayCounter, 160 * adjustMultiplier)) await SendWebsocketEventMessage(webSocket, new Event(){Type="NewWordSpoken", Data="be"});
	    if (isApproximately(eventsDelayCounter, 166 * adjustMultiplier)) await SendWebsocketEventMessage(webSocket, new Event(){Type="NewWordSpoken", Data="mad."});
        
        // if (testCounter == 170 * adjustMultiplier)
        // {
        //     Task.Run(async () =>
        //     {
        //         var vconJsonString = vconCreator.GetVconJsonString(
        //             "I fear demons because hey demons it's me ya boy. But you know. " +
        //             "I mighta owed them money or something and forgotten about so they might be mad.");
        //
        //         Events.Add(new Event(){Type="VconWithSentiment", Data=await vconJsonString}); 
        //     });
        // }
    }

    private static async Task SendWebsocketEventMessage(WebSocket webSocket, Event eventToSend)
    {
        var message = JsonConvert.SerializeObject(eventToSend, Formatting.Indented);
        
        var messageBytes = Encoding.ASCII.GetBytes(message);
        
        var arraySegment = new ArraySegment<byte>(messageBytes, 0, messageBytes.Length);
        
        await webSocket.SendAsync(
            arraySegment,
            WebSocketMessageType.Text, 
            WebSocketMessageFlags.EndOfMessage,
            CancellationToken.None);        
    }


    private static bool isApproximately(int testValue, double equateToValue)
    {
        return Math.Abs(testValue - equateToValue) <= 0.5;
    }
}