using System;
using System.Collections.Generic;
using System.Text;

namespace GeoplacementClicker.Persistence.Entities
{
    public class DataEntry
    {
        public int Id { get; set; }

        public string Command { get; set; }

        public int SequenceNumber { get; set; }

        public int TimeStamp { get; set; }

        public int Fcnt { get; set; }

        public int Port { get; set; }

        public int Frequence { get; set; }

        public int TOA { get; set; }

        public string DR { get; set; }

        public bool ACK { get; set; }

        public string SessionKeyId { get; set; }

        public int BAT { get; set; }

        public string Data { get; set; }

        public IEnumerable<Gateway> Gateways { get; set; }
    }
}
