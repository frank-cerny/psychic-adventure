using Microsoft.EntityFrameworkCore.Migrations;

namespace bike_selling_app.Infrastructure.Persistence.Migrations.MySql
{
    public partial class updating_noncapital_item : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Units",
                table: "NonCapitalItems",
                newName: "UnitsRemaining");

            migrationBuilder.AddColumn<int>(
                name: "UnitsPurchased",
                table: "NonCapitalItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitsPurchased",
                table: "NonCapitalItems");

            migrationBuilder.RenameColumn(
                name: "UnitsRemaining",
                table: "NonCapitalItems",
                newName: "Units");
        }
    }
}
