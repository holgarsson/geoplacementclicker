using GeoplacementClicker.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoplacementClicker.Web.Models.Home
{
    public class LocationViewModel
    {
        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }

        public int? DataEntryId { get; set; }
    }
}
