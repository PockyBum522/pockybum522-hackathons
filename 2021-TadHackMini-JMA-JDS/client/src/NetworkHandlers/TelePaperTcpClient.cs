using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Ink;
using Newtonsoft.Json;
using JsonConverter = TelePaperProject.NetworkHandlers.JsonConverter;

namespace ConsoleApp1
{
    public class TelePaperTcpClient
    {
        private static TcpClient? _client;
        private static NetworkStream? _networkStream;
        
        public static void Connect(String server)
        {
            if (_client is not null || _networkStream is not null) return; // We're already connected 
            
            try
            {
                Int32 port = 5001;
                _client = new TcpClient(server, port);

                _networkStream = _client.GetStream();
            }
            catch (Exception e) 
            {
                Console.WriteLine("Exception: {0}", e);
            }
        }
        
        public static void SendLatestStroke(Stroke strokeToSend) 
        {
            try
            {
                if (_client is null || _networkStream is null) 
                    throw new Exception("Client or stream not initialized, call connect first");

                byte[] myByteArray = new byte[10000000];
                MemoryStream? stream = new MemoryStream(myByteArray);

                var strokeCollectionToSend = new StrokeCollection();
                
                strokeCollectionToSend.Add(strokeToSend);
                
                strokeCollectionToSend.Save(stream);

                if (stream.Position > 0)
                {
                    _networkStream.Write(myByteArray, 0, (int)stream.Position);

                    stream = null;
                    
                    Debug.WriteLine("Sent new strokes");  
                }
                       
            } 
            catch (Exception e) 
            {
                Console.WriteLine("Exception: {0}", e);
            }
        }

        public static async Task<MemoryStream> GetCurrentStrokeCollection()
        {            
            try
            {
                if (_client is null || _networkStream is null) 
                    throw new Exception("Client or stream not initialized, call connect first");

                // Bytes Array to receive Server Response.
                var data = new Byte[10000000];
                String response = String.Empty;
                
                // Read the Tcp Server Response Bytes.
                var bytes = _networkStream.Read(data, 0, data.Length);
                
                //response = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Debug.WriteLine("Received: {0}", response);

                
                MemoryStream memoryStream = new MemoryStream(data);

                return memoryStream;
            } 
            catch (Exception e) 
            {
                Console.WriteLine("Exception: {0}", e);
            }

            // On failure
            return new MemoryStream();
        }
        
        public static void Disconnect()
        {
            if (_client is null || _networkStream is null) 
                throw new Exception("Client or stream not initialized, call connect first");

            _networkStream.Close();         
            _client.Close();
        }
        //
        // public static void ConnectAndSend(String server, String message) 
        // {
        //     try 
        //     {
        //         Int32 port = 5001;
        //         TcpClient client = new TcpClient(server, port);
        //
        //         NetworkStream stream = client.GetStream();
        //
        //         // Translate the Message into ASCII.
        //         Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);   
        //
        //         // Send the message to the connected TcpServer. 
        //         stream.Write(data, 0, data.Length);
        //         Console.WriteLine("Sent: {0}", message);         
        //
        //         // Bytes Array to receive Server Response.
        //         data = new Byte[256];
        //         String response = String.Empty;
        //
        //         // Read the Tcp Server Response Bytes.
        //         Int32 bytes = stream.Read(data, 0, data.Length);
        //         response = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
        //         Console.WriteLine("Received: {0}", response);      
        //
        //         Thread.Sleep(2000);   
        //
        //         stream.Close();         
        //         client.Close();         
        //     } 
        //     catch (Exception e) 
        //     {
        //         Console.WriteLine("Exception: {0}", e);
        //     }
        //
        //     Console.Read();
        // }
    }
}