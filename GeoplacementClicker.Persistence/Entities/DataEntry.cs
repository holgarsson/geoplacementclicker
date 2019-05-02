using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoplacementClicker.Persistence.Entities
{
    public class DataEntry
    {
        public int Id { get; set; }

        [JsonProperty(PropertyName = "cmd")]
        public string Command { get; set; }

        [JsonProperty(PropertyName = "seqno")]
        public int SequenceNumber { get; set; }

        [JsonProperty(PropertyName = "EUI")]
        public string EUI { get; set; }

        [JsonProperty(PropertyName = "ts")]
        public long TimeStamp { get; set; }

        [JsonProperty(PropertyName = "fcnt")]
        public int Fcnt { get; set; }

        [JsonProperty(PropertyName = "port")]
        public int Port { get; set; }

        [JsonProperty(PropertyName = "frew")]
        public int Frequence { get; set; }

        [JsonProperty(PropertyName = "toa")]
        public int TOA { get; set; }

        [JsonProperty(PropertyName = "dr")]
        public string DR { get; set; }

        [JsonProperty(PropertyName = "ack")]
        public bool ACK { get; set; }

        [JsonProperty(PropertyName = "sessionKeyId")]
        public string SessionKeyId { get; set; }

        [JsonProperty(PropertyName = "bat")]
        public int BAT { get; set; }

        [JsonProperty(PropertyName = "data")]
        public string Data { get; set; }

        [JsonProperty(PropertyName = "gws")]
        public IEnumerable<Gateway> Gateways { get; set; }
    }
}
