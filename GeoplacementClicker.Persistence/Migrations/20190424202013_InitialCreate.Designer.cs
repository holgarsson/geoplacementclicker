﻿// <auto-generated />
using System;
using GeoplacementClicker.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GeoplacementClicker.Persistence.Migrations
{
    [DbContext(typeof(GeoplacementClickerDbContext))]
    [Migration("20190424202013_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GeoplacementClicker.Persistence.Entities.DataEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("ACK");

                    b.Property<int>("BAT");

                    b.Property<string>("Command");

                    b.Property<string>("DR");

                    b.Property<string>("Data");

                    b.Property<int>("Fcnt");

                    b.Property<int>("Frequence");

                    b.Property<int>("Port");

                    b.Property<int>("SequenceNumber");

                    b.Property<string>("SessionKeyId");

                    b.Property<int>("TOA");

                    b.Property<int>("TimeStamp");

                    b.HasKey("Id");

                    b.ToTable("DataEntries");
                });

            modelBuilder.Entity("GeoplacementClicker.Persistence.Entities.Gateway", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DataEntryId");

                    b.Property<string>("GWEUI");

                    b.Property<decimal>("Latitude");

                    b.Property<decimal>("Longitude");

                    b.Property<int>("RSSI");

                    b.Property<int>("SNR");

                    b.Property<int?>("TMMS");

                    b.Property<DateTime?>("Time");

                    b.Property<int>("TimeStamp");

                    b.HasKey("Id");

                    b.HasIndex("DataEntryId");

                    b.ToTable("Gateways");
                });

            modelBuilder.Entity("GeoplacementClicker.Persistence.Entities.Gateway", b =>
                {
                    b.HasOne("GeoplacementClicker.Persistence.Entities.DataEntry", "DataEntry")
                        .WithMany("Gateways")
                        .HasForeignKey("DataEntryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
