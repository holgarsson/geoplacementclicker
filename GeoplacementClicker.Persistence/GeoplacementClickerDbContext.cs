using GeoplacementClicker.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoplacementClicker.Persistence
{
    public class GeoplacementClickerDbContext : DbContext
    {
        public GeoplacementClickerDbContext()
        {

        }

        public GeoplacementClickerDbContext(DbContextOptions<GeoplacementClickerDbContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=GeoplacementClickerDb;Trusted_Connection=True;ConnectRetryCount=0";
            optionsBuilder.UseSqlServer(connectionString);
        }

        public DbSet<DataEntry> DataEntries { get; set; }
        public DbSet<Gateway> Gateways { get; set; }
    }

}
