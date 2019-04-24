using System;
using System.Collections.Generic;
using System.Text;

namespace GeoplacementClicker.Persistence.Entities
{
    public class Gateway
    {
        public int Id { get; set; }

        public int RSSI { get; set; }

        public int SNR { get; set; }

        public int TimeStamp { get; set; }

        public int? TMMS { get; set; }

        public DateTime? Time { get; set; }

        public string GWEUI { get; set; }

        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }
        

        // Navigational properties
        public int DataEntryId { get; set; }
        public DataEntry DataEntry { get; set; }
    }
}
