using Microsoft.EntityFrameworkCore.Migrations;

namespace GeoplacementClicker.Persistence.Migrations
{
    public partial class AddedEUIProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EUI",
                table: "DataEntries",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EUI",
                table: "DataEntries");
        }
    }
}
