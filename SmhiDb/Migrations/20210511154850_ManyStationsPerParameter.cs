using Microsoft.EntityFrameworkCore.Migrations;

namespace SmhiDb.Migrations
{
    public partial class ManyStationsPerParameter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Stations_SmhiParameterId",
                table: "Stations");

            migrationBuilder.CreateIndex(
                name: "IX_Stations_SmhiParameterId",
                table: "Stations",
                column: "SmhiParameterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Stations_SmhiParameterId",
                table: "Stations");

            migrationBuilder.CreateIndex(
                name: "IX_Stations_SmhiParameterId",
                table: "Stations",
                column: "SmhiParameterId",
                unique: true);
        }
    }
}
