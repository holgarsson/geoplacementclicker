using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoplacementClicker.Persistence.Entities
{
    public class Gateway
    {
        public int Id { get; set; }

        [JsonProperty(PropertyName = "rssi")]
        public int RSSI { get; set; }

        [JsonProperty(PropertyName = "snr")]
        public string SNR { get; set; }

        [JsonProperty(PropertyName = "ts")]
        public long TimeStamp { get; set; }

        [JsonProperty(PropertyName = "tmms")]
        public int? TMMS { get; set; }

        [JsonProperty(PropertyName = "time")]
        public DateTime? Time { get; set; }

        [JsonProperty(PropertyName = "gweui")]
        public string GWEUI { get; set; }

        [JsonProperty(PropertyName = "lon")]
        public decimal Longitude { get; set; }

        [JsonProperty(PropertyName = "lat")]
        public decimal Latitude { get; set; }
        

        // Navigational properties
        public int DataEntryId { get; set; }
        public DataEntry DataEntry { get; set; }
    }
}
