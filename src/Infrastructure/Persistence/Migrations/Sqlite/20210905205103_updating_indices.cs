using Microsoft.EntityFrameworkCore.Migrations;

namespace bike_selling_app.Infrastructure.Persistence.Migrations.Sqlite
{
    public partial class updating_indices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bikes_SerialNumber",
                table: "Bikes");

            migrationBuilder.CreateIndex(
                name: "IX_Bikes_SerialNumber_Make_Model",
                table: "Bikes",
                columns: new[] { "SerialNumber", "Make", "Model" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bikes_SerialNumber_Make_Model",
                table: "Bikes");

            migrationBuilder.CreateIndex(
                name: "IX_Bikes_SerialNumber",
                table: "Bikes",
                column: "SerialNumber",
                unique: true);
        }
    }
}
