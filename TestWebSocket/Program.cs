using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebSockets;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Text;
using Newtonsoft.Json;

namespace TestWebSocket
{
    class Program
    {
        static ClientWebSocket webSocket;
        static void Main(string[] args)
        {
            webSocket = new ClientWebSocket();

            var url = "wss://localhost:5001/ws";

            ConnectWebSocket(url);
            if (webSocket.State != WebSocketState.Open)
                return;
            SendMessage("Hello World");
            var task =  ReadMessage();
            task.Wait();
            //webSocket.SendAsy
            // var ws = new WebSocket("wss://localhost:5001/ws");
            //ws.Connect();
            //ws.ConnectAsync();
            //while(ws.ReadyState == WebSocketState.Connecting)
            //{
            //    Console.WriteLine("Trying to connect");
            //}
            //Console.WriteLine("Connected");
        }

        public static void ConnectWebSocket(string url)
        {
            //var canToken = CancellationToken.None;
            var task = webSocket.ConnectAsync(new Uri(url), CancellationToken.None);
            task.Wait();
            while(webSocket.State == WebSocketState.Connecting)
            {
                Console.WriteLine("Trying to connect");
            }
            Console.WriteLine("Connected");
        }

        public static void SendMessage(string message)
        {
            var arraySegment = new ArraySegment<byte>(Encoding.ASCII.GetBytes(message));

            var task = webSocket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);

            task.Wait();

        }
        public static async Task ReadMessage()
        {
            var buffer = new byte[4096 * 20];

            while (webSocket.State == WebSocketState.Open)
            {
               var response = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

               // task.Wait();

               // var response = task.Result as WebSocketReceiveResult;

                if (response.MessageType == WebSocketMessageType.Close)
                {
                    await
                        webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Close response received",
                            CancellationToken.None);
                    //closeTask.Wait();
                }
                else
                {
                    var result = Encoding.UTF8.GetString(buffer);
                    var a = buffer[1000];
                    var b = buffer[10000];
                    var c = buffer[50000];
                    var d = buffer[81000];
                    Console.WriteLine(result);
                    //var responseObject = JsonConvert.DeserializeObject<Response>(result, _requestParameters.ResponseDataType);

                    //OnSocketReceive.Invoke(this, new SocketEventArgs { Response = responseObject });
                    buffer = new byte[4096 * 20];
                }
            }
        }

    }
}
