using GeoplacementClicker.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoplacementClicker.Web.Models.Home
{
    public class HomeViewModel
    {
        public IEnumerable<DataEntry> DataEntries { get; set; }
    }
}
