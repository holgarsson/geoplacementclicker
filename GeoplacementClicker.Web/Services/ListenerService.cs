using GeoplacementClicker.Persistence;
using GeoplacementClicker.Persistence.Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GeoplacementClicker.Web.Services
{
    public class ListenerService : IListenerService
    {
        private readonly string url = "wss://iotnet.teracom.dk/app?token=vnoRcgAAABFpb3RuZXQudGVyYWNvbS5ka9zqmsRR_ld4kZ3qMEKTzHQ";
        
        private ClientWebSocket ws = new ClientWebSocket();

        public ListenerService(IConfiguration configuration)
        {
            //url = configuration.GetValue<string>("WebsocketUrl");
        }


        private bool isListening { get; set; } = false;

        public async Task StartListening()
        {
            isListening = true;

            while (ws.State != WebSocketState.Open)
            {
                await ws.ConnectAsync(new Uri(url), CancellationToken.None);
                Console.WriteLine("Web socket : " + ws.State);

                while (isListening)
                {
                    await OnReceive();

                }
            }
        }

        public async Task StopListening()
        {
            isListening = false;

            await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing web socket", CancellationToken.None);
        }

        private async Task OnReceive()
        {
            ArraySegment<byte> receivedBytes = new ArraySegment<byte>(new byte[1024]);
            WebSocketReceiveResult result = await ws.ReceiveAsync(receivedBytes, CancellationToken.None);
            var resultString = Encoding.UTF8.GetString(receivedBytes.Array, 0, result.Count);

            // Deserializing json data to object  
            DataEntry jsonObject = JsonConvert.DeserializeObject<DataEntry>(resultString);
            if (jsonObject == null)
                return;

            if (jsonObject.Command.Equals("gw", StringComparison.InvariantCultureIgnoreCase))
                return;

            using (var dbContext = new GeoplacementClickerDbContext())
            {
                dbContext.DataEntries.Add(jsonObject);

                await dbContext.SaveChangesAsync();
            };
        }

        public Task<string> GetListeningUrl()
        {
            return Task.FromResult(url);
        }
    }
}
