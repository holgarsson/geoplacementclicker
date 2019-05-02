using GeoplacementClicker.Persistence;
using GeoplacementClicker.Persistence.Entities;
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
        //private const string url = "wss://iotnet.teracom.dk/app?token=vnoRTgAAABFpb3RuZXQudGVyYWNvbS5ka4X59dCkx4K4rDGzSl93ZTU="; Gary

        private const string url = "wss://iotnet.teracom.dk/app?token=vnoRcgAAABFpb3RuZXQudGVyYWNvbS5ka9zqmsRR_ld4kZ3qMEKTzHQ"; // Oliver
        
        private ClientWebSocket ws = new ClientWebSocket();


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
                    string message = await OnReceive();

                }
            }
        }

        public async Task StopListening()
        {
            isListening = false;

            await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing web socket", CancellationToken.None);
        }

        private async Task<string> OnReceive()
        {
            ArraySegment<byte> receivedBytes = new ArraySegment<byte>(new byte[1024]);
            WebSocketReceiveResult result = await ws.ReceiveAsync(receivedBytes, CancellationToken.None);
            var resultString = Encoding.UTF8.GetString(receivedBytes.Array, 0, result.Count);

            // Deserializing json data to object  
            DataEntry jsonObject = JsonConvert.DeserializeObject<DataEntry>(resultString);

            if (jsonObject == null)
                return null;

            using (var dbContext = new GeoplacementClickerDbContext())
            {

                //DataEntry dataEntry = new DataEntry
                //{
                //    Command = jsonObject.cmd ?? string.Empty,
                //    SequenceNumber = jsonObject.seqno ? int.Parse(jsonObject.seqno) : 0,
                //    EUI = jsonObject.EUI ?? string.Empty,
                //    TimeStamp = jsonObject.ts ? int.Parse(jsonObject.ts) : 0,
                //    Fcnt = jsonObject.fcnt ? int.Parse(jsonObject.fcnt) : 0,
                //    Port = jsonObject.port ? int.Parse(jsonObject.port) : 0,
                //    Frequence = jsonObject.freq ? int.Parse(jsonObject.freq) : 0,
                //    TOA = jsonObject.toa ? int.Parse(jsonObject.toa) : 0,
                //    DR = jsonObject.dr ?? string.Empty,
                //    ACK = jsonObject.ack ? bool.Parse(jsonObject.ack) : 0,
                //    SessionKeyId = jsonObject.sessionKeyId ?? string.Empty,
                //    BAT = jsonObject.bat ? int.Parse(jsonObject.bat) : 0,
                //    Data = jsonObject.data ?? string.Empty
                //};

                dbContext.DataEntries.Add(jsonObject);

                await dbContext.SaveChangesAsync();
            };


            return resultString;
        }

        public Task<string> GetListeningUrl()
        {
            return Task.FromResult(url);
        }
    }
}
