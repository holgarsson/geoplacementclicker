using Microsoft.EntityFrameworkCore.Migrations;

namespace GeoplacementClicker.Persistence.Migrations
{
    public partial class ModifiedPropertyTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "TimeStamp",
                table: "Gateways",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "SNR",
                table: "Gateways",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<long>(
                name: "TimeStamp",
                table: "DataEntries",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TimeStamp",
                table: "Gateways",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<int>(
                name: "SNR",
                table: "Gateways",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TimeStamp",
                table: "DataEntries",
                nullable: false,
                oldClrType: typeof(long));
        }
    }
}
