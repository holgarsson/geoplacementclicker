using GeoplacementClicker.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoplacementClicker.Persistence
{
    public class GeoplacementClickerDbContext : DbContext
    {
        public GeoplacementClickerDbContext(DbContextOptions<GeoplacementClickerDbContext> options)
            : base(options)
        { }

        public DbSet<DataEntry> DataEntries { get; set; }
        public DbSet<Gateway> Gateways { get; set; }
    }

}
