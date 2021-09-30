using Microsoft.EntityFrameworkCore.Migrations;

namespace SmhiDb.Migrations
{
    public partial class IndexOnValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Values_Date_SmhiStationId",
                table: "Values",
                columns: new[] { "Date", "SmhiStationId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Values_Date_SmhiStationId",
                table: "Values");
        }
    }
}
