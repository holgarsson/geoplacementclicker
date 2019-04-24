using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GeoplacementClicker.Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataEntries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Command = table.Column<string>(nullable: true),
                    SequenceNumber = table.Column<int>(nullable: false),
                    TimeStamp = table.Column<int>(nullable: false),
                    Fcnt = table.Column<int>(nullable: false),
                    Port = table.Column<int>(nullable: false),
                    Frequence = table.Column<int>(nullable: false),
                    TOA = table.Column<int>(nullable: false),
                    DR = table.Column<string>(nullable: true),
                    ACK = table.Column<bool>(nullable: false),
                    SessionKeyId = table.Column<string>(nullable: true),
                    BAT = table.Column<int>(nullable: false),
                    Data = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gateways",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RSSI = table.Column<int>(nullable: false),
                    SNR = table.Column<int>(nullable: false),
                    TimeStamp = table.Column<int>(nullable: false),
                    TMMS = table.Column<int>(nullable: true),
                    Time = table.Column<DateTime>(nullable: true),
                    GWEUI = table.Column<string>(nullable: true),
                    Longitude = table.Column<decimal>(nullable: false),
                    Latitude = table.Column<decimal>(nullable: false),
                    DataEntryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gateways", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gateways_DataEntries_DataEntryId",
                        column: x => x.DataEntryId,
                        principalTable: "DataEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gateways_DataEntryId",
                table: "Gateways",
                column: "DataEntryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gateways");

            migrationBuilder.DropTable(
                name: "DataEntries");
        }
    }
}
