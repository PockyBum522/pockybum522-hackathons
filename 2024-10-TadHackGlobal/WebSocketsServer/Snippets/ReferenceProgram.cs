// using System.Net.WebSockets;
// using System.Text;
// using Newtonsoft.Json;
// using WebSocketsServer.Models;
//
// namespace WebSocketsServer.Snippets;
//
// public static class Program
// {
//     public static void UseWebSockets(WebApplication app)
//     {
//         // <snippet_UseWebSockets>
//         app.UseWebSockets();
//         // </snippet_UseWebSockets>
//     }
//
//     public static void AcceptWebSocketAsyncBackgroundSocketProcessor(WebApplication app)
//     {
//         // <snippet_AcceptWebSocketAsyncBackgroundSocketProcessor>
//         app.Run(async (context) =>
//         {
//             using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
//             var socketFinishedTcs = new TaskCompletionSource<object>();
//
//             BackgroundSocketProcessor.AddSocket(webSocket, socketFinishedTcs);
//
//             await socketFinishedTcs.Task;
//         });
//         // </snippet_AcceptWebSocketAsyncBackgroundSocketProcessor>
//     }
//
//     public static void UseWebSocketsOptionsAllowedOrigins(WebApplication app)
//     {
//         // <snippet_UseWebSocketsOptionsAllowedOrigins>
//         var webSocketOptions = new WebSocketOptions
//         {
//             KeepAliveInterval = TimeSpan.FromMinutes(2)
//         };
//
//         webSocketOptions.AllowedOrigins.Add("https://client.com");
//         webSocketOptions.AllowedOrigins.Add("https://www.client.com");
//
//         app.UseWebSockets(webSocketOptions);
//         // </snippet_UseWebSocketsOptionsAllowedOrigins>
//     }
//
// }
